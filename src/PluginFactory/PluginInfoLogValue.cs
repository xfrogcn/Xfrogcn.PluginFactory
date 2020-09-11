using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xfrogcn.PluginFactory
{

    /// <summary>
    /// 记录插件信息的日志值，可用于Logger作用域或日志记录
    /// </summary>
    internal class PluginInfoLogValue : IReadOnlyList<KeyValuePair<string, object>>
    {
        private PluginInfo _pluginInfo = null;

        private string _formatted = null;

        private List<KeyValuePair<string, object>> _values;
        public PluginInfoLogValue(PluginInfo pluginInfo)
        {
            if (pluginInfo == null)
            {
                throw new ArgumentNullException(nameof(pluginInfo));
            }
            _pluginInfo = pluginInfo;
        }

        private List<KeyValuePair<string, object>> Values
        {
            get
            {
                if (_values == null)
                {
                    var values = new List<KeyValuePair<string, object>>();
                    values.Add(new KeyValuePair<string, object>("PluginID", _pluginInfo.Id));
                    values.Add(new KeyValuePair<string, object>("PluginName", _pluginInfo.Name));
                    values.Add(new KeyValuePair<string, object>("PluginAlias", _pluginInfo.Alias??string.Empty));
                    values.Add(new KeyValuePair<string, object>("PluginType", _pluginInfo.PluginType.FullName));
                    _values = values;
                }

                return _values;
            }
        }





        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                return Values[index];
            }
        }

        public int Count => Values.Count;

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public override string ToString()
        {
            if(_formatted == null)
            {
                var builder = new StringBuilder();
                builder.AppendLine(Resources.PluginInfo + ": ");

                for (var i = 0; i < Values.Count; i++)
                {
                    var kvp = Values[i];
                    builder.Append(kvp.Key);
                    builder.Append(": ");

                    foreach (var value in (IEnumerable<object>)kvp.Value)
                    {
                        builder.Append(value);
                        builder.Append(", ");
                    }

                    builder.Remove(builder.Length - 2, 2);
                    builder.AppendLine();
                }

                _formatted = builder.ToString();
            }
            return _formatted;
        }
    }
}
