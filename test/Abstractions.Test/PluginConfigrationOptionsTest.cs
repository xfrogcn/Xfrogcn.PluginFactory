using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Xfrogcn.PluginFactory.Abstractions.Test
{
    [Trait("Group", "PluginConfigrationOptions")]
    public class PluginConfigrationOptionsTest
    {
        class PluginAOptions
        {
            public string Test { get; set; }
        }

        [Plugin(Alias = "PluginA")]
        class PluginA : IPlugin
        {
            public string Test { get; private set; }

            public PluginA(IOptionsMonitor<PluginAOptions> options)
            {
                Test = options.CurrentValue.Test;
                options.OnChange((opt, name) =>
                {
                    Test = opt.Test;
                });
            }

            public Task StartAsync(IPluginContext context)
            {
                return Task.CompletedTask;
            }

            public Task StopAsync(IPluginContext context)
            {
                return Task.CompletedTask;
            }
        }

        [Fact(DisplayName = "Normal")]
        public void Test1()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginA:Test","A"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginConfigrationOptions<PluginA, PluginAOptions> optionsAction = new PluginConfigrationOptions<PluginA, PluginAOptions>(
                new PluginConfigrationProvider<PluginA>(new PluginFactoryConfigration(config)));

            PluginAOptions options = new PluginAOptions();
            optionsAction.Configure(options);

            Assert.Equal("A", options.Test);
        }

        [Fact(DisplayName = "WithDI")]
        public void Test2()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginA:Test","A"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration rootConfig = new PluginFactoryConfigration(config);

            IServiceCollection services = new ServiceCollection()
                .AddSingleton(rootConfig)
                .AddSingleton(typeof(IPluginConfigrationProvider<>), typeof(PluginConfigrationProvider<>))
                .AddOptions()
                .ConfigureOptions<PluginConfigrationOptions<PluginA, PluginAOptions>>();

            var sp = services.BuildServiceProvider();
            var options = sp.GetRequiredService<IOptions<PluginAOptions>>();

            Assert.Equal("A", options.Value.Test);
        }


        [Fact(DisplayName = "OptionsChanged")]
        public void Test3()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginA:Test","A"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration rootConfig = new PluginFactoryConfigration(config);

            IServiceCollection services = new ServiceCollection()
                .AddSingleton(rootConfig)
                .AddSingleton(typeof(IPluginConfigrationProvider<>), typeof(PluginConfigrationProvider<>))
                .AddOptions()
                .ConfigureOptions<PluginConfigrationOptions<PluginA, PluginAOptions>>()
                .AddSingleton(typeof(IOptionsChangeTokenSource<>), typeof(ConfigurationChangeTokenSource<>))
                .AddSingleton<IPlugin, PluginA>();
            // 必须
            services.TryAddSingleton<IConfiguration>(config);

            var sp = services.BuildServiceProvider();
            var plugin = sp.GetRequiredService<IPlugin>() as PluginA;

            Assert.Equal("A", plugin.Test);

            config["Plugins:PluginA:Test"] = "B";
            config.Reload();

            Assert.Equal("B", plugin.Test);
        }
    }
}
