using System;
using System.Collections.Generic;
using System.Text;

namespace PluginFactory.Test
{
    public class TestPluginC : TestPluginB, ISupportInitPlugin
    {

        public void Init(IPluginInitContext context)
        {
            
        }
    }
}
