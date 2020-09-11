using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Xfrogcn.PluginFactory
{
    /// <summary>
    /// 默认的插件载入器
    /// </summary>
    public class DefaultPluginLoader : IPluginLoader
    {
        readonly PluginFactoryOptions _options;
        readonly IServiceCollection _services;

        public DefaultPluginLoader(PluginFactoryOptions options, IServiceCollection services)
        {
            _options = options;
            _services = services;
        }

        private List<PluginInfo> _pluginList = new List<PluginInfo>();
        public IReadOnlyList<PluginInfo> PluginList => _pluginList;

        public virtual void Load()
        {
            

            lock (_pluginList)
            {
                // 载入附加组件
                foreach(Assembly assembly in _options.AdditionalAssemblies)
                {
                    LoadPluginFromAssembly(assembly);
                }
                if (_options.FileProvider == null)
                {
                    return;
                }

                var dir = _options.FileProvider.GetDirectoryContents(string.Empty);

                if (!dir.Exists)
                {
                    return;
                }
                foreach (var p in dir)
                {
                    if (p.IsDirectory)
                    {
                        // 隔离插件
                        var pluginDir = _options.FileProvider.GetDirectoryContents(p.Name);
                        foreach (var pd in pluginDir)
                        {
                            if (pd.IsDirectory)
                            {
                                continue;
                            }
                            string fileName = Path.GetFileNameWithoutExtension(pd.PhysicalPath);
                            if (fileName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                // 插件程序集
                                LoadPluginFromAssembly(pd.PhysicalPath);
                            }

                        }
                    }
                    else if (p.PhysicalPath != null && Path.GetExtension(p.PhysicalPath) == ".dll")
                    {
                        //
                        LoadPluginFromAssembly(p.PhysicalPath);
                    }

                }
                
            }
        }

        protected virtual void LoadPluginFromAssembly(string assemblyPath)
        {
            if (_options.Predicate!=null && !_options.Predicate(assemblyPath))
            {
                return;
            }

            IsolationAssemblyLoadContext context = new IsolationAssemblyLoadContext(assemblyPath);
            var assembly = context.Load();
            if (assembly != null)
            {
                LoadPluginFromAssembly(assembly);
            }
        }


        protected virtual void LoadPluginFromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                // 异常
                throw new ArgumentNullException(nameof(assembly));
            }

            var types = assembly.GetExportedTypes();
            List<PluginInfo> plist = new List<PluginInfo>();
            foreach (Type t in types)
            {
                PluginInfo pi = LoadPluginFromType(t);
                if (pi != null && !_pluginList.Any(p => p.PluginType == pi.PluginType))
                {
                    _pluginList.Add(pi);
                }
            }
  
        }

        protected virtual PluginInfo LoadPluginFromType(Type type)
        {
            if(type.IsAbstract)
            {
                return null;
            }

            Type[] iTypes = type.GetInterfaces();
            if(iTypes==null || iTypes.Length == 0)
            {
                return null;
            }
            var pluginType = typeof(IPlugin);

            

            PluginInfo pi = null;
            if (typeof(IPlugin).GetTypeInfo().IsAssignableFrom(type))
            {
                pi = new PluginInfo()
                {
                    PluginType = type,
                    CanInit = false,
                    CanConfig = false
                };
            }
            if(pi == null)
            {
                return null;
            }

            var attr = type.GetCustomAttributes(typeof(PluginAttribute), false).OfType<PluginAttribute>().FirstOrDefault();
            if(attr != null)
            {
                pi.Id = attr.Id;
                pi.Name = attr.Name;
                pi.Alias = attr.Alias;
                pi.Description = attr.Description;
            }
            pi.Id = string.IsNullOrEmpty(pi.Id) ? type.FullName : pi.Id;
            pi.Name = string.IsNullOrEmpty(pi.Name) ? (string.IsNullOrEmpty(pi.Alias) ? type.FullName : pi.Alias) : pi.Name;

            // 初始化
            if (typeof(ISupportInitPlugin).IsAssignableFrom(type))
            {
                pi.CanInit = true;
            }

            // 配置
            Type cfgType = iTypes.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISupportConfigPlugin<>));
            if (cfgType != null)
            {
                pi.ConfigType = cfgType.GenericTypeArguments[0];
                pi.CanConfig = true;
            }

            // 是否禁用
            if ( (!String.IsNullOrEmpty(pi.Alias) && _options.DisabledPluginList.Contains(pi.Alias))  ||
                _options.DisabledPluginList.Contains(pi.Name) ||
                _options.DisabledPluginList.Contains(pi.PluginType.FullName) ||
                _options.DisabledPluginList.Contains(pi.PluginType.FullName.Replace("+",".")))
            {
                pi.IsEnable = false;
            }

            return pi;
        }


        public virtual void Init()
        {
            var initList = _pluginList.Where(x => x.CanInit && x.IsEnable).ToList();
            if (initList.Count == 0)
            {
                return;
            }

            IPluginInitContext initContext = new PluginInitContext(_options.PluginPath, this, _services);
            var initInstanceList = initContext.InitServiceProvider.GetRequiredService<IEnumerable<ISupportInitPlugin>>();
            foreach (ISupportInitPlugin p in initInstanceList)
            {
                p.Init(initContext);
            }

        }

        
    }
}
