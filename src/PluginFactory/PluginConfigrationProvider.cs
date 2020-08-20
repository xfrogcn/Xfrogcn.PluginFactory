using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PluginFactory.Abstractions;

namespace PluginFactory
{
    /// <summary>
    /// PluginConfigrationProvider从 <seealso cref="PluginFactoryConfigration"/> 配置中获取配置
    /// 每个插件以插件类型全名称或插件别名为键
    /// </summary>
    internal class PluginConfigrationProvider<TPlugin> : IPluginConfigrationProvider<TPlugin>
    {
        private readonly IConfiguration _configuration;

        public PluginConfigrationProvider(PluginFactoryConfigration configration)
        {
            if (configration == null)
            {
                throw new ArgumentNullException(nameof(configration));
            }

            Type pluginType = typeof(TPlugin);
            string configKey = typeof(TPlugin).FullName;
            var attr = pluginType.GetCustomAttributes(typeof(PluginAttribute), false).OfType<PluginAttribute>().FirstOrDefault();
            if(attr !=null && !String.IsNullOrEmpty(attr.Alias))
            {
                configKey = attr.Alias;
            }
            _configuration = configration.Configuration.GetSection(configKey);
        }

        public IConfiguration Configuration => _configuration;
    }
}
