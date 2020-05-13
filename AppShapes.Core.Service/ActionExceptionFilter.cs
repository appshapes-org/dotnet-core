using AppShapes.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppShapes.Core.Service
{
    public class ActionExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            HandleNotFoundException(context);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        protected virtual bool HandleNotFoundException(ActionExecutedContext context)
        {
            if (!(context.Exception is NotFoundException exception))
                return false;
            context.Result = new NotFoundObjectResult(exception.Message);
            context.ExceptionHandled = true;
            return true;
        }
    }
}