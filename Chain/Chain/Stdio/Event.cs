using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Stdio
{
    public class ConsoleInput : Event
    {
        public string Message;

        public override string ToString()
        {
            return Message;
        }
    }
}
