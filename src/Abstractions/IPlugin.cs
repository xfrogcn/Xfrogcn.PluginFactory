using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginFactory.Abstractions
{
    public interface IPlugin
    {
        Task StartAsync(IPluginContext context);

        Task StopAsync(IPluginContext context);
    }
}
