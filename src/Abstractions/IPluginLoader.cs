using System.Collections.Generic;

namespace PluginFactory
{
    /// <summary>
    /// 插件载入器
    /// </summary>
    public interface IPluginLoader
    {

        /// <summary>
        /// 插件信息列表
        /// </summary>
        IReadOnlyList<PluginInfo> PluginList { get; }

        /// <summary>
        /// 载入插件
        /// </summary>
        /// <returns></returns>
        void Load();

        /// <summary>
        /// 初始化插件
        /// </summary>
        void Init();
    }
}
