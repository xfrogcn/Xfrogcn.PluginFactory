using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace PluginFactory.Test
{
    [Trait("Group", "DefaultPluginLoader")]
    public class DefaultPluginLoaderTest
    {
        [Fact(DisplayName = "Loader")]
        public void Test1()
        {
            PluginFactoryOptions options = new PluginFactoryOptions();
            IServiceCollection services = new ServiceCollection();

            options.PluginPath = AppDomain.CurrentDomain.BaseDirectory;
            options.FileProvider = new PhysicalFileProvider(options.PluginPath);

            options.AddAssembly(Assembly.GetExecutingAssembly());

            IPluginLoader loader = new DefaultPluginLoader(options, services);
            loader.Load();

            Assert.Equal(4, loader.PluginList.Count);
            var pi = loader.PluginList.First(p => p.PluginType == typeof(TestPluginB));
            Assert.True(pi.IsEnable);
            Assert.False(pi.CanInit);
            Assert.False(pi.CanConfig);

            pi = loader.PluginList.First(p => p.PluginType == typeof(TestPluginC));
            Assert.True(pi.IsEnable);
            Assert.True(pi.CanInit);
            Assert.False(pi.CanConfig);

            pi = loader.PluginList.First(p => p.PluginType == typeof(TestPluginD));
            Assert.True(pi.IsEnable);
            Assert.False(pi.CanInit);
            Assert.True(pi.CanConfig);
            Assert.Equal(typeof(TestPluginDOptions), pi.ConfigType);

            pi = loader.PluginList.First(p => p.PluginType == typeof(TestPluginE));
            Assert.True(pi.IsEnable);
            Assert.True(pi.CanInit);
            Assert.True(pi.CanConfig);
            Assert.Equal(typeof(TestPluginDOptions), pi.ConfigType);

        }
    }
}
