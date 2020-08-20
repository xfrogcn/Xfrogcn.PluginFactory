using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace PluginFactory
{
    public class PluginFactoryOptions
    {
        public const string DEFAULT_CONFIG_KEY = "Plugins";
        public const string DEFAULT_PLUGIN_PATH_KEY = "Path";
        public const string DEFAULT_ISENABLED_KEY = "IsEnabled";

        /// <summary>
        /// 插件路径
        /// </summary>
        public string PluginPath { get; set; }

        /// <summary>
        /// 文件提供器
        /// </summary>
        public IFileProvider FileProvider { get; set; }

        private List<string> _disabledPluginList = new List<string>();

        /// <summary>
        /// 被禁用的插件
        /// </summary>
        public IReadOnlyList<string> DisabledPluginList => _disabledPluginList;

        public void DisablePlugin(string pluginTypeName)
        {
            if( !_disabledPluginList.Any(x=>x.Equals(pluginTypeName, StringComparison.OrdinalIgnoreCase)))
            {
                _disabledPluginList.Add(pluginTypeName);
            }
        }

        /// <summary>
        /// 从配置中获取插件工厂设置
        /// </summary>
        /// <param name="configuration"></param>
        internal void ConfigFromConfigration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            IConfiguration pluginConfig = configuration.GetSection(DEFAULT_CONFIG_KEY);
            string path = pluginConfig[DEFAULT_PLUGIN_PATH_KEY];
            if(!String.IsNullOrEmpty(path) && !String.Equals(path, PluginPath, StringComparison.OrdinalIgnoreCase))
            {
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                }
                path = Path.GetFullPath(path);
                PluginPath = path;
                if( FileProvider==null || FileProvider is PhysicalFileProvider)
                {
                    FileProvider = new PhysicalFileProvider(path);
                }
            }

            configDisablePlugin(pluginConfig);

        }

        /// <summary>
        /// 配置下以插件全类型名称或别名作为键，如果下面存在IsEnabled配置且值为false，0则放入禁止列表
        /// </summary>
        /// <param name="pluginConfig">插件工厂根配置节点</param>
        private void configDisablePlugin(IConfiguration pluginConfig)
        {
            foreach(IConfigurationSection section in pluginConfig.GetChildren())
            {
                var isEnabledSection = section.GetSection(DEFAULT_ISENABLED_KEY);
                if(isEnabledSection.Exists() && isEnabledSection.Value=="0" || isEnabledSection.Value.ToLower() == "false")
                {
                    DisablePlugin(section.Key);
                }
            }
        }

    }
}
