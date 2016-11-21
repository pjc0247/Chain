using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace Chain.Redis
{
    public class PublishString : ChainTask
    {
        private IDatabase Redis;

        private string Channel;
        private string Message;

        public PublishString()
        {
        }
        public PublishString(string channel, string message)
        {
            Channel = channel;
            Message = message;
        }

        public override void OnExecute()
        {
            Redis = Context.CredentialProvider.Get<RedisCredentials, IDatabase>();

            Redis.Publish(Channel, Message);
        }
    }
}
