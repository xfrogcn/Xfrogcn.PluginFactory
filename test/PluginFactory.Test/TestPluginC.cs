using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace PluginFactory.Test
{
    public class TestPluginC : TestPluginB, ISupportInitPlugin
    {
        public class TestDIClass
        {
            public string Message { get; set; } = "Hello";
        }


        public void Init(IPluginInitContext context)
        {
            context.ServiceCollection.AddSingleton<TestDIClass>();
        }
    }
}
