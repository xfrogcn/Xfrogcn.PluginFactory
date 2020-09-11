using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xfrogcn.PluginFactory.Test
{
    public class TestPluginDOptions
    {

    }

    public class TestPluginD : SupportConfigPluginBase<TestPluginDOptions>
    {
        public TestPluginD(IOptionsMonitor<TestPluginDOptions> options) : base(options)
        {
        }
    }
}
