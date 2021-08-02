using System;
using System.IO;
using System.Reflection;

namespace TestAdapterCore.Infrastructure
{
    public class TestAssemblyLoader : IDisposable
    {
        private TestAssemblyLoadContext testAssemblyLoadContext;
        private bool disposedValue;

        public Assembly LoadAssembly(string assemblyPath)
        {
            testAssemblyLoadContext = new TestAssemblyLoadContext(assemblyPath);
            return testAssemblyLoadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(assemblyPath)));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && testAssemblyLoadContext is not null && testAssemblyLoadContext.IsCollectible)
                {
                    testAssemblyLoadContext.Unload();
                    testAssemblyLoadContext = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
