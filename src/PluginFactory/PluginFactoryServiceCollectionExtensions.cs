using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using PluginFactory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PluginFactoryServiceCollectionExtensions
    {
        public static readonly string DEFAULT_PLUGIN_PATH = "Plugins";


        public static IServiceCollection AddPluginFactory(this IServiceCollection services)
        {
            services.AddOptions();

            string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_PLUGIN_PATH);

            // 默认设置
            services.Configure<PluginFactoryOptions>(options =>
            {
                options.PluginPath = pluginPath;
                options.FileProvider = new PhysicalFileProvider(pluginPath);
            });

            return services;
        }

        public static IServiceCollection AddPluginFactory(this IServiceCollection services, IConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddPluginFactory();
            //注入配置
            PluginFactoryConfigration factoryConfigration = new PluginFactoryConfigration(configuration);
            services.AddSingleton<PluginFactoryConfigration>(factoryConfigration);
            // 从配置中获取设置
            services.Configure<PluginFactoryOptions>(options =>
            {
                options.ConfigFromConfigration(configuration);
            });



            return services;
        }


        public static IServiceCollection AddPluginFactory(this IServiceCollection services, Action<PluginFactoryOptions> configureOptions)
        {
            services.AddPluginFactory();
            services.Configure<PluginFactoryOptions>(configureOptions);
            return services;
        }
    }
}
