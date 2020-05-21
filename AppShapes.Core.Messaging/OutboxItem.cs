using System;
using System.Collections.Generic;
using AppShapes.Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AppShapes.Core.Messaging
{
    public class OutboxItem
    {
        protected OutboxItem( /* serialization */)
        {
        }

        public OutboxItem(MessageBase message)
        {
            Context = message.Context;
            CorrelationId = message.CorrelationId;
            Id = message.Id;
            EntityId = message.GetEntityId();
            Entity = message.Entity;
            Message = SerializeMessage(message);
            Timestamp = message.Timestamp;
            Type = message.Type;
        }

        public string Context { get; set; }

        public string CorrelationId { get; set; }

        public string Entity { get; set; }

        public Guid EntityId { get; set; }

        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(EntityId)}: {EntityId}, {nameof(Context)}: {Context}, {nameof(Entity)}: {Entity}, {nameof(CorrelationId)}: {CorrelationId}, {nameof(Id)}: {Id}, {nameof(Timestamp)}: {Timestamp}";
        }

        public string Type { get; set; }

        public virtual void UpdateMessage(NonEmptyString name, string value)
        {
            IDictionary<string, JToken> document = JObject.Parse(Message);
            if (!document.ContainsKey(name))
                throw new ArgumentException($"{Type}.{name} not found; not setting to: {value}");
            document[name] = value;
            Message = SerializeMessage(document);
        }

        protected virtual string SerializeMessage(object message)
        {
            return JsonConvert.SerializeObject(message, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver(), DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Include});
        }
    }
}