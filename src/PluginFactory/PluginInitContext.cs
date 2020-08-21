using Microsoft.Extensions.DependencyInjection;

namespace PluginFactory
{
    public class PluginInitContext : IPluginInitContext
    {

        public PluginInitContext(string pluginPath, IPluginLoader pluginLoader, IServiceCollection services)
        {
            PluginLoader = pluginLoader;
            ServiceCollection = services;
            PluginPath = pluginPath;
        }

        public IPluginLoader PluginLoader { get; }

        public IServiceCollection ServiceCollection { get; }

        public string PluginPath { get; }

    }
}
