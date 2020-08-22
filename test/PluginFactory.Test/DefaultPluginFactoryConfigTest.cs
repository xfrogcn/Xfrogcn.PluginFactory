using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace PluginFactory.Test
{
    [Trait("Group", "DefaultPluginFactory")]
    public class DefaultPluginFactoryConfigTest
    {
        [Fact(DisplayName = "Normal_Config")]
        public void Test1()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:Path", "Test/Plugins" }
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();


            IServiceCollection sc = new ServiceCollection()
                .AddPluginFactory(config);

            var sp = sc.BuildServiceProvider();
            var options = sp.GetRequiredService<PluginFactoryOptions>();
            IPluginLoader loader = sp.GetRequiredService<IPluginLoader>();

            Assert.EndsWith("Test/Plugins", options.PluginPath);
            Assert.Empty(loader.PluginList);

        }


        [Fact(DisplayName = "Plugin_IsEnable")]
        public void Test2()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:Path", "Test/Plugins" },
                { "Plugins:PluginFactory.Test.TestPluginB:IsEnabled", "0" }
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();


            IServiceCollection sc = new ServiceCollection()
                .AddPluginFactory(config, typeof(DefaultPluginFactoryConfigTest).Assembly);

            var sp = sc.BuildServiceProvider();
            var options = sp.GetRequiredService<PluginFactoryOptions>();
            IPluginLoader loader = sp.GetRequiredService<IPluginLoader>();
            DefaultPluginFactory factory = sp.GetRequiredService<IPluginFactory>() as DefaultPluginFactory;

            Assert.Equal(4, loader.PluginList.Count);
            PluginInfo pi = loader.PluginList.FirstOrDefault(p => p.PluginType == typeof(TestPluginB));
            Assert.False(pi.IsEnable);

            var p = factory.GetPlugin<TestPluginB>();
            Assert.Null(p);

            var p2 = factory.GetPlugin<TestPluginC>();
            Assert.NotNull(p2);
        }
    }



}
