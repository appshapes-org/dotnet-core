using System;
using AppShapes.Core.Messaging;

namespace AppShapes.Core.Testing.Messaging
{
    public class TestEvent : MessageBase
    {
        public override Guid GetEntityId()
        {
            return TestId;
        }

        public Guid TestId { get; set; } = Guid.NewGuid();

        protected override string GetContext()
        {
            return "Testing";
        }

        protected override string GetEntityName()
        {
            return "Test";
        }

        protected override string GetMessageType()
        {
            return "TestEvent";
        }
    }
}