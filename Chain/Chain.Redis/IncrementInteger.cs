using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace Chain.Redis
{
    public class IncrementInteger : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Key = nameof(Key);
        public static readonly string IN_Value = nameof(Value);
        #endregion

        private IDatabase Redis;

        private string Key;
        private int Value;

        public IncrementInteger()
        {
            Value = 1;
        }
        public IncrementInteger(string key)
        {
            Key = key;
            Value = 1;
        }
        public IncrementInteger(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public override void OnExecute()
        {
            Redis = Context.CredentialProvider.Get<RedisCredentials, IDatabase>();

            Redis.StringIncrement(Key, Value);
        }
    }
}
