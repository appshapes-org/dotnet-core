using System;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Testing.Service
{
    public class MockServiceScope : IServiceScope
    {
        public MockServiceScope(IServiceProvider serviceProvider, Action disposeAction = null)
        {
            ServiceProvider = serviceProvider;
            DisposeAction = disposeAction ?? (() => { });
        }

        public void Dispose()
        {
            DisposeAction();
        }

        public IServiceProvider ServiceProvider { get; }

        private Action DisposeAction { get; }
    }
}