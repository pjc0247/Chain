using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Github
{
    using Git;

    public class RemoteRepository : Event
    {
        public GitRemotePoint From;
        public string Sha;
    }

    public class CommitPushedEvent : Event
    {
        public GitRemotePoint From;
        public User User;
        public string Sha;

        public override string ToString()
        {
            return $"{User.Name} pushed to `{From.BranchName}` at {From.Owner}/{From.RepositoryName}";
        }
    }

    public class CommentEvent : Event
    {
        public GitRemotePoint From;
        public User User;

        public string Message;
    }
}
