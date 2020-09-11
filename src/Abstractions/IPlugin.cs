using System.Threading.Tasks;

namespace Xfrogcn.PluginFactory
{
    public interface IPlugin
    {
        Task StartAsync(IPluginContext context);

        Task StopAsync(IPluginContext context);
    }
}
