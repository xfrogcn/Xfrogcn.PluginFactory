using System;
using Microsoft.Extensions.DependencyInjection;

namespace PluginFactory.Abstractions
{
    public interface IPluginInitContext
    {
        IPluginFactory PluginFactory { get; }

        IServiceCollection ServiceCollection { get; }
    }
}
