using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PluginFactory
{
    public class DefaultPluginFactory : IPluginFactory
    {
        private IPluginLoader _loader;

        private readonly ILogger _logger;

        private List<IPlugin> _pluginList = new List<IPlugin>();

        public DefaultPluginFactory(
            IPluginLoader loader,
            ILoggerFactory loggerFactory,
            IEnumerable<IPlugin> plugins)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }
            if( loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (plugins != null)
            {
                List<PluginInfo> disabledList = loader.PluginList.Where(x => !x.IsEnable).ToList();
                _pluginList.AddRange(plugins);
                // 禁用插件列表
            }
            

            _loader = loader;
            _logger = loggerFactory.CreateLogger("PluginFactory");
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
