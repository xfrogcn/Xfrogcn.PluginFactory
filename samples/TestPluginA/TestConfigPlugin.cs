using Microsoft.Extensions.Options;
using Xfrogcn.PluginFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPluginA
{
    public class TestConfigPluginOptions : ShareOptions
    {

    }
    [Plugin(Alias = "TestConfigPlugin")]
    public class TestConfigPlugin : SupportConfigPluginBase<TestConfigPluginOptions>
    {
        public TestConfigPlugin(IOptionsMonitor<TestConfigPluginOptions> options) : base(options)
        {
        }
    }
}
