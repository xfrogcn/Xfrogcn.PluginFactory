# .NET Core 插件框架

pluginfactory 是 .NET Core 下基于依赖注入实现的插件框架，此框架是插件化开发与依赖注入的完美集合，同时融入了 .NET Core 中的配置机制，可以很好地与 ASP.NET Core 等框架融合。

## 使用向导

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

`UsePluginFactory`具有多个重载版本，详细请查看[配置](./doc/Configuration.md)文档  
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

`AddPluginFactory`具有多个重载版本，详细请查看[配置](./doc/Configuration.md)文档
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
            Console.WriteLine("插件B已启动");
            return base.StopAsync(context);
        }
    }
    ```

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

### 使用插件配置

### 插件化 ASP.NET Core
