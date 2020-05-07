using AppShapes.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Service
{
    public class ActionLoggerFilter : IActionFilter
    {
        public ActionLoggerFilter(ILogger<ActionLoggerFilter> logger)
        {
            Logger = logger;
        }

        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.Information<ActionLoggerFilter>(context.ActionDescriptor.DisplayName);
        }

        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.Information<ActionLoggerFilter>(context.ActionDescriptor.DisplayName);
        }

        protected ILogger<ActionLoggerFilter> Logger { get; }
    }
}