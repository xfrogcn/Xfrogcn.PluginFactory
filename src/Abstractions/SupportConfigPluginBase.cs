using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xfrogcn.PluginFactory
{
    /// <summary>
    /// 支持配置的插件基类
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class SupportConfigPluginBase<TOptions> : PluginBase, ISupportConfigPlugin<TOptions>
        where TOptions : class, new()
    {
        protected  TOptions Options { get; private set; }
        public SupportConfigPluginBase(IOptionsMonitor<TOptions> options)
        {
            if (options != null)
            {
                Options = options.CurrentValue;
                options.OnChange(OnOptionsChanged);
            }
        }

        protected virtual void OnOptionsChanged(TOptions options)
        {
            Options = options;
        }
    }
}
