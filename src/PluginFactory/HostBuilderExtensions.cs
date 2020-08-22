using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using PluginFactory;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder)
        {
            return UsePluginFactory(hostBuilder, (IConfiguration)null);
        }

        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            return UsePluginFactory(hostBuilder, configuration, (Assembly)null);
        }

        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, IConfiguration configuration, Assembly assembly)
        {
            return UsePluginFactory(hostBuilder, configuration, new Assembly[] { assembly });
        }



        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, IConfiguration configuration, IEnumerable< Assembly> assemblies)
        {
            hostBuilder.ConfigureServices((context, sc) =>
            {
                configuration = configuration ?? context.Configuration;
                sc.AddPluginFactory(configuration, assemblies);
            });
            return hostBuilder;
        }

        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, Assembly assembly)
        {
            return UsePluginFactory(hostBuilder, new Assembly[] { assembly });
        }

        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, IEnumerable<Assembly> assemblies)
        {
            return UsePluginFactory(hostBuilder, null, assemblies);
        }

        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, Action<PluginFactoryOptions> options)
        {
            return UsePluginFactory(hostBuilder, null, options);
        }

        public static IHostBuilder UsePluginFactory(this IHostBuilder hostBuilder, IConfiguration configuration, Action<PluginFactoryOptions> configureOptions)
        {
            hostBuilder.ConfigureServices((context, sc) =>
            {
                configuration = configuration ?? context.Configuration;
                sc.AddPluginFactory(configuration, configureOptions);
            });
            return hostBuilder;
        }

    }
}
