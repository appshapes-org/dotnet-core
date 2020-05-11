using System.Collections.Generic;
using AppShapes.Core.Testing.Messaging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Messaging
{
    public class TestCreatedEventTests
    {
        [Fact]
        public void GetContextMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal(nameof(AppShapes), new StubTestCreatedEvent().InvokeGetContext());
        }

        [Fact]
        public void GetEntityIdMustReturnTestIdWhenCalled()
        {
            TestCreatedEvent e = new TestCreatedEvent();
            Assert.Equal(e.TestId, e.GetEntityId());
        }

        [Fact]
        public void GetEntityNameMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal("StubTest", new StubTestCreatedEvent().InvokeGetEntityName());
        }

        [Fact]
        public void GetMessageTypeMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal(nameof(StubTestCreatedEvent), new StubTestCreatedEvent().InvokeGetMessageType());
        }

        [Fact]
        public void GetVersionMustReturnExpectedStringWhenCalled()
        {
            Assert.Equal("v1.0", new StubTestCreatedEvent().Version);
        }

        private class StubTestCreatedEvent : TestCreatedEvent
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