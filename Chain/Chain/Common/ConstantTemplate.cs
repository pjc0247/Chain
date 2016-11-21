using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Common
{
    public class ConstantTemplate : IMessageTemplate
    {
        private string Message;

        public ConstantTemplate(string message)
        {
            Message = message;
        }

        public string OnExecute(TaskContext Context)
        {
            return Message;
        }
    }
}
