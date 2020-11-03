# API

## IHostBuilder.UsePluginFactory 扩展方法

### UsePluginFactory()

- 说明：无参数，使用默认配置来启用插件工厂，此时会使用IHostBuilder的配置作为插件配置容器，使用应用目录的Plugins目录作为插件目录

### UsePluginFactory(IConfiguration configuration)

- 说明：使用指定的配置来初始化插件工厂
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置

### UsePluginFactory(IConfiguration configuration, Assembly assembly)

- 说明：使用指定的配置来初始化插件工厂, 并自动加载assembly参数指定的程序集
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置
  - *assembly*：Assembly，要载入的插件程序集

### UsePluginFactory(IConfiguration configuration, IEnumerable&lt;Assembly&gt; assemblies)

- 说明：使用指定的配置来初始化插件工厂, 并自动加载assemblies参数指定的程序集列表
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置
  - *assemblies*：List&lt;Assembly&gt;，要载入的插件程序集列表

### UsePluginFactory(Assembly assembly)

- 说明：使用默认配置初始化插件工厂, 并自动加载assembly参数指定的程序集
- 参数列表：
  - *assembly*：Assembly，要载入的插件程序集

### UsePluginFactory(IEnumerable&lt;Assembly&gt; assemblies)

- 说明：使用默认配置初始化插件工厂, 并自动加载assemblies参数指定的程序集列表
- 参数列表：
  - *assemblies*：List&lt;Assembly&gt;，要载入的插件程序集列表

### UsePluginFactory(Action&lt;PluginFactoryOptions&gt; options)

