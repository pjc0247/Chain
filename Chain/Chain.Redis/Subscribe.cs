using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace Chain.Redis
{
    public class Subscribe : IEventPublisher
    {
        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        private string Channel;

        private ConcurrentQueue<RedisValue> Messages
            = new ConcurrentQueue<RedisValue>();

        public Subscribe(string channel)
        {
            Channel = channel;

            var Redis = new CredentialsProvider().Get<RedisCredentials, IDatabase>();

            Redis.Multiplexer.GetSubscriber().Subscribe(
                Channel,
                (ch, value) =>
                {
                    Messages.Enqueue(value);
                });
        }

        public Task<IEnumerable<Event>> GetEvents()
        {
            var resultList = new List<Event>();
            
            while (Messages.IsEmpty == false)
            {
                RedisValue redisMessage;

                if (Messages.TryDequeue(out redisMessage) == false)
                    break;

                resultList.Add(new PubSubMessage()
                {
                    Channel = Channel,
                    Message = redisMessage
                });
            }

            return Task.FromResult(resultList.AsEnumerable());
        }
    }
}
