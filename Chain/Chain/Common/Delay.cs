using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chain.Common
{
    public class Delay : ChainTask
    {
        private int Ms;

        public Delay(int ms)
        {
            Ms = ms;
        }

        public override void OnExecute()
        {
            Thread.Sleep(Ms);  
        }
    }
}
