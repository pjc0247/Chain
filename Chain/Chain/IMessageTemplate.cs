using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    public interface IMessageTemplate
    {
        string OnExecute(TaskContext Context);
    }
}
