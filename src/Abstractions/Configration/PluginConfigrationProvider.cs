using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace PluginFactory
{
    /// <summary>
    /// PluginConfigrationProvider从 <seealso cref="PluginFactoryConfigration"/> 配置中获取配置
    /// 每个插件以插件类型全名称或插件别名为键
    /// </summary>
    public class PluginConfigrationProvider<TPlugin> : IPluginConfigrationProvider<TPlugin>
        where TPlugin : IPlugin
    {
        public PluginConfigrationProvider(PluginFactoryConfigration configration)
        {
            if (configration == null)
            {
                throw new ArgumentNullException(nameof(configration));
            }

            Type pluginType = typeof(TPlugin);
            string configKey = typeof(TPlugin).FullName;
            var section = configration.Configuration.GetSection(configKey);
            var attr = pluginType.GetCustomAttributes(typeof(PluginAttribute), false).OfType<PluginAttribute>().FirstOrDefault();
            if(!section.Exists() && attr !=null && !String.IsNullOrEmpty(attr.Alias))
            {
                configKey = attr.Alias;
                section = configration.Configuration.GetSection(configKey);
            }
            Configuration = section;
        }

        public IConfiguration Configuration { get; }
    }
}
