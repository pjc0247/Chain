using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class CloseIssue : ChainTask
    {
        private GitHubClient Github;

        private GitRemotePoint Repo;
        private int IssueNo;

        public CloseIssue()
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
            
            var issueUpdate = new IssueUpdate();
            issueUpdate.State = ItemState.Closed;

            var result = Github.Issue.Update(Repo.Owner, Repo.RepositoryName, IssueNo, issueUpdate).Result;
        }
    }
}
