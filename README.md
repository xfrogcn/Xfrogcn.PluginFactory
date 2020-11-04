# .NET Core 插件框架

pluginfactory 是 .NET Core 下基于依赖注入实现的插件框架，此框架是插件化开发与依赖注入的完美集合，同时融入了 .NET Core 中的配置机制，可以很好地与 ASP.NET Core 等框架融合。

- [.NET Core 插件框架](#net-core-插件框架)
  - [使用向导](#使用向导)
    - [安装](#安装)
    - [在主程序中启用](#在主程序中启用)
      - [通过`IHostBuilder`的`UsePluginFactory`方法启用插件库](#通过ihostbuilder的usepluginfactory方法启用插件库)
      - [通过`IServiceCollection`的`AddPluginFactory`方法启用插件库](#通过iservicecollection的addpluginfactory方法启用插件库)
    - [编写插件](#编写插件)
    - [插件启动](#插件启动)
    - [编写支持初始化的插件](#编写支持初始化的插件)
    - [使用插件配置](#使用插件配置)
    - [插件化 ASP.NET Core](#插件化-aspnet-core)


## 使用向导

示例项目可参考：`Xfrogcn.PluginFactory.Example` [Gitee地址](https://gitee.com/WuYeCai/Xfrogcn.PluginFactory.Example) [Github地址](https://github.com/xfrogcn/Xfrogcn.PluginFactory.Example)

### 安装

在主程序项目中添加`Xfrogcn.PluginFactory`包

    ```dotnet
    dotnet add package Xfrogcn.PluginFactory --version 1.0.0
    ```

在插件项目中添加`Xfrogcn.PluginFactory.Abstractions`包

    ```dotnet
    dotnet add package Xfrogcn.PluginFactory.Abstractions --version 1.0.0
    ```

### 在主程序中启用

可通过以下两种方式来启用插件库，一是通过在`Host`层级的Use机制以及在依赖注入`IServiceCollection`层级的Add机制，以下分别说明：

#### 通过`IHostBuilder`的`UsePluginFactory`方法启用插件库

    ```c#
    var builder = Host.CreateDefaultBuilder(args);
    builder.UsePluginFactory();
    ```

`UsePluginFactory`具有多个重载版本，详细请查看[API](./doc/api.md)文档  
默认配置下，将使用程序运行目录下的`Plugins`目录作为插件程序集目录, 使用宿主配置文件作为插件配置文件（通常为appsettings.json）  
你也可以通过使用带有`Assembly`或`IEnumerable<Assembly>`参数的版本直接传入插件所在的程序集

#### 通过`IServiceCollection`的`AddPluginFactory`方法启用插件库

    ```c#
    var builder = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddPluginFactory();
        });
    ```

`AddPluginFactory`具有多个重载版本，详细请查看[API](./doc/api.md)文档
默认配置下，将使用程序运行目录下的`Plugins`目录作为插件程序集目录

`注意：` AddPluginFactory方法`不会`使用默认的配置文件作为插件配置，你需要显式地传入`IConfiguration`, 如果是在 ASP.NET Core 环境中，你可以在Startup类中直接获取到

### 编写插件

插件是实现了IPlugin接口的类，在插件库中也提供了PluginBase基类，一般从此类继承即可。标准插件具有启动和停止方法，通过`IPluginFactory`进行控制。

要编写插件，一般遵循以下步骤：

1. 创建插件项目（.NET Core 类库），如TestPluginA
1. 添加`Xfrogcn.PluginFactory.Abstractions`包

        ```nuget
        dotnet add package Xfrogcn.PluginFactory.Abstractions
        ```

1. 创建插件类，如Plugin，从PluginBase继承

        ```c#
        public class Plugin : PluginBase
        {
            public override Task StartAsync(IPluginContext context)
            {
                Console.WriteLine("插件A已启动");
                return base.StartAsync(context);
            }

            public override Task StopAsync(IPluginContext context)
            {
                Console.WriteLine("插件A已停止");
                return base.StopAsync(context);
            }
        }
        ```

    *启动或停止方法中可通过context中的ServiceProvider获取注入服务*

1. 通过`PluginAttribute`特性设置插件的元数据

        ```c#
        [Plugin(Alias = "PluginA", Description = "测试插件")]
        public class Plugin : PluginBase
        {
        }
        ```

    *插件元数据以及插件载入的插件列表信息可以通过`IPluginLoader.PluginList`获取*

### 插件启动

`IPluginFactory`本身实现了.NET Core扩展库的`IHostedService`机制，故如果你是在宿主环境下使用，如（ASP.NET Core）,插件的启动及停止将自动跟随宿主进行  
如果未使用宿主，可通过获取`IPluginFactory`实例调用相应方法来完成

    ```c#
    // 手动启动
    var pluginFactory = provider.GetRequiredService<IPluginFactory>();
    await pluginFactory.StartAsync(default);
    await pluginFactory.StopAsync(default);
    ```

### 编写支持初始化的插件

在很多场景，我们需要在插件中控制宿主的依赖注入，如注入新的服务等，这时候我们可通过实现支持初始化的插件（`ISupportInitPlugin`）来实现，该接口的`Init`方法将在依赖注入构建之前调用，通过方法参数`IPluginInitContext`中的`ServiceCollection`可以控制宿主注入容器。

    ```c#
    [Plugin(Alias = "PluginA", Description = "测试插件")]
    public class Plugin : PluginBase, ISupportInitPlugin
    {
        public void Init(IPluginInitContext context)
        {
            // 注入服务
            //context.ServiceCollection.TryAddScoped<ICustomerService>();
        }
    }
    ```

### 使用插件配置

插件支持 .NET Core 扩展库中的Options及Configuration机制，你只需要从`SupportConfigPluginBase<TOptions>`类继承实现插件即可，其中TOptions泛型为插件的配置类型。插件配置自动从宿主配置或启用插件工厂时传入的配置中获取，插件配置位于配置下的Plugins节点，该节点下以插件类名称或插件别名（通过`PluginAttribute`特性指定）作为键名，此键之下为插件的配置，如以下配置文件：

    ```appsettings.json
    {
        "Plugins": {
            "PluginA": {
                "TestConfig": "Hello World"
            },

        }
    }
    ```

扩展PluginA实现配置：

1. 定义配置类，如PluginOptions

        ```c#
        public class PluginOptions
        {
            public string TestConfig { get; set; }
        }
        ```

2. 实现插件

        ```c#
        [Plugin(Alias = "PluginA", Description = "测试插件")]
        public class Plugin : SupportConfigPluginBase<PluginOptions>, ISupportInitPlugin
        {

            public Plugin(IOptionsMonitor<PluginOptions> options) : base(options)
            {
            }

            public void Init(IPluginInitContext context)
            {
                // 注入服务
                //context.ServiceCollection.TryAddScoped<ICustomerService>();
                Console.WriteLine($"Init 插件配置：{Options.TestConfig}");
            }

            public override Task StartAsync(IPluginContext context)
            {
                Console.WriteLine("插件A已启动");
                Console.WriteLine($"StartAsync 插件配置：{Options.TestConfig}");
                return base.StartAsync(context);
            }

            public override Task StopAsync(IPluginContext context)
            {
                Console.WriteLine("插件A已停止");
                return base.StopAsync(context);
            }
        ```

    *注意：在插件初始化方法中也可使用注入的配置*
3. 跨插件配置

有些配置可能需要在多个插件中共享，此时你可通过`Plugins`下的`_Share`节点进行配置，此节点下配置将会被合并到插件配置中，可通过PluginOptions进行访问。

    ```appsettings.json
    {
        "Plugins": {
            "PluginA": {
            },
            "_Share": {
                "TestConfig": "Hello World"
            }
        }
    }
    ```

### 插件化 ASP.NET Core

要让 ASP.NET Core 获取得到插件中的控制器，你只需要在插件的初始化方法`Init`中，向MVC注入插件程序集：

    ```c#
    context.ServiceCollection.AddMvcCore()
        .AddApplicationPart(typeof(Plugin).Assembly);
    ```
