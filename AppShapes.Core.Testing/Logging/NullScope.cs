using System;

namespace AppShapes.Core.Testing.Logging
{
    public class NullScope : IDisposable
    {
        private NullScope()
        {
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public static NullScope Instance { get; } = new NullScope();

        public bool IsDisposed { get; private set; }
    }
}