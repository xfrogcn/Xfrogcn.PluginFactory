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
    }
}
