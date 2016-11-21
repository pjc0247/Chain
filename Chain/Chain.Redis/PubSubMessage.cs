using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Redis
{
    public class PubSubMessage : Event
    {
        public string Channel;
        public string Message;
    }
}
