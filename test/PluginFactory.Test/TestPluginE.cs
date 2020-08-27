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
        public TestPluginE() : base(null)
        {

        }

        private readonly TestPluginEService _service = null;
        public TestPluginEService Service => _service;
        public bool IsStarted { get; private set; }

        public TestPluginE(IOptionsMonitor<TestPluginEOptions> options, TestPluginEService service) : base(options)
        {
            _service = service;
            _service.Options = options.CurrentValue;
        }

        protected override void OnOptionsChanged(TestPluginEOptions options)
        {
            _service.Options = options;
            base.OnOptionsChanged(options);
        }

        public void Init(IPluginInitContext context)
        {
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
