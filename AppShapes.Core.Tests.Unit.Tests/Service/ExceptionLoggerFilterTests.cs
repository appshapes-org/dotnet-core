using System;
using System.Collections.Generic;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ExceptionLoggerFilterTests
    {
        [Fact]
        public void OnExceptionMustLogErrorWhenCalled()
        {
            MockLogger<ExceptionLoggerFilter> logger = new MockLogger<ExceptionLoggerFilter>();
            new ExceptionLoggerFilter(logger).OnException(new ExceptionContext(new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()), new List<IFilterMetadata>()) {Exception = new Exception("Test"), ExceptionHandled = true});
            Assert.Contains(logger.Storage, x => x.StartsWith("Error: Test"));
        }
    }
}