using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Xfrogcn.PluginFactory.Test
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

            Assert.EndsWith($"Test{System.IO.Path.DirectorySeparatorChar}Plugins", options.PluginPath);
            Assert.Empty(loader.PluginList);

        }


        [Fact(DisplayName = "Plugin_IsEnable")]
        public void Test2()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:Path", "Test/Plugins" },
                { "Plugins:Xfrogcn.PluginFactory.Test.TestPluginB:IsEnabled", "0" }
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


        [Fact(DisplayName = "Plugin_Options")]
        public void Test3()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:Path", "Test/Plugins" },
                { "Plugins:Xfrogcn.PluginFactory.Test.TestPluginE:ConfigA", "A" },
                { "Plugins:_Share:ConfigB", "B" }, //共享配置
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

            TestPluginEService pluginEService = sp.GetRequiredService<TestPluginEService>();

            var p2 = factory.GetPlugin<TestPluginE>();
            Assert.Equal(pluginEService, p2.Service);
            Assert.Equal("A", pluginEService.Options.ConfigA);
            Assert.Equal("B", pluginEService.Options.ConfigB);

            // Options变更
            config["Plugins:_Share:ConfigB"] = "B2";
            config.Reload();
            Assert.Equal("B2", pluginEService.Options.ConfigB);

        }

        [Fact(DisplayName = "Host_Builder")]
        public async Task Test4()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:Path", "Test/Plugins" },
                { "Plugins:PluginFactory.Test.TestPluginE:ConfigA", "A" },
                { "Plugins:_Share:ConfigB", "B" }, //共享配置
            };

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddInMemoryCollection(dic);
                })
                .UsePluginFactory(typeof(DefaultPluginFactoryConfigTest).Assembly)
                .Build();

            await host.StartAsync();

            DefaultPluginFactory factory = host.Services.GetRequiredService<IPluginFactory>() as DefaultPluginFactory;
            var pluginE = factory.GetPlugin<TestPluginE>();
            Assert.Equal("B", pluginE.Service.Options.ConfigB);
            Assert.True(pluginE.IsStarted);
            
        }
    }



}
