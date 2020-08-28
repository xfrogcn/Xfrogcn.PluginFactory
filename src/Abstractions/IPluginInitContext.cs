using Microsoft.Extensions.DependencyInjection;
using System;

namespace PluginFactory
{
    public interface IPluginInitContext
    {
        string PluginPath { get; }

        IPluginLoader PluginLoader { get; }

        IServiceCollection ServiceCollection { get; }

        IServiceProvider InitServiceProvider { get; }
    }
}
