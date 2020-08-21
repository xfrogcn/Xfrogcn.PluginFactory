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
            if (!section.Exists())
            {
                // 内嵌类型，将+号替换为.
                configKey = configKey.Replace("+", ".");
                section= configration.Configuration.GetSection(configKey);
            }
            Configuration = section;

            var attr = pluginType.GetCustomAttributes(typeof(PluginAttribute), false).OfType<PluginAttribute>().FirstOrDefault();
            if(attr !=null && !String.IsNullOrEmpty(attr.Alias))
            {
                configKey = attr.Alias;
                var section2 = configration.Configuration.GetSection(configKey);
                if (section2.Exists())
                {
                    //合并
                    var config = new ConfigurationBuilder()
                        .AddConfiguration(section)
                        .AddConfiguration(section2)
                        .Build();
                    Configuration = config;
                }

            }
           
        }

        public IConfiguration Configuration { get; }
    }
}
