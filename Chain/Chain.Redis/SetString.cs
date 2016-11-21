using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace Chain.Redis
{
    public class SetString : ChainTask
    {
        private IDatabase Redis;

        private string Key;
        private string Value;
        private TimeSpan? Expiry;

        public SetString()
        {
        }
        public SetString(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override void OnExecute()
        {
            Redis = Context.CredentialProvider.Get<RedisCredentials, IDatabase>();

            Redis.StringSet(Key, Value, Expiry);
        }
    }
}
