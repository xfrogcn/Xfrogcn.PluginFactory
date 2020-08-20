using System;
using Microsoft.Extensions.DependencyInjection;
using PluginFactory.Abstractions;

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
