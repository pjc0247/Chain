using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class AssignIssue : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Repo = nameof(Repo);
        public static readonly string IN_IssueNo = nameof(IssueNo);
        public static readonly string IN_Assignee = nameof(Assignee);
        #endregion

        private GitHubClient Github;

        private GitRemotePoint Repo;
        private int IssueNo;
        private string Assignee;

        public AssignIssue()
        {
        }
        public AssignIssue(string assignee)
        {
            Assignee = assignee;
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
            issueUpdate.Assignee = Assignee;

            var result = Github.Issue.Update(Repo.Owner, Repo.RepositoryName, IssueNo, issueUpdate).Result;
        }
    }
}
