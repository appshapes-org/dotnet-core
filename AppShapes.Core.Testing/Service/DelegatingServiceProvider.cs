using System;

namespace AppShapes.Core.Testing.Service
{
    public class DelegatingServiceProvider : IServiceProvider, IDisposable
    {
        public DelegatingServiceProvider(Func<Type, object> getServiceFunction, Action disposeAction)
        {
            GetServiceFunction = getServiceFunction;
            DisposeAction = disposeAction;
        }

        public void Dispose()
        {
            DisposeAction();
            IsDisposed = true;
        }

        public object GetService(Type serviceType)
        {
            return GetServiceFunction(serviceType);
        }

        public bool IsDisposed { get; private set; }

        private Action DisposeAction { get; set; }

        private Func<Type, object> GetServiceFunction { get; }
    }
}