using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class NewPullRequestEventPublisher : IEventPublisher
    {
        private GitHubClient Github;
        private GitRemotePoint Repo;

        private int LastPullRequestNo = -1;

        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public NewPullRequestEventPublisher(string owner, string repositoryName)
        {
            Repo = new GitRemotePoint()
            {
                Host = Config.Get("github.host", "https://github.com"),
                Owner = owner,
                RepositoryName = repositoryName
            };

            Github = new GitHubClient(
                new ProductHeaderValue("Chain.Github"),
                new Uri(Config.Get("github.host", "https://github.com")));
            Github.Credentials = new Credentials(
                Config.Get("github.id", ""), Config.Get("github.password", ""));
        }

        private bool IsFreshPullRequest(PullRequest pullRequest)
        {
            return (DateTime.Now - pullRequest.CreatedAt).TotalMinutes <= 10;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            IEnumerable<Event> result = null;

            var request = new PullRequestRequest()
            {
                SortDirection = SortDirection.Descending,
                SortProperty = PullRequestSort.Created
            };

            var pullRequests = await Github.PullRequest.GetAllForRepository(
                Repo.Owner, Repo.RepositoryName, request);

            if (LastPullRequestNo == -1 ||
                LastPullRequestNo == pullRequests.FirstOrDefault()?.Number)
                result = new Event[] { };
            else
            {
                var resultList = new List<Event>();

                foreach (var pullRequest in pullRequests)
                {
                    if (pullRequest.Number == LastPullRequestNo)
                        break;
                    if (IsFreshPullRequest(pullRequest) == false)
                        break;
                    
                    var user = new User()
                    {
                        Name = pullRequest.User.Login
                    };

                    var gitPullRequest = new GitPullRequest()
                    {
                        RemotePoint = Repo,
                        No = pullRequest.Number
                    };
                    gitPullRequest.RemotePoint.BranchName = pullRequest.Base.Ref;

                    resultList.Add(gitPullRequest);
                    resultList.Add(new IssueOrPullRequest()
                    {
                        From = Repo,
                        User = user,
                        Url = pullRequest.HtmlUrl.AbsoluteUri,
                        Title = pullRequest.Title,
                        No = pullRequest.Number
                    });
                }

                result = resultList;
            }

            LastPullRequestNo = pullRequests.FirstOrDefault()?.Number ?? -1;

            return result;
        }
    }
}
