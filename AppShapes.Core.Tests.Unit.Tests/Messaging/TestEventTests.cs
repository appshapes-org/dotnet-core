using System.Collections.Generic;
using AppShapes.Core.Testing.Messaging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Messaging
{
    public class TestEventTests
    {
        [Fact]
        public void GetContextMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal("Testing", new StubTestEvent().InvokeGetContext());
        }

        [Fact]
        public void GetEntityIdMustReturnTestIdWhenCalled()
        {
            TestEvent e = new TestEvent();
            Assert.Equal(e.TestId, e.GetEntityId());
        }

        [Fact]
        public void GetEntityNameMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal("Test", new StubTestEvent().InvokeGetEntityName());
        }

        [Fact]
        public void GetMessageTypeMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal("TestEvent", new StubTestEvent().InvokeGetMessageType());
        }

        private class StubTestEvent : TestEvent
        {
            public IEnumerable<char> InvokeGetContext()
            {
                return GetContext();
            }

            public IEnumerable<char> InvokeGetEntityName()
            {
                return GetEntityName();
            }

            public IEnumerable<char> InvokeGetMessageType()
            {
                return GetMessageType();
            }
        }
    }
}