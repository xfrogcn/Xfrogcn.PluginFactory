using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PluginFactory.Abstractions.Test
{
    [Trait("Group", "PluginConfigrationProvider")]
    public class PluginConfigrationProviderTest
    {
        class PluginA : IPlugin
        {
            public Task StartAsync(IPluginContext context)
            {
                return Task.CompletedTask;
            }

            public Task StopAsync(IPluginContext context)
            {
                return Task.CompletedTask;
            }
        }

        [Plugin(Alias = "PluginB")]
        class PluginB : PluginA
        {

        }

        [Fact(DisplayName = "Normal")]
        public void Test1()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginFactory.Abstractions.Test.PluginConfigrationProviderTest.PluginA:Test","A"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration factoryConfig = new PluginFactoryConfigration(config);
            PluginConfigrationProvider<PluginA> provider = new PluginConfigrationProvider<PluginA>(factoryConfig);
            Assert.Equal("A", provider.Configuration["Test"]);
        }

        [Fact(DisplayName = "Alias")]
        public void Test2()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginB:Test","B"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration factoryConfig = new PluginFactoryConfigration(config);
            PluginConfigrationProvider<PluginB> provider = new PluginConfigrationProvider<PluginB>(factoryConfig);
            Assert.Equal("B", provider.Configuration["Test"]);
        }

        [Fact(DisplayName = "Merge")]
        public void Test3()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginFactory.Abstractions.Test.PluginConfigrationProviderTest.PluginB:Test","B"  },
                { "Plugins:PluginB:Test2","B2"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration factoryConfig = new PluginFactoryConfigration(config);
            PluginConfigrationProvider<PluginB> provider = new PluginConfigrationProvider<PluginB>(factoryConfig);
            Assert.Equal("B", provider.Configuration["Test"]);
            Assert.Equal("B2", provider.Configuration["Test2"]);
        }

        [Fact(DisplayName = "Empty")]
        public void Test4()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration factoryConfig = new PluginFactoryConfigration(config);
            PluginConfigrationProvider<PluginB> provider = new PluginConfigrationProvider<PluginB>(factoryConfig);
            Assert.Empty(provider.Configuration.GetChildren());
           
        }




        [Fact(DisplayName = "Share_Normal")]
        public void Test5()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginB:Test","B"  },
                { "Plugins:_Share:Test2","B2"  },
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration factoryConfig = new PluginFactoryConfigration(config);
            PluginConfigrationProvider<PluginB> provider = new PluginConfigrationProvider<PluginB>(factoryConfig);
            Assert.Equal("B", provider.Configuration["Test"]);
            Assert.Equal("B2", provider.Configuration["Test2"]);
        }

        [Fact(DisplayName = "Share_Override")]
        public void Test6()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Plugins:PluginB:Test","B"  },
                { "Plugins:_Share:Test","B2"  },
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            PluginFactoryConfigration factoryConfig = new PluginFactoryConfigration(config);
            PluginConfigrationProvider<PluginB> provider = new PluginConfigrationProvider<PluginB>(factoryConfig);
            Assert.Equal("B", provider.Configuration["Test"]);
        }
    }
}
