using System;
using Microsoft.Extensions.DependencyInjection;

namespace PluginFactory.Abstractions
{
    public interface IPluginInitContext
    {
        string PluginPath { get; }

        IPluginLoader PluginLoader { get; }

        IServiceCollection ServiceCollection { get; }
    }
}
