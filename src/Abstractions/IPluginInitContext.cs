using Microsoft.Extensions.DependencyInjection;

namespace PluginFactory
{
    public interface IPluginInitContext
    {
        string PluginPath { get; }

        IPluginLoader PluginLoader { get; }

        IServiceCollection ServiceCollection { get; }
    }
}
