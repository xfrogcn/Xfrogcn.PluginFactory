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

        public void Test1()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "PluginFactory.Abstractions.Test.PluginConfigrationProviderTest.PluginA:Test","A"  }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();
            
            

            //PluginConfigrationProvider<PluginA> provider = new PluginConfigrationProvider<PluginA>(config);
                
        }
    }
}
