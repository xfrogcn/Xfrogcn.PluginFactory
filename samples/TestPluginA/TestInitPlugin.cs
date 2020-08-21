using PluginFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPluginA
{
    [Plugin(Alias = "TestInitPlugin")]
    public class TestInitPlugin : PluginBase, ISupportInitPlugin
    {
        public void Init(IPluginInitContext context)
        {
            
        }
    }
}
