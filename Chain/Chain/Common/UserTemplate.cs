using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Common
{
    public class UserTemplate : IMessageTemplate
    {
        private Func<TaskContext, string> Callback;

        public UserTemplate(Func<TaskContext, string> callback)
        {
            Callback = callback;
        }

        public string OnExecute(TaskContext Context)
        {
            return Callback(Context);
        }
    }
}
