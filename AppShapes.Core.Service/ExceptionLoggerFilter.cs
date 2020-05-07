using AppShapes.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Service
{
    public class ExceptionLoggerFilter : IExceptionFilter
    {
        public ExceptionLoggerFilter(ILogger<ExceptionLoggerFilter> logger)
        {
            Logger = logger;
        }

        public virtual void OnException(ExceptionContext context)
        {
            Logger.Error<ExceptionLoggerFilter>(context.Exception, $"{context.Exception.Message} (Handled: ${context.ExceptionHandled})");
        }

        protected ILogger<ExceptionLoggerFilter> Logger { get; }
    }
}