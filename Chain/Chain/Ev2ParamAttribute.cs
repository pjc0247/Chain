using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    public class Ev2ParamAttribute : Attribute
    {
        public Type EventType;

        public Ev2ParamAttribute(Type eventType)
        {
            EventType = eventType;
        }
    }
}
