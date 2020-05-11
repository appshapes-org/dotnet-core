using System;
using AppShapes.Core.Messaging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Messaging
{
    public class MessageBaseTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("\t")]
        public void ContextMustReturnEntityWhenNamespaceIsNotSet(string value)
        {
            Assert.Equal("FakeEntity", new FakeEntityCreatedEvent(value).Context);
        }

        [Theory]
        [InlineData("FakeContext")]
        [InlineData("FakeContext.Domain.Events")]
        public void ContextMustReturnFirstPartOfNamespaceWhenNamespaceIsSet(string value)
        {
            Assert.Equal("FakeContext", new FakeEntityCreatedEvent(value).Context);
        }

        private class FakeEntityCreatedEvent : MessageBase
        {
            private string itsNameSpace;

            public FakeEntityCreatedEvent(string nameSpace)
            {
                itsNameSpace = nameSpace;
            }

            public override Guid GetEntityId()
            {
                return Guid.NewGuid();
            }

            protected override string GetNamespace()
            {
                base.GetNamespace();
                return itsNameSpace;
            }
        }
    }
}