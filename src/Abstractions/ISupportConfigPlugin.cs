namespace PluginFactory
{
    /// <summary>
    /// 支持配置的插件接口
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface ISupportConfigPlugin<TOptions>
        where TOptions: class, new()
    {
        
    }
}
