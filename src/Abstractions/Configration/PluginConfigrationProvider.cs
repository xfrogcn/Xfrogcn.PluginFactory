using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace PluginFactory
{
    /// <summary>
    /// PluginConfigrationProvider从 <seealso cref="PluginFactoryConfigration"/> 配置中获取配置
    /// 每个插件以插件类型全名称或插件别名为键
    /// </summary>
    public class PluginConfigrationProvider<TPlugin> : IPluginConfigrationProvider<TPlugin>
        where TPlugin : IPlugin
    {
        public const string DEFAULT_SHARE_KEY = "_Share";

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
           
            var attr = pluginType.GetCustomAttributes(typeof(PluginAttribute), false).OfType<PluginAttribute>().FirstOrDefault();
            IConfigurationSection aliasSection = null;
            if(attr !=null && !String.IsNullOrEmpty(attr.Alias))
            {
                configKey = attr.Alias;
                var section2 = configration.Configuration.GetSection(configKey);
                if (section2.Exists())
                {
                    aliasSection = section2;
                }

            }

            // 共享配置
            var shareSection = configration.Configuration.GetSection(DEFAULT_SHARE_KEY);


            // 合并多个配置
            IConfigurationSection[] configList = new IConfigurationSection[]
            {
                // 共享配置可以被覆盖
                shareSection, section, aliasSection
            };
            if (configList.Count(x => x != null && x.Exists()) > 1)
            {
                var cb = new ConfigurationBuilder();
                configList.All(s =>
                {
                    if(s !=null && s.Exists())
                    {
                        cb.AddConfiguration(s);
                    }
                    return true;
                });
                Configuration = cb.Build();
            }
            else
            {
                Configuration = configList.FirstOrDefault(x => x != null && x.Exists());
                if(Configuration == null)
                {
                    Configuration = section;
                }
            }

           
        }

        public IConfiguration Configuration { get; }
    }
}
