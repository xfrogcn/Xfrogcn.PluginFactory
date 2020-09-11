using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xfrogcn.PluginFactory.Test
{
    class TestPluginA : IPlugin
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
