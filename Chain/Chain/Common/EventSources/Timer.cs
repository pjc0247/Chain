using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Common.EventSources
{
    public class Timer : IEventPublisher
    {
        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        private int Interval;
        private int LastFired;

        public Timer(int msInterval)
        {
            Interval = msInterval;
            LastFired = Environment.TickCount;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            if (Environment.TickCount - LastFired >= Interval)
            {
                LastFired = Environment.TickCount;
                return new Event[] {
                    new Event()
                };
            }
            else
                return new Event[] { };
        }
    }
}
