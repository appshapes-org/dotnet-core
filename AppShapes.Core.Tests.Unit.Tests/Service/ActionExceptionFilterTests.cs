using System.Collections.Generic;
using AppShapes.Core.Domain.Exceptions;
using AppShapes.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ActionExceptionFilterTests
    {
        [Fact]
        public void OnActionExecutedMustNotSetResultWhenContextExceptionIsNotNotFoundObjectResult()
        {
            ControllerActionDescriptor descriptor = new ControllerActionDescriptor {DisplayName = "Test"};
            ActionExecutedContext context = new ActionExecutedContext(new ActionContext(new DefaultHttpContext(), new RouteData(), descriptor, new ModelStateDictionary()), new List<IFilterMetadata>(), null);
            new ActionExceptionFilter().OnActionExecuted(context);
            Assert.Null(context.Result);
            Assert.False(context.ExceptionHandled);
        }

        [Fact]
        public void OnActionExecutedMustSetResultWhenContextExceptionIsNotFoundObjectResult()
        {
            ControllerActionDescriptor descriptor = new ControllerActionDescriptor {DisplayName = "Test"};
            ActionExecutedContext context = new ActionExecutedContext(new ActionContext(new DefaultHttpContext(), new RouteData(), descriptor, new ModelStateDictionary()), new List<IFilterMetadata>(), null) {Exception = new NotFoundException("42")};
            new ActionExceptionFilter().OnActionExecuted(context);
            NotFoundObjectResult actual = Assert.IsAssignableFrom<NotFoundObjectResult>(context.Result);
            Assert.Equal("42", actual.Value);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnActionExecutingMustDoNothingWhenCalled()
        {
            ControllerActionDescriptor descriptor = new ControllerActionDescriptor {DisplayName = "Test"};
            ActionExecutingContext context = new ActionExecutingContext(new ActionContext(new DefaultHttpContext(), new RouteData(), descriptor, new ModelStateDictionary()), new List<IFilterMetadata>(), new Dictionary<string, object>(), null) {ActionDescriptor = descriptor};
            Assert.Null(context.Result);
            new ActionExceptionFilter().OnActionExecuting(context);
            Assert.Null(context.Result);
        }
    }
}