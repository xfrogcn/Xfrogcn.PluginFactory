using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace PluginFactory
{
    public class PluginInitContext : IPluginInitContext
    {

        public PluginInitContext(string pluginPath, IPluginLoader pluginLoader, IServiceCollection services)
        {
            PluginLoader = pluginLoader;
            ServiceCollection = services;
            PluginPath = pluginPath;
            IServiceCollection tempServices = new ServiceCollection();
            foreach(var d in services)
            {
                tempServices.Add(d);
            }
            // 注入可初始化的插件列表
            var pl = pluginLoader.PluginList.Where(x => x.IsEnable && x.CanInit);
            foreach(var pi in pl)
            {
                tempServices.AddSingleton(typeof(ISupportInitPlugin), pi.PluginType);
            }
            InitServiceProvider = tempServices.BuildServiceProvider();
        }

        public IPluginLoader PluginLoader { get; }

        public IServiceCollection ServiceCollection { get; }

        public IServiceProvider InitServiceProvider { get; }

        public string PluginPath { get; }

    }
}
