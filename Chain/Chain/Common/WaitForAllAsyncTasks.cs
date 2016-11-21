using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Common
{
    public class WaitForAllAsyncTasks : ChainTask
    {
        private int? Timeout;

        public WaitForAllAsyncTasks()
        {
        }
        public WaitForAllAsyncTasks(int msTimeout)
        {
            Timeout = msTimeout;
        }

        public override void OnExecute()
        {
            Context.JoinAndClearAsyncThreads(
                Timeout.HasValue ? Timeout.Value : int.MaxValue);
        }
    }
}
