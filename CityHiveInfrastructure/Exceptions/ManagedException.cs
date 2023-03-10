using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveInfrastructure.Exceptions
{
    public class ManagedException : Exception
    {
        private AppLayer layer;
        private AppModule module;

        public ManagedException(Exception innerException, AppLayer layer)
    : base("", innerException)
        {
            this.layer = layer;
        }

        public ManagedException(Exception innerException, string message, AppLayer layer)
            : base(message, innerException)
        {
            this.layer = layer;
        }

        public ManagedException(Exception innerException, AppModule module, AppLayer layer)
            : base("", innerException)
        {
            this.module = module;
            this.layer = layer;
        }
        public ManagedException(Exception innerException, string message, AppModule module, AppLayer layer)
            : base(message, innerException)
        {
            this.module = module;
            this.layer = layer;
        }
        public string Layer
        {
            get
            {
                return findLayer(this);
            }
        }

        public string Module
        {
            get
            {
                return findModule(this);
            }
        }
        public string Parameters
        {
            get
            {
                var paramDict = new Dictionary<object, object>();
                collectParameters(this, paramDict);
                var paramItems = paramDict.Select(x => x.Key + ": " + x.Value);
                return String.Join(", ", paramItems);
            }
        }

        public string Messages
        {
            get
            {
                var messageList = new List<string>();
                collectMessages(this, messageList);
                return String.Join(" > ", messageList);
            }
        }

        public string OriginalStackTrace
        {
            get
            {
                var messageList = new List<string>();
                collectStackTrace(this, messageList);
                return String.Join(" > ", messageList);
            }
        }

        private string findLayer(Exception ex)
        {
            if (ex.InnerException is ManagedException) return findLayer(ex.InnerException);
            else if (ex is ManagedException) return (ex as ManagedException).layer.ToString();
            else return "";
        }

        private string findModule(Exception ex)
        {
            if (ex is ManagedException && (ex as ManagedException).module != AppModule.UNDEFINED)
                return (ex as ManagedException).module.ToString();
            else if (ex.InnerException is ManagedException) return findModule(ex.InnerException);
            else return "";
        }

        private void collectParameters(Exception ex, Dictionary<object, object> paramDict)
        {
            if (ex is ManagedException)
            {
                foreach (var key in ex.Data.Keys)
                {
                    if (paramDict.ContainsKey(key)) continue;
                    var value = ex.Data[key];
                    paramDict[key] = value;
                }
            }
            if (ex.InnerException is ManagedException) collectParameters(ex.InnerException, paramDict);
            else return;
        }

        private void collectMessages(Exception ex, List<string> messageList)
        {
            if (ex.Message != null && ex.Message.Length != 0) messageList.Add(RemoveNewLines(ex.Message));

            if (ex.InnerException is Exception) collectMessages(ex.InnerException, messageList);
            else return;
        }

        private void collectStackTrace(Exception ex, List<string> messageList)
        {

            if (ex.StackTrace != null) messageList.Add(ex.StackTrace);
            if (ex.InnerException is Exception) collectStackTrace(ex.InnerException, messageList);
            else return;
        }

        private static string RemoveNewLines(string str)
        {
            return str.Trim().Replace("\r", "").Replace("\n", " ");
        }
    }

    public enum AppLayer
    {
        UNDEFINED,
        WEB_API,
        BUSINESS_LOGIC,
        DATABASE,
        BUSCONSUMERS
    }

    public enum AppModule
    {
        UNDEFINED,
        GENERAL_HANDLER,
        MESSAGE
    }
}
