using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Stdio
{
    public class ReadLine : IEventPublisher
    {
        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            var line = await Console.In.ReadLineAsync();

            return new Event[]
            {
                new ConsoleInput()
                {
                    Message = line
                }
            };
        }
    }
}
