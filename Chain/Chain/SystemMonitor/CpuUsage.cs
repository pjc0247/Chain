using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chain.SystemMonitor
{
    public class CpuUsage : IEventPublisher
    {
        public int DefaultPollingInterval
        {
            get
            {
                return 5000;
            }
        }

        public Task<IEnumerable<Event>> GetEvents()
        {
            var cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            cpuCounter.NextValue();
            Thread.Sleep(1000); // http://stackoverflow.com/questions/278071/how-to-get-the-cpu-usage-in-c

            return Task.FromResult<IEnumerable<Event>>(new Event[]
            {
                new CpuUsageInfo()
                {
                    ProcessorCount = Environment.ProcessorCount,
                    Time = DateTime.Now,
                    Value = cpuCounter.NextValue()
                }
            });
        }
    }
}
