using Microsoft.AspNetCore.Mvc.Filters;

namespace AppShapes.Core.Service
{
    public class ConfigureFiltersCommand
    {
        public virtual void Execute(FilterCollection filters)
        {
            filters.Add<ExceptionLoggerFilter>();
            filters.Add<ActionLoggerFilter>();
            filters.Add<ActionExceptionFilter>();
        }
    }
}