using System;
using AppShapes.Core.Messaging;

namespace AppShapes.Core.Testing.Messaging
{
    public class TestCreatedEvent : MessageBase
    {
        public override Guid GetEntityId()
        {
            return TestId;
        }

        public Guid TestId { get; set; } = Guid.NewGuid();
    }
}