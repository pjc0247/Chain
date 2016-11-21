using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class WriteIssueComment : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Message = nameof(Message);
        #endregion

        private GitHubClient Github;

        private GitRemotePoint Repo;
        private int IssueNo;
        private string Message;

        public WriteIssueComment()
        {
        }

        [Ev2Param(typeof(IssueOrPullRequest))]
        private void OnIssueOrPullRequest(IssueOrPullRequest ev)
        {
            Repo = ev.From;
            IssueNo = ev.No;
        }

        public override void OnExecute()
        {
            Github = Context.CredentialProvider.Get<GithubCredentials, GitHubClient>();
            
            Github.Issue.Comment.Create(
                Repo.Owner, Repo.RepositoryName,
                IssueNo,
                Message);
        }
    }
}
