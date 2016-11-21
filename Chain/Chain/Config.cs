using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Chain
{
    public class Config
    {
        private static Dictionary<string, object> Storage
            = new Dictionary<string, object>();

        public static T Get<T>(string key, T defaultValue)
        {
            if (Storage.ContainsKey(key) == false)
                return defaultValue;
            return (T)Storage[key];
        }
        public static void Set(string key, object value)
        {
            Storage[key] = value;
        }

        public static void SetCredential<TCRED>(string key, TCRED obj)
            where TCRED : IServiceCredentials
        {
            Set("credentials." + typeof(TCRED).FullName + "." + key, JsonConvert.SerializeObject(obj));
        }
        public static void SetCredential<TCRED>(TCRED obj)
            where TCRED : IServiceCredentials
        {
            SetCredential<TCRED>("default", obj);
        }
    }
}
