using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

[assembly: InternalsVisibleTo("PluginFactory.Abstractions.Test")]

namespace PluginFactory
{
    /// <summary>
    /// 以隔离的方式加载程序集，每个加载的程序集可以有独立的依赖版本
    /// 隔离的插件需位于单独的文件夹中，且文件夹名称需与程序集名称一致，且必须具有deps.json文件
    /// </summary>
    public class IsolationAssemblyLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;
        private readonly string _assemblyName;

        public IsolationAssemblyLoadContext(string assemblyPath)
        {
            if(String.IsNullOrEmpty(assemblyPath))
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }
            _assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            
            _resolver = new AssemblyDependencyResolver(assemblyPath);
        }

        public Assembly Load()
        {
            return Load(new AssemblyName(_assemblyName));
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }

    }
}
