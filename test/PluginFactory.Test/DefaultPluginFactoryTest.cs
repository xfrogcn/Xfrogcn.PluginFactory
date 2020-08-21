using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PluginFactory.Test
{
    [Trait("Group", "DefaultPluginFactory")]
    public class DefaultPluginFactoryTest
    {
        [Fact(DisplayName = "No_Config")]
        public void Test1()
        {
            // 默认使用Plugins目录作为插件目录
            IServiceCollection serviceDescriptors = new ServiceCollection()
                .AddPluginFactory();

            var sp = serviceDescriptors.BuildServiceProvider();
            var factory = sp.GetRequiredService<IPluginFactory>();
            var loader = sp.GetRequiredService<IPluginLoader>();
            Assert.Equal(4, loader.PluginList.Count);
        }

        [Fact(DisplayName = "Get_Plugin_Instance")]
        public void Test2()
        {
            // 默认使用Plugins目录作为插件目录
            IServiceCollection serviceDescriptors = new ServiceCollection()
                .AddPluginFactory(options =>
                {
                    // 不载入Plugins目录下文件
                    options.Predicate = _ => false;
                    // 加入测试程序集
                    options.AddAssembly(typeof(DefaultPluginFactoryTest).Assembly);
                });

            var sp = serviceDescriptors.BuildServiceProvider();
            var factory = sp.GetRequiredService<IPluginFactory>() as DefaultPluginFactory;
            var loader = sp.GetRequiredService<IPluginLoader>();
            Assert.Equal(4, loader.PluginList.Count);

            // TestPluginA 为私有
            var pluginA = factory.GetPlugin<TestPluginA>();
            Assert.Null(pluginA);

            var pluginB = factory.GetPlugin<TestPluginB>();
            Assert.NotNull(pluginB);

            var pluginC = factory.GetPlugin<TestPluginC>();
            Assert.NotNull(pluginC);

            var pluginD = factory.GetPlugin<TestPluginD>();
            Assert.NotNull(pluginD);

            var pluginE = factory.GetPlugin<TestPluginE>();
            Assert.NotNull(pluginE);

        }

        [Fact(DisplayName = "Start_Stop")]
        public async Task Test3()
        {
            // 默认使用Plugins目录作为插件目录
            IServiceCollection serviceDescriptors = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddDebug();
                    loggingBuilder.AddConsole();
                })
                .AddPluginFactory(options =>
                {
                    // 不载入Plugins目录下文件
                    options.Predicate = _ => false;
                    // 加入测试程序集
                    options.AddAssembly(typeof(DefaultPluginFactoryTest).Assembly);
                });

            var sp = serviceDescriptors.BuildServiceProvider();
            var factory = sp.GetRequiredService<IPluginFactory>() as DefaultPluginFactory;
            var loader = sp.GetRequiredService<IPluginLoader>();
            Assert.Equal(4, loader.PluginList.Count);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await factory.StartAsync(cancellationTokenSource.Token);

            await factory.StopAsync(cancellationTokenSource.Token);
        }
    }
}
