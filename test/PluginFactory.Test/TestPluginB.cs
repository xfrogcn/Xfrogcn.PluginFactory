using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginFactory.Test
{
    public class TestPluginB : IPlugin
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
}
