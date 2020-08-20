using System;
using System.Collections.Generic;
using System.Text;

namespace PluginFactory.Abstractions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute
    {
        /// <summary>
        /// 插件Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 插件别名，在插件配置中可使用别名作为插件配置键
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; }


    }
}
