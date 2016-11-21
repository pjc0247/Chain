using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.SystemMonitor
{
    public class CpuUsageInfo : Event
    {
        public DateTime Time;
        public int ProcessorCount;
        public float Value;
    }
}
