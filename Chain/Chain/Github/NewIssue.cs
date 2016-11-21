using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class NewIssue : IEventPublisher
    {
        private GitHubClient Github;
        private GitRemotePoint Repo;

        private int LastIssueNo = -1;

        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public NewIssue(string owner, string repositoryName)
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

        private bool IsFreshIssue(Issue issue)
        {
            return (DateTime.Now - issue.CreatedAt).TotalMinutes <= 10;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            IEnumerable<Event> result = null;

            var request = new RepositoryIssueRequest()
            {
                SortDirection = SortDirection.Descending,
                SortProperty = IssueSort.Created
            };

            var issues = await Github.Issue.GetAllForRepository(
                Repo.Owner, Repo.RepositoryName, request);

            if (LastIssueNo == -1 ||
                LastIssueNo == issues.FirstOrDefault()?.Number)
                result = new Event[] { };
            else
            {
                var resultList = new List<Event>();

                foreach (var issue in issues)
                {
                    if (issue.Number == LastIssueNo)
                        break;
                    if (IsFreshIssue(issue) == false)
                        break;

                    var user = new User()
                    {
                        Name = issue.User.Login
                    };
                    resultList.Add(new IssueOrPullRequest()
                    {
                        From = Repo,
                        User = user,
                        Url = issue.HtmlUrl.AbsoluteUri,
                        Title = issue.Title,
                        No = issue.Number
                    });
                }

                result = resultList;
            }

            LastIssueNo = issues.FirstOrDefault()?.Number ?? -1;

            return result;
        }
    }
}
