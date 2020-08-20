using System;
using Microsoft.Extensions.Configuration;

namespace PluginFactory
{
    /// <summary>
    /// IConfiguration的封装
    /// </summary>
    internal class PluginFactoryConfigration
    {
        private readonly IConfiguration _configuration;

        public PluginFactoryConfigration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get => _configuration;
        }
    }
}
