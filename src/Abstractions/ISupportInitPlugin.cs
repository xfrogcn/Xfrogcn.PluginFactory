namespace PluginFactory
{
    /// <summary>
    /// 支持初始化的插件接口
    /// 初始化方法在构建ServiceProvider之前调用，可向服务容器中注入插件自定义的服务
    /// </summary>
    /// <remarks>
    /// 如果插件支持此接口，必须支持无参构造
    /// 注意：请毋在初始化接口中设置实例属性或变量，因为初始化方法与Start与Stop方法对应的
    /// 插件实例可能不同
    /// </remarks>
    public interface ISupportInitPlugin
    {
        void Init(IPluginInitContext context);
    }
}
