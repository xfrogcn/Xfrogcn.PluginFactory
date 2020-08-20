using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PluginFactory.Abstractions;

namespace PluginFactory
{
    public class DefaultPluginFactory : IPluginFactory
    {
        private IPluginLoader _loader;
        public DefaultPluginFactory(IPluginLoader loader)
        {
            _loader = loader;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
