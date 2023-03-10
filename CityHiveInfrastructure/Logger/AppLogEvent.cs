using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveInfrastructure.Logger
{
    public class AppLogEvent : IEnumerable<KeyValuePair<string, object>>
    {
        List<KeyValuePair<string, object>> _properties = new List<KeyValuePair<string, object>>();

        public string Message { get; }

        public AppLogEvent(string message)
        {
            Message = message;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public AppLogEvent AddProp(string name, object value)
        {
            _properties.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }
        public AppLogEvent ReplaceProp(string name, object value)
        {
            _properties.RemoveAll(prop => prop.Key == name);
            AddProp(name, value);
            return this;
        }

        public static Func<AppLogEvent, Exception, string> Formatter { get; } = (l, e) => l.Message;
    }
}
