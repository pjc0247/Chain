using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Stdio
{
    public class WriteLine : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Message = nameof(Message);
        #endregion

        private string Message;

        public WriteLine()
        {
        }
        public WriteLine(string message)
        {
            Message = message;
        }

        public override void OnExecute()
        {
            Console.WriteLine(Message);
        }
    }
}
