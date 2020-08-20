using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginFactory.Abstractions
{
    /// <summary>
    /// 插件工厂
    /// </summary>
    public interface IPluginFactory
    {
        /// <summary>
        /// 插件信息列表
        /// </summary>
        IReadOnlyList<PluginInfo> PluginList { get; }

        /// <summary>
        /// 载入插件
        /// </summary>
        /// <param name="pluginPath"></param>
        /// <returns></returns>
        void Load(string pluginPath);

        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="context"></param>
        void Init(IPluginInitContext context);

        /// <summary>
        /// 启动所有插件
        /// </summary>
        /// <param name="context">插件上下文</param>
        /// <returns></returns>
        Task StartAsync(IPluginContext context);

        /// <summary>
        /// 停止所有插件
        /// </summary>
        /// <param name="context">插件上下文</param>
        /// <returns></returns>
        Task StopAsync(IPluginContext context);
    }
}