- 说明：使用指定的设置来初始化插件工厂
- 参数列表：
  - *options*：Action&lt;PluginFactoryOptions&gt;，设置委托，可设置项参考[PluginFactoryOptions](#PluginFactoryOptions)

### UsePluginFactory(IConfiguration configuration, Action&lt;PluginFactoryOptions&gt; options)

- 说明：指定插件配置容器，并使用指定的设置来初始化插件工厂
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置
  - *options*：Action&lt;PluginFactoryOptions&gt;，设置委托，可设置项参考[PluginFactoryOptions](#PluginFactoryOptions)

## IServiceCollection.AddPluginFactory 扩展方法

### AddPluginFactory()

- 说明：使用默认设置初始化插件工厂，此时内部会使用内存配置容器作为插件配置，故`不会`从配置文件或其他配置源载入配置；插件目录将使用Plugins目录

### AddPluginFactory(IConfiguration configuration)

- 说明：使用指定的配置来初始化插件工厂
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置

### AddPluginFactory(IConfiguration configuration, Assembly assembly)

- 说明：使用指定的配置来初始化插件工厂, 并自动加载assembly参数指定的程序集
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置
  - *assembly*：Assembly，要载入的插件程序集

### AddPluginFactory(IConfiguration configuration, IEnumerable&lt;Assembly&gt; assemblies)

- 说明：使用指定的配置来初始化插件工厂, 并自动加载assemblies参数指定的程序集列表
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置
  - *assemblies*：List&lt;Assembly&gt;，要载入的插件程序集列表

### AddPluginFactory(Action&lt;PluginFactoryOptions&gt; configureOptions)

- 说明：使用指定的设置来初始化插件工厂
- 参数列表：
  - *options*：Action&lt;PluginFactoryOptions&gt;，设置委托，可设置项参考[PluginFactoryOptions](#PluginFactoryOptions)

### AddPluginFactory(IConfiguration configuration, Action&lt;PluginFactoryOptions&gt; configureOptions)

- 说明：指定插件配置容器，并使用指定的设置来初始化插件工厂
- 参数列表：
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置
  - *options*：Action&lt;PluginFactoryOptions&gt;，设置委托，可设置项参考[PluginFactoryOptions](#PluginFactoryOptions)

### AddPluginFactory(PluginFactoryOptions options, IConfiguration configuration)

- 说明：指定设置对象，并使用指定配置对象来初始化插件工厂
- 参数列表：
  - *options*：PluginFactoryOptions，设置对象，可设置项参考[PluginFactoryOptions](#PluginFactoryOptions)
  - *configuration*： IConfiguration，插件所使用的配置对象，插件将从此配置的Plugins节点获取插件自身的配置

## IPluginLoader

`IPluginLoader`主要用于从指定位置载入插件

### PluginList属性

- 说明：只读属性，返回已加载的插件列表，类型为IReadOnlyList&lg;PluginInfo&gt;，参见[PluginInfo](#PluginInfo)

### Load()

- 说明：加载插件程序集，*注意*：通常情况下你无需调用此方法，框架内部会自动调用，如果你要实现自己的加载机制，则可实现此接口
- 返回：void

### Init()

- 说明：初始化插件，*注意*：通常情况下你无需调用此方法，框架内部会自动调用，如果你要实现自己的加载机制，则可实现此接口
- 返回：void

## IPluginFactory

`IPluginFactory`插件工厂接口，用于控制插件的生命周期，该接口从`IHostedService`继承，故支持宿主的托管服务机制

### StartAsync(CancellationToken cancellationToken)

- 说明：启动所有插件
- 参数：
  - cancellationToken： CancellationToken，取消监听令牌
- 返回：Task

### StopAsync(CancellationToken cancellationToken)

- 说明：停止所有插件
- 参数：
  - cancellationToken： CancellationToken，取消监听令牌
- 返回：Task

## IPluginContext

`IPluginContext`插件上下文，是插件运行时的上下文数据，将传递给插件的相关方法

### PluginFactory属性

- 说明：只读，当前的IPluginFactory实例

### ServiceProvider属性

- 说明：只读，当前的IServiceProvider实例

### CancellationToken属性

- 说明：只读，关联的取消监听令牌，你可以通过此令牌判断上层操作是否已被取消

## IPluginInitContext

`IPluginInitContext`插件初始化上下文，由支持初始化的插件接口[ISupportInitPlugin](#ISupportInitPlugin)的Init方法使用

### PluginPath属性

- 说明：只读，插件路径

### PluginLoader属性

- 说明：只读，当前关联的插件载入器, 请参考[IPluginLoader](#IPluginLoader)

### ServiceCollection属性

- 说明：只读，应用的依赖注入服务容器，通过此属性可注入新的服务

### InitServiceProvider属性

- 说明：只读，初始化所使用的服务提供器，通过此属性可获取依赖的其他服务，*注意通过此提供器可获取初始化插件工厂方法之前所注入的服务。*

## IPlugin

`IPlugin`是插件的基础接口，所有插件需实现此接口

### StartAsync(IPluginContext context)

- 说明：启动插件
- 参数：
  - context, [IPluginContext](#IPluginContext), 插件上下文
- 返回：Task

### Task StopAsync(IPluginContext context)

- 说明：停止插件
- 参数：
  - context, [IPluginContext](#IPluginContext), 插件上下文
- 返回：Task

## ISupportInitPlugin

`ISupportInitPlugin`接口用于实现插件的初始化机制

### Init(IPluginInitContext context)

- 说明：插件初始化
- 参数：
  - context：[IPluginInitContext](#IPluginInitContext), 插件初始化上下文

## ISupportConfigPlugin

`ISupportConfigPlugin`接口用于实现插件的配置机制，它是泛型接口，泛型参数为`TOptions`，表示插件的配置类型

## PluginFactoryOptions

`PluginFactoryOptions`是插件工厂的配置类型

### PluginPath属性

- 说明：获取或设置插件工厂所使用的插件路径

### FileProvider属性

- 说明：获取或设置插件载入时所使用的文件提供器，默认将使用`PhysicalFileProvider`提供器

### DisabledPluginList属性

- 说明，只读，获取当前被禁用的插件列表

### AddAssembly(Assembly assembly)

- 说明：添加插件程序集
- 参数：
  - assembly：Assembly，插件程序集

## PluginInfo

## PluginBase

## SupportConfigPluginBase

## PluginAttribute