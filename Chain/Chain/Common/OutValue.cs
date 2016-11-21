using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Common
{
    public class OutValue : ChainTask
    {
        private string Key;
        private object Value;

        public OutValue(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public override void OnExecute()
        {
            Console.WriteLine($"[{Key}] = {Value}");
            Context.Out(Key, Value);
        }
    }
}
