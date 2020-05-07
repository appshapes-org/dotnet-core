using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Logging
{
    public class LoggingContext
    {
        protected LoggingContext( /* serialization */)
        {
        }

        public LoggingContext(string loggerName, LogLevel logLevel, string memberName)
        {
            LoggerName = loggerName;
            LogLevel = logLevel;
            MemberName = memberName;
            ApplicationName = GetApplicationName();
            EnvironmentName = GetEnvironmentName();
            ThreadName = GetThreadName();
            ActivityId = GetActivityId();
            UserId = GetUserId();
            OrganizationId = GetOrganizationId();
        }

        public string ActivityId { get; set; }

        public string ApplicationName { get; set; }

        public string EnvironmentName { get; set; }

        public string Exception { get; set; }

        public string LoggerName { get; set; }

        public LogLevel LogLevel { get; set; }

        public string MemberName { get; set; }

        public string Message { get; set; }

        public string OrganizationId { get; set; }

        public string ThreadName { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions {IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }

        public string UserId { get; set; }

        protected virtual string GetActivityId()
        {
            return new RuntimeContext().ActivityId;
        }

        protected virtual string GetApplicationName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        protected virtual string GetEnvironmentName()
        {
            return new RuntimeContext().EnvironmentName;
        }

        protected virtual string GetOrganizationId()
        {
            return new RuntimeContext().OrganizationId;
        }

        protected virtual string GetThreadName()
        {
            return new RuntimeContext().ThreadName;
        }

        protected virtual string GetUserId()
        {
            return new RuntimeContext().UserId;
        }
    }
}