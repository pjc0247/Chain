using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Common
{
    public class UserTask : ChainTask
    {
        private Action<TaskContext> Callback;

        public UserTask(Action<TaskContext> callback)
        {
            Callback = callback;
        }

        public override void OnExecute()
        {
            Callback?.Invoke(Context);
        }
    }
}
