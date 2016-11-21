using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace Chain.Redis
{
    public class GetString : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Key = nameof(Key);
        #endregion
        #region OUT_KEYS
        public static readonly string OUT_Value = $"Redis.{nameof(GetString)}.Value";
        #endregion

        private IDatabase Redis;

        private string Key;

        public GetString()
        {
        }
        public GetString(string key)
        {
            Key = key;
        }

        public override void OnExecute()
        {
            Redis = Context.CredentialProvider.Get<RedisCredentials, IDatabase>();

            var value = Redis.StringGet(Key);

            Context.Out(OUT_Value, value.ToString());
        }
    }
}
