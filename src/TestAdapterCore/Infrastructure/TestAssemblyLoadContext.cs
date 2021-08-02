using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace TestAdapterCore.Infrastructure
{
    public class TestAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        public TestAssemblyLoadContext(string pluginPath) : base(pluginPath, true)
        {
            resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var loadedAssembly = Default.Assemblies.FirstOrDefault(assembly => assembly.GetName().Name == assemblyName.Name);
            if (loadedAssembly is not null)
            {
                return loadedAssembly;
            }

            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
