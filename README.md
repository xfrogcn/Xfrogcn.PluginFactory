# .NET Core 插件框架

pluginfactory 是 .NET Core 下基于依赖注入实现的插件框架，此框架是插件化开发与依赖注入的完美集合，同时融入了 .NET Core 中的配置机制，可以很好地与 ASP.NET Core 等框架融合。

## 使用向导

### 安装

添加包引用，在主程序项目中添加`Xfrogcn.PluginFactory`包

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

`UsePluginFactory`具有多个重载版本，详细请查看文档[配置](./doc/Configuration.md)  
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

`AddPluginFactory`具有多个重载版本，详细请查看文档[配置](./doc/Configuration.md)  
默认配置下，将使用程序运行目录下的`Plugins`目录作为插件程序集目录

`注意：` AddPluginFactory方法`不会`使用默认的配置文件作为插件配置，你需要显式地传入`IConfiguration`, 在 ASP.NET Core 环境中，Startup类中可直接获取到

### 编写插件

### 插件启动

### 使用自定义配置

### 插件化 ASP.NET Core
