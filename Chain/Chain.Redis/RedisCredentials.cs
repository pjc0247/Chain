using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace Chain.Redis
{
    public class RedisCredentials : IServiceCredentials
    {
        public string ConnectionString;
        public int DbIdx = -1;

        public object CreateClient()
        {
            return ConnectionMultiplexer.Connect(ConnectionString).GetDatabase(DbIdx);
        }
    }
}
