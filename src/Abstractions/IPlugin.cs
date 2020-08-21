using System.Threading.Tasks;

namespace PluginFactory
{
    public interface IPlugin
    {
        Task StartAsync(IPluginContext context);

        Task StopAsync(IPluginContext context);
    }
}
