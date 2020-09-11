using Microsoft.Extensions.Configuration;
using System;

namespace Xfrogcn.PluginFactory
{
    /// <summary>
    /// IConfiguration的封装
    /// </summary>
    public class PluginFactoryConfigration
    {
        public static readonly string DEFAULT_CONFIG_KEY = "Plugins";

        public PluginFactoryConfigration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Configuration = configuration.GetSection(DEFAULT_CONFIG_KEY);
        }

        public IConfiguration Configuration { get; }
    }
}
