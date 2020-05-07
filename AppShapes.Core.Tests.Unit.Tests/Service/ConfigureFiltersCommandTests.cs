using AppShapes.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureFiltersCommandTests
    {
        [Fact]
        public void ExecuteMustAddActionLoggerFilterWhenCalled()
        {
            FilterCollection filters = new FilterCollection();
            new ConfigureFiltersCommand().Execute(filters);
            Assert.Contains(filters, x => (x as TypeFilterAttribute)?.ImplementationType == typeof(ActionLoggerFilter));
        }

        [Fact]
        public void ExecuteMustAddExceptionLoggerFilterWhenCalled()
        {
            FilterCollection filters = new FilterCollection();
            new ConfigureFiltersCommand().Execute(filters);
            Assert.Contains(filters, x => (x as TypeFilterAttribute)?.ImplementationType == typeof(ExceptionLoggerFilter));
        }
    }
}