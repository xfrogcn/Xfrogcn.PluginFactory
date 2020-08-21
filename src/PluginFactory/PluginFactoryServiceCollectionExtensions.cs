using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using PluginFactory;
using System;
using System.IO;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PluginFactoryServiceCollectionExtensions
    {
        public static readonly string DEFAULT_PLUGIN_PATH = "Plugins";


        public static IServiceCollection AddPluginFactory(this IServiceCollection services)
        {
            services.AddOptions();

            var options = createDefaultOptions();

            services.AddPluginFactory(options);

            return services;
        }

        public static IServiceCollection AddPluginFactory(this IServiceCollection services, IConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //注入配置
            PluginFactoryConfigration factoryConfigration = new PluginFactoryConfigration(configuration);
            services.TryAddSingleton(factoryConfigration);

            // 从配置中获取设置
            PluginFactoryOptions options = createDefaultOptions();
            options.ConfigFromConfigration(configuration);

            services.AddPluginFactory(options);

            

            return services;
        }


        public static IServiceCollection AddPluginFactory(this IServiceCollection services, Action<PluginFactoryOptions> configureOptions)
        {
            PluginFactoryOptions options = createDefaultOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

            services.AddPluginFactory(options);

            return services;
        }


        public static IServiceCollection AddPluginFactory(this IServiceCollection services, PluginFactoryOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.AddLogging();
            services.AddOptions();

            IPluginLoader loader = createPluginLoader(services, options);
            // 载入器单例
            services.TryAddSingleton(loader);
            services.TryAddSingleton(options);

            // 从配置中获取插件设置，以插件类型名称或插件别名作为配置键
            services.TryAddSingleton(typeof(IPluginConfigrationProvider<>), typeof(PluginConfigrationProvider<>));

            // 注册配置变更监听
            services.TryAddSingleton(typeof(IOptionsChangeTokenSource<>), typeof(ConfigurationChangeTokenSource<>));

            // 注入插件工厂
            services.TryAddSingleton<IPluginFactory, DefaultPluginFactory>();
            // 兼容托管服务，在宿主环境中自动调用开始和停止方法
            services.AddHostedService((sp) =>
            {
                IPluginFactory factory = sp.GetRequiredService<IPluginFactory>();
                return factory;
            });

            

            return services;
        }

        private static PluginFactoryOptions createDefaultOptions()
        {
            string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_PLUGIN_PATH);

            // 默认设置
            PluginFactoryOptions options = new PluginFactoryOptions()
            {
                PluginPath = pluginPath,
                FileProvider = new PhysicalFileProvider(pluginPath)
            };
            return options;
        }


        private static IPluginLoader createPluginLoader(IServiceCollection services, PluginFactoryOptions options)
        {
            IPluginLoader loader = new DefaultPluginLoader(options, services);
            loader.Load();
            loader.Init();

            foreach(PluginInfo pi in loader.PluginList)
            {
                if (!pi.IsEnable)
                {
                    continue;
                }
                services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IPlugin), pi.PluginType));
            }



            // 注入配置映射
            var list = loader.PluginList.Where(x => x.CanConfig && x.IsEnable).ToList();

            foreach(PluginInfo pi in list)
            {
                Type cfgOptionsType = typeof(IConfigureOptions<>).MakeGenericType(pi.ConfigType);
                Type impleType = typeof(PluginConfigrationOptions<,>).MakeGenericType(pi.PluginType, pi.ConfigType);
                services.TryAddEnumerable(ServiceDescriptor.Singleton(cfgOptionsType, impleType));
            }

            return loader;
        }
    }
}
