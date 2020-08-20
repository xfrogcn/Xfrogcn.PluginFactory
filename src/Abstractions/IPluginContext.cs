using System;
namespace PluginFactory.Abstractions
{
    public interface IPluginContext
    {
        public IPluginFactory PluginFactory { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
