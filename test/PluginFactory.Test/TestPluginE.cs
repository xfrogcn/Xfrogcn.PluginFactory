using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginFactory.Test
{
    public class TestPluginE : TestPluginD, ISupportInitPlugin
    {
        public TestPluginE() : base(null)
        {

        }
        public TestPluginE(IOptionsMonitor<TestPluginDOptions> options) : base(options)
        {
        }

        public void Init(IPluginInitContext context)
        {
            
        }
    }
}
