using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Stdio
{
    public class Publisher : IEventPublisher
    {
        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public Task<IEnumerable<Event>> GetEvents()
        {
            throw new NotImplementedException();
        }
    }
}
