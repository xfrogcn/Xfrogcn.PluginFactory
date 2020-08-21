using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace PluginFactory
{
    public class DefaultPluginFactory : IPluginFactory
    {
        private IPluginLoader _loader;

        private readonly ILogger _logger;

        private readonly IServiceProvider _serviceProvider;

        private List<IPlugin> _pluginList = new List<IPlugin>();

        public DefaultPluginFactory(
            IPluginLoader loader,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
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
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ForEachPlugin(async (plugin, ctx) =>
            {
                Log._pluginBeginStart(_logger, null);
                Stopwatch sw = new Stopwatch();
                try
                {
                    await plugin.StartAsync(ctx);
                }
                catch (Exception e)
                {
                    Log._pluginStartError(_logger, e.ToString(), e);
                    throw;
                }
                sw.Stop();
                Log._pluginCompleteStart(_logger, sw.ElapsedMilliseconds, null);

            }, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await ForEachPlugin(async (plugin, ctx) =>
            {
                Log._pluginBeginStop(_logger, null);
                Stopwatch sw = new Stopwatch();
                try
                {
                    await plugin.StartAsync(ctx);
                }
                catch (Exception e)
                {
                    Log._pluginStopError(_logger, e.ToString(), e);
                    throw;
                }
                sw.Stop();
                Log._pluginCompleteStop(_logger, sw.ElapsedMilliseconds, null);

            }, cancellationToken);
        }

        protected async Task ForEachPlugin(Func<IPlugin, IPluginContext, Task> proc, CancellationToken cancellationToken)
        {
            foreach (IPlugin p in _pluginList)
            {
                PluginInfo pi = getPluginInfo(p.GetType());
                PluginInfoLogValue logValue = new PluginInfoLogValue(pi);
                using (var scope = _logger.BeginScope(logValue))
                {
                    IPluginContext context = new PluginContext(
                        this,
                        _serviceProvider,
                        cancellationToken
                        );
                    await proc(p, context);
                }
            }
        }

        private PluginInfo getPluginInfo(Type pluginType)
        {
            return _loader.PluginList.First(x => x.PluginType == pluginType);
        }

        static class Log
        {
            static EventId PluginStartingEventId = new EventId(100, "PluginStarting");
            static EventId PluginStartFinishEventId = new EventId(101, "PluginStartFinish");
            static EventId PluginStoppingEventId = new EventId(102, "PluginStopping");
            static EventId PluginStopFinishEventId = new EventId(103, "PluginStopFinish");

            static EventId PluginStartErrorEventId = new EventId(110, "PluginStartError");
            static EventId PluginStopErrorEventId = new EventId(111, "PluginStopError");

            public static Action<ILogger, Exception> _pluginBeginStart =
                LoggerMessage.Define(LogLevel.Information, PluginStartingEventId, Resources.PluginBeginStart);
            public static Action<ILogger,long, Exception> _pluginCompleteStart =
                LoggerMessage.Define<long>(LogLevel.Information, PluginStartFinishEventId, Resources.PluginCompleteStart);
            public static Action<ILogger, string, Exception> _pluginStartError =
                LoggerMessage.Define<string>(LogLevel.Error, PluginStartErrorEventId, Resources.PluginStartException);

            public static Action<ILogger, Exception> _pluginBeginStop =
                LoggerMessage.Define(LogLevel.Information, PluginStoppingEventId, Resources.PluginBeginStop);
            public static Action<ILogger, long, Exception> _pluginCompleteStop =
                LoggerMessage.Define<long>(LogLevel.Information, PluginStartFinishEventId, Resources.PluginCompleteStop);
            public static Action<ILogger, string, Exception> _pluginStopError =
                LoggerMessage.Define<string>(LogLevel.Error, PluginStopErrorEventId, Resources.PluginStopException);
        }
    }
}
