using Microsoft.Extensions.Options;
using PluginFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPluginA
{
    public class TestConfigPluginWithInitOptions : ShareOptions
    {

    }

    [Plugin(Alias = "TestConfigPluginWithInit")]
    public class TestConfigPluginWithInit : SupportConfigPluginBase<TestConfigPluginWithInitOptions>, ISupportInitPlugin
    {
        public TestConfigPluginWithInit() : base(null)
        {

        }
        public TestConfigPluginWithInit(IOptionsMonitor<TestConfigPluginWithInitOptions> options) : base(options)
        {
        }

        public void Init(IPluginInitContext context)
        {
            
        }
    }
}
