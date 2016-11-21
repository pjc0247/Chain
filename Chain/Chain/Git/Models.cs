using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Git
{
    public class GitRemotePoint
    {
        public string Host;
        public string Owner;
        public string RepositoryName;
        public string BranchName;

        public string Url
        {
            get
            {
                return $"{Host}/{Owner}/{RepositoryName}";
            }
        }
    }
}
