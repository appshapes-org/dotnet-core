using System.Collections.Generic;
using AppShapes.Core.Console;
using AppShapes.Core.Database;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Dispatcher
{
    public abstract class DispatcherProgram<T> : DatabaseProgram<T> where T : OutboxContext
    {
        protected DispatcherProgram(IEnumerable<string> args) : base(args)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            new ConfigureDispatcherCommand().Execute<T>(services, Configuration);
        }
    }
}