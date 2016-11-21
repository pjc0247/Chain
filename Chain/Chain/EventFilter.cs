using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    public class EventFilter
    {
        public virtual bool OnExecute(Event e)
        {
            return false;
        }
    }
}
