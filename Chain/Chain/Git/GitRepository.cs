using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Git
{
    public class GitRepository : Event
    {
        public GitRemotePoint RemotePoint;

        public string Sha;
    }
}
