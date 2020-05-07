using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AppShapes.Core.Logging
{
    public class RuntimeContext
    {
        private static string theApplicationName;
        private static string theEnvironmentName;

        public RuntimeContext()
        {
            ThreadName = GetThreadName();
            ActivityId = GetActivityId();
            UserId = GetUserId();
            OrganizationId = GetOrganizationId();
        }

        public string ActivityId { get; }

        public virtual string ApplicationName => theApplicationName ??= GetApplicationName();

        public virtual string EnvironmentName => theEnvironmentName ??= GetEnvironmentName();

        public string OrganizationId { get; set; }

        public virtual string ThreadName { get; }

        public string UserId { get; set; }

        protected virtual string GetActivityId()
        {
            return string.IsNullOrWhiteSpace(Activity.Current?.Id) ? string.Empty : Activity.Current.Id;
        }

        protected virtual string GetApplicationName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        protected virtual List<string> GetEnvironmentKeys()
        {
            return new List<string> {"DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT"};
        }

        protected virtual string GetEnvironmentName()
        {
            string key = Environment.GetEnvironmentVariables().Keys.Cast<string>().FirstOrDefault(IsEnvironmentKey);
            return key == null ? string.Empty : Environment.GetEnvironmentVariable(key);
        }

        protected virtual string GetOrganizationId()
        {
            string value = Activity.Current?.GetBaggageItem("OrganizationId");
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        protected virtual string GetThreadName()
        {
            return IsThreadNamed(Thread.CurrentThread) ? Thread.CurrentThread.Name : $"{Thread.CurrentThread.ManagedThreadId}";
        }

        protected virtual string GetUserId()
        {
            string value = Activity.Current?.GetBaggageItem("UserId");
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        protected virtual bool IsEnvironmentKey(string key)
        {
            List<string> keys = GetEnvironmentKeys();
            return keys.Exists(x => string.Equals(x, key, StringComparison.OrdinalIgnoreCase));
        }

        protected virtual bool IsThreadNamed(Thread thread)
        {
            return !string.IsNullOrWhiteSpace(thread.Name);
        }
    }
}