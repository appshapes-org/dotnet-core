using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AppShapes.Core.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Logging
{
    public class RuntimeContextTests
    {
        [Fact]
        public void ActivityIdMustReturnActivityIdWhenCurrentActivityIsNotNull()
        {
            Activity activity = new Activity(nameof(ActivityIdMustReturnActivityIdWhenCurrentActivityIsNotNull)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                Assert.False(string.IsNullOrWhiteSpace(new RuntimeContext().ActivityId));
                Assert.Equal(activity.Id, new RuntimeContext().ActivityId);
            }
        }

        [Fact]
        public void ActivityIdMustReturnEmptyWhenCurrentActivityIsNull()
        {
            Assert.Equal(string.Empty, new RuntimeContext().ActivityId);
        }

        [Fact]
        public void ApplicationNameMustNotCallGetApplicationNameOnceApplicationNameHasBeenDetermined()
        {
            MockRuntimeContext context = new MockRuntimeContext();
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, context.ApplicationName);
            int expected = context.GetApplicationNameCalled;
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, context.ApplicationName);
            Assert.Equal(expected, context.GetApplicationNameCalled);
        }

        [Fact]
        public void ApplicationNameMustReturnCurrentDomainFriendlyNameWhenCalled()
        {
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, new RuntimeContext().ApplicationName);
        }

        [Fact]
        public void EnvironmentNameMustNotCallGetEnvironmentNameOnceEnvironmentNameHasBeenDetermined()
        {
            MockRuntimeContext context = new MockRuntimeContext();
            Assert.Equal(string.Empty, context.EnvironmentName);
            int expected = context.GetEnvironmentNameCalled;
            Assert.Equal(string.Empty, context.EnvironmentName);
            Assert.Equal(expected, context.GetEnvironmentNameCalled);
        }

        [Fact]
        public void EnvironmentNameMustReturnEmptyWhenEnvironmentVariableIsNotSet()
        {
            Assert.Equal(string.Empty, new RuntimeContext().EnvironmentName);
        }

        [Fact]
        public void GetEnvironmentKeysMustReturnExpectedSetOfEnvironmentKeysWhenCalled()
        {
            Assert.Contains(new MockRuntimeContext().InvokeGetEnvironmentKeys(), x => x == "DOTNET_ENVIRONMENT");
            Assert.Contains(new MockRuntimeContext().InvokeGetEnvironmentKeys(), x => x == "ASPNETCORE_ENVIRONMENT");
        }

        [Fact]
        public void GetEnvironmentNameMustReturnEmptyWhenEnvironmentVariablesDoesNotContainEnvironmentName()
        {
            Assert.Equal(string.Empty, new MockRuntimeContext().InvokeGetEnvironmentName());
        }

        [Fact]
        public void GetEnvironmentNameMustReturnEnvironmentNameWhenEnvironmentVariableIsSet()
        {
            Environment.SetEnvironmentVariable(nameof(GetEnvironmentNameMustReturnEnvironmentNameWhenEnvironmentVariableIsSet), "Test", EnvironmentVariableTarget.Process);
            Assert.Equal("Test", new MockRuntimeContext(nameof(GetEnvironmentNameMustReturnEnvironmentNameWhenEnvironmentVariableIsSet)).InvokeGetEnvironmentName());
        }

        [Fact]
        public void OrganizationIdMustReturnEmptyWhenCurrentActivityDoesNotContainsOrganizationId()
        {
            Activity activity = new Activity(nameof(OrganizationIdMustReturnEmptyWhenCurrentActivityDoesNotContainsOrganizationId)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                Assert.Equal(string.Empty, new RuntimeContext().OrganizationId);
            }
        }

        [Fact]
        public void OrganizationIdMustReturnOrganizationIdWhenCurrentActivityContainsOrganizationId()
        {
            Activity activity = new Activity(nameof(OrganizationIdMustReturnOrganizationIdWhenCurrentActivityContainsOrganizationId)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                activity.AddBaggage("OrganizationId", $"{42}");
                Assert.Equal("42", new RuntimeContext().OrganizationId);
            }
        }

        [Fact]
        public void ThreadNameMustReturnCurrentThreadIdWhenCurrentThreadNameIsNullOrWhitespace()
        {
            Assert.True(string.IsNullOrWhiteSpace(Thread.CurrentThread.Name));
            Assert.Equal($"{Thread.CurrentThread.ManagedThreadId}", new RuntimeContext().ThreadName);
        }

        [Fact]
        public void ThreadNameMustReturnCurrentThreadNameWhenCurrentThreadNameIsNotNullOrWhitespace()
        {
            bool result = false;
            Thread thread = new Thread(() =>
            {
                try
                {
                    result = new RuntimeContext().ThreadName == "Test";
                }
                catch
                {
                    result = false;
                }
            }) {IsBackground = true, Name = "Test"};
            thread.Start();
            thread.Join();
            Assert.True(result);
        }

        [Fact]
        public void UserIdMustReturnEmptyWhenCurrentActivityDoesNotContainsUserId()
        {
            Activity activity = new Activity(nameof(UserIdMustReturnEmptyWhenCurrentActivityDoesNotContainsUserId)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                Assert.Equal(string.Empty, new RuntimeContext().UserId);
            }
        }

        [Fact]
        public void UserIdMustReturnUserIdWhenCurrentActivityContainsUserId()
        {
            Activity activity = new Activity(nameof(UserIdMustReturnUserIdWhenCurrentActivityContainsUserId)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                activity.AddBaggage("UserId", $"{42}");
                Assert.Equal("42", new RuntimeContext().UserId);
            }
        }

        private class Disposable : IDisposable
        {
            public Disposable(Action disposeAction)
            {
                DisposeAction = disposeAction;
            }

            public void Dispose()
            {
                DisposeAction();
            }

            private Action DisposeAction { get; }
        }

        private class MockRuntimeContext : RuntimeContext
        {
            public MockRuntimeContext(params string[] environmentKeys)
            {
                EnvironmentKeys = environmentKeys.ToList();
            }

            public int GetApplicationNameCalled { get; private set; }

            public int GetEnvironmentNameCalled { get; private set; }

            public List<string> InvokeGetEnvironmentKeys()
            {
                return GetEnvironmentKeys();
            }

            public string InvokeGetEnvironmentName()
            {
                return GetEnvironmentName();
            }

            protected override string GetApplicationName()
            {
                ++GetApplicationNameCalled;
                return base.GetApplicationName();
            }

            protected override List<string> GetEnvironmentKeys()
            {
                return EnvironmentKeys.Count > 0 ? EnvironmentKeys : base.GetEnvironmentKeys();
            }

            protected override string GetEnvironmentName()
            {
                ++GetEnvironmentNameCalled;
                return base.GetEnvironmentName();
            }

            private List<string> EnvironmentKeys { get; }
        }
    }
}