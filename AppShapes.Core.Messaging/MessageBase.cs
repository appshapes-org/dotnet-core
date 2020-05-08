using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AppShapes.Core.Domain;

namespace AppShapes.Core.Messaging
{
    public abstract class MessageBase
    {
        private string itsContext;
        private string itsName;
        private string itsType;
        private string itsVersion;

        [Required]
        [Description("Bounded Context")]
        public string Context { get => itsContext ??= GetContext(); set => itsContext = value; }

        [Required]
        [Description("Activity/Tracing Id")]
        public string CorrelationId { get; set; }

        [Required]
        [Description("Entity Name")]
        public string Entity { get => itsName ??= GetEntityName(); set => itsName = value; }

        public abstract Guid GetEntityId();

        [Required]
        [Description("Message Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Description("Message Timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        [Description("Message Type")]
        public string Type { get => itsType ??= GetMessageType(); set => itsType = value; }

        [Required]
        [Description("Type Version")]
        public string Version { get => itsVersion ??= new SemanticVersion(GetVersion()).Value; set => itsVersion = new SemanticVersion(value).Value; }

        protected abstract string GetContext();

        protected abstract string GetEntityName();

        protected abstract string GetMessageType();

        protected virtual string GetVersion()
        {
            return "v1.0";
        }
    }
}