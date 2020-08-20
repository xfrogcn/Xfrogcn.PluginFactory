using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace PluginFactory
{
    /// <summary>
    /// 从配置中获取插件设置
    /// </summary>
    /// <typeparam name="TPlugin">插件类型</typeparam>
    /// <typeparam name="TPluginOptions">插件设置</typeparam>
    public class PluginConfigrationOptions<TPlugin, TPluginOptions> : ConfigureFromConfigurationOptions<TPluginOptions>
        where TPluginOptions : class, new()
    {
        public PluginConfigrationOptions(IPluginConfigrationProvider<TPlugin> provider) : base(provider.Configuration)
        {
        }
    }
}
