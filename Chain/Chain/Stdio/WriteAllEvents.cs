using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Stdio
{
    public class WriteAllEvents : ChainTask
    {
        public WriteAllEvents()
        {
        }

        public override void OnExecute()
        {
            var events = Context.GetAll<Event>();

            foreach (var ev in events)
            {
                Console.WriteLine(ev);
            }
        }
    }
}
