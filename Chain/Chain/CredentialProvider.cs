using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Chain
{
    public class CredentialProvider
    {
        private Dictionary<string, string> KeyStorage
            = new Dictionary<string, string>();
        private Dictionary<string, string> KeyOverrides
            = new Dictionary<string, string>();

        private string GetKey<TCRED>()
            where TCRED : IServiceCredential
        {
            var typeKey = typeof(TCRED).FullName;

            if (KeyOverrides.ContainsKey(typeKey))
                return KeyOverrides[typeKey];
            if (KeyStorage.ContainsKey(typeKey))
                return KeyStorage[typeKey];

            return "default";
        }
        public void SetKey<TCRED>(string key)
            where TCRED : IServiceCredential
        {
            var typeKey = typeof(TCRED).FullName;

            KeyStorage[typeKey] = key;
        }
        public void SetKeyOnce<TCRED>(string key)
        {
            var typeKey = typeof(TCRED).FullName;

            KeyOverrides[typeKey] = key;
        }
        public void SetKeyOnce(Type type, string key)
        {
            var typeKey = type.FullName;

            KeyOverrides[typeKey] = key;
        }

        public TCLIENT Get<TCRED, TCLIENT>()
            where TCRED : IServiceCredential
        {
            var json = Config.Get<string>("credentials." + GetKey<TCRED>(), null);

            if (json == null)
                throw new InvalidOperationException("no credential");

            return (TCLIENT)JsonConvert.DeserializeObject<TCRED>(json).CreateClient();
        }

        public void Reset()
        {
            KeyOverrides = new Dictionary<string, string>();
        }
    }
}
