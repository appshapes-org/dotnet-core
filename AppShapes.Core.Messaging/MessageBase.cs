using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppShapes.Core.Domain;

namespace AppShapes.Core.Messaging
{
    /// <summary>
    ///     MessageBase uses the following naming conventions:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>Context</term><description>first part of namespace (e.g., OrderManagement)</description>
    ///         </item>
    ///         <item>
    ///             <term>Type</term><description>type name (e.g., OrderCreatedEvent)</description>
    ///         </item>
    ///         <item>
    ///             <term>Entity</term><description>type name minus action and type (e.g., Order)</description>
    ///         </item>
    ///     </list>
    ///     <remarks>
    ///         Override <see cref="GetContext" />, <see cref="GetMessageType" />, and/or <see cref="GetEntityName" /> to
    ///         customize naming convention.
    ///     </remarks>
    /// </summary>
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

        protected virtual string GetContext()
        {
            string nameSpace = GetNamespace();
            return string.IsNullOrWhiteSpace(nameSpace) ? Entity : nameSpace.Split('.')[0];
        }

        protected virtual string GetEntityName()
        {
            string[] words = GetType().Name.WordsFromCamelCase();
            return string.Join(null, words.Take(words.Length - 2));
        }

        protected virtual string GetMessageType()
        {
            return GetType().Name;
        }

        protected virtual string GetNamespace()
        {
            return GetType().Namespace;
        }

        protected virtual string GetVersion()
        {
            return "v1.0";
        }
    }
}