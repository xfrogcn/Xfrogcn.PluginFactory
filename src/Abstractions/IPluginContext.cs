using System;
using System.Threading;

namespace PluginFactory.Abstractions
{
    public interface IPluginContext
    {
        public IPluginFactory PluginFactory { get; }

        public IServiceProvider ServiceProvider { get; }

        public CancellationToken CancellationToken { get; }
    }
}
