using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Xunit;


namespace PluginFactory.Abstractions.Test
{
    [Collection("隔离加载")]
    [Trait("Group", "隔离加载")]
    public class IsolationLoader
    {
        
        string classANormalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugin-a-n");
        string classAIsolationErrorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugin-a-i-e");
        string classAIsolationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugin-a-i");
        string classBNormalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugin-b-n");
        string classBIsolationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugin-b-i");
        string classBIsolationErrorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugin-b-i-e");

        [Fact(DisplayName = "加载无依赖库")]
        public void Test1()
        {
            var loader = new Abstractions.IsolationAssemblyLoadContext(Path.Combine(classAIsolationPath, "ClassA/ClassA.dll"));
            var assembly =  loader.Load();
            Assert.NotNull(assembly);
            var types = assembly.GetTypes();

            // 无deps.json时将会回退通过Default方式载入
            loader = new Abstractions.IsolationAssemblyLoadContext(Path.Combine(classANormalPath, "ClassA.dll"));
            assembly = loader.Load();
            Assert.NotNull(assembly);
            types = assembly.GetTypes();
        }

        [Fact(DisplayName = "加载有依赖库")]
        public void Test2()
        {
            var loader = new Abstractions.IsolationAssemblyLoadContext(Path.Combine(classBNormalPath, "ClassB.dll"));
            var assembly = loader.Load();
            Assert.NotNull(assembly);
            var types = assembly.GetTypes();

            loader = new Abstractions.IsolationAssemblyLoadContext(Path.Combine(classBIsolationPath, "ClassB/ClassB.dll"));
            assembly = loader.Load();
            Assert.NotNull(assembly);
            types = assembly.GetTypes();

            loader = new Abstractions.IsolationAssemblyLoadContext(Path.Combine(classBIsolationErrorPath, "ClassB/ClassB.dll"));

            assembly = loader.Load();
            loader.Resolving += Loader_Resolving;
            Assert.NotNull(assembly);
            var types2 = assembly.GetTypes();
            Assert.NotEqual(types2[0].BaseType, types[0].BaseType);
    

            // 默认载入器会自动从当前目录解析程序集
            var loader2 = Assembly.LoadFile(Path.Combine(classBIsolationErrorPath, "ClassB/ClassB.dll"));
            assembly = loader.Load();
            Assert.NotNull(assembly);
            types = assembly.GetTypes();
        }

        private Assembly Loader_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            return null;
        }
    }
}
