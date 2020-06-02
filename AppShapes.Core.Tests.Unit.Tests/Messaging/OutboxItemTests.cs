using System;
using AppShapes.Core.Messaging;
using AppShapes.Core.Testing.Messaging;
using Newtonsoft.Json;
using Xunit;

// ReSharper disable RedundantBaseConstructorCall
namespace AppShapes.Core.Tests.Unit.Tests.Messaging
{
    public class OutboxItemTests
    {
        [Fact]
        public void ConstructorMustNotSetPropertiesWhenDefaultConstructorCalled()
        {
            StubOutboxItem outboxItem = new StubOutboxItem();
            Assert.Equal(default, outboxItem.CorrelationId);
            Assert.Equal(default, outboxItem.Entity);
            Assert.Equal(default, outboxItem.EntityId);
            Assert.Equal(default, outboxItem.Id);
            Assert.Equal(default, outboxItem.Message);
            Assert.Equal(default, outboxItem.Timestamp);
            Assert.Equal(default, outboxItem.Type);
        }

        [Fact]
        public void ConstructorMustSerializeMessageThatMatchesOriginalMessageWhenDeserialized()
        {
            TestCreatedEvent expected = new TestCreatedEvent();
            TestCreatedEvent actual = JsonConvert.DeserializeObject<TestCreatedEvent>(new OutboxItem(expected).Message);
            Assert.Equal(expected.CorrelationId, actual.CorrelationId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.GetEntityId(), actual.GetEntityId());
            Assert.Equal(expected.Entity, actual.Entity);
            Assert.Equal(expected.Timestamp, actual.Timestamp);
            Assert.Equal(expected.Type, actual.Type);
        }

        [Fact]
        public void ConstructorMustSetPropertiesFromMessageWhenCalled()
        {
            TestCreatedEvent testEvent = new TestCreatedEvent();
            StubOutboxItem outboxItem = new StubOutboxItem(testEvent);
            Assert.Equal(testEvent.CorrelationId, outboxItem.CorrelationId);
            Assert.Equal(testEvent.Id, outboxItem.Id);
            Assert.Equal(testEvent.GetEntityId(), outboxItem.EntityId);
            Assert.Equal(testEvent.Entity, outboxItem.Entity);
            Assert.Equal(outboxItem.InvokeSerializeMessage(testEvent), outboxItem.Message);
            Assert.Equal(testEvent.Timestamp, outboxItem.Timestamp);
            Assert.Equal(testEvent.Type, outboxItem.Type);
        }

        [Fact]
        public void ToStringMustReturnExpectedValueWhenCalled()
        {
            TestCreatedEvent message = new TestCreatedEvent {CorrelationId = "42"};
            Assert.Equal($"Type: {message.Type}, EntityId: {message.GetEntityId()}, Context: {message.Context}, Entity: {message.Entity}, CorrelationId: {message.CorrelationId}, Id: {message.Id}, Timestamp: {message.Timestamp}", new OutboxItem(message).ToString());
        }

        [Fact]
        public void UpdateMessageMustThrowExceptionWhenMessageDoesNotContainProperty()
        {
            Assert.Equal($"{nameof(TestCreatedEvent)}.DoesNotExist not found; not setting to: 42", Assert.Throws<ArgumentException>(() => new OutboxItem(new TestCreatedEvent()).UpdateMessage("DoesNotExist", "42")).Message);
        }

        [Fact]
        public void UpdateMessageMustUpdatePropertyWhenMessageContainsProperty()
        {
            OutboxItem outboxItem = new OutboxItem(new TestCreatedEvent());
            outboxItem.UpdateMessage("correlationId", "42");
            TestCreatedEvent testEvent = JsonConvert.DeserializeObject<TestCreatedEvent>(outboxItem.Message);
            Assert.Equal("42", testEvent.CorrelationId);
        }

        private class StubOutboxItem : OutboxItem
        {
            public StubOutboxItem() : base()
            {
            }

            public StubOutboxItem(MessageBase message) : base(message)
            {
            }

            public string InvokeSerializeMessage(MessageBase message)
            {
                return SerializeMessage(message);
            }
        }
    }
}