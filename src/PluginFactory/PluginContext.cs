using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PluginFactory
{
    class PluginContext : IPluginContext
    {
        public PluginContext(IPluginFactory pluginFactory, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            PluginFactory = pluginFactory;
            ServiceProvider = serviceProvider;
            CancellationToken = cancellationToken;
        }

        public IPluginFactory PluginFactory { get; }

        public IServiceProvider ServiceProvider { get; }

        public CancellationToken CancellationToken { get; }
    }
}
