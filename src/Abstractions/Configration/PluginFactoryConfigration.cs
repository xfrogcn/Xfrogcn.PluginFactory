﻿using Microsoft.Extensions.Configuration;

namespace PluginFactory.Abstractions
{
    /// <summary>
    /// IConfiguration的封装
    /// </summary>
    public class PluginFactoryConfigration
    {
        public PluginFactoryConfigration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
    }
}