using System.Collections.Generic;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ActionLoggerFilterTests
    {
        [Fact]
        public void OnActionExecutedMustLogInformationWhenCalled()
        {
            MockLogger<ActionLoggerFilter> logger = new MockLogger<ActionLoggerFilter>();
            ControllerActionDescriptor descriptor = new ControllerActionDescriptor {DisplayName = "Test"};
            new ActionLoggerFilter(logger).OnActionExecuted(new ActionExecutedContext(new ActionContext(new DefaultHttpContext(), new RouteData(), descriptor, new ModelStateDictionary()), new List<IFilterMetadata>(), null) {ActionDescriptor = descriptor});
            Assert.Contains(logger.Storage, x => x.StartsWith($"{LogLevel.Information}: {descriptor.DisplayName}"));
        }

        [Fact]
        public void OnActionExecutingMustLogInformationWhenCalled()
        {
            MockLogger<ActionLoggerFilter> logger = new MockLogger<ActionLoggerFilter>();
            ControllerActionDescriptor descriptor = new ControllerActionDescriptor {DisplayName = "Test"};
            new ActionLoggerFilter(logger).OnActionExecuting(new ActionExecutingContext(new ActionContext(new DefaultHttpContext(), new RouteData(), descriptor, new ModelStateDictionary()), new List<IFilterMetadata>(), new Dictionary<string, object>(), null) {ActionDescriptor = descriptor});
            Assert.Contains(logger.Storage, x => x.StartsWith($"{LogLevel.Information}: {descriptor.DisplayName}"));
        }
    }
}