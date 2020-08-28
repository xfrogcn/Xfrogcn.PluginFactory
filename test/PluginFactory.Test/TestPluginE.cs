using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace PluginFactory.Test
{
    public class TestPluginEOptions
    {
        public string ConfigA { get; set; }

        public string ConfigB { get; set; }
    }

    /// <summary>
    ///  模拟插件注入服务
    /// </summary>
    public class TestPluginEService
    {
        public TestPluginEOptions Options { get; set; }
    }

    public class TestPluginE : SupportConfigPluginBase<TestPluginEOptions>, ISupportInitPlugin
    {

        private readonly TestPluginEService _service = null;
        public TestPluginEService Service => _service;
        public bool IsStarted { get; private set; }

        private  TestPluginEOptions _options;

        /// <summary>
        /// 初始化使用的构造函数
        /// </summary>
        /// <param name="options"></param>
        public TestPluginE(IOptionsMonitor<TestPluginEOptions> options) : base(options)
        {
            _options = options.CurrentValue;
        }

        public TestPluginE(IOptionsMonitor<TestPluginEOptions> options, TestPluginEService service) : base(options)
        {
            _service = service;
            _service.Options = options.CurrentValue;
        }

        protected override void OnOptionsChanged(TestPluginEOptions options)
        {
            if (_service != null)
            {
                _service.Options = options;
            }
            _options = options;
            base.OnOptionsChanged(options);
        }

        public void Init(IPluginInitContext context)
        {
            if (_options == null)
            {
                throw new Exception("注入配置失败");
            }
            context.ServiceCollection.AddSingleton<TestPluginEService>();
        }

        public override Task StartAsync(IPluginContext context)
        {
            IsStarted = true;
            return base.StartAsync(context);
        }

        public override Task StopAsync(IPluginContext context)
        {
            IsStarted = false;
            return base.StopAsync(context);
        }
    }
}
