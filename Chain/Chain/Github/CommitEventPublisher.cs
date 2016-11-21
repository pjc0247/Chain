using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class CommitEventPublisher : IEventPublisher
    {
        private GitHubClient Github;
        private GitRemotePoint Repo;

        private string LastCommitSha = null;

        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public CommitEventPublisher(string owner, string repositoryName, string branchName)
        {
            Repo = new GitRemotePoint()
            {
                Host = Config.Get("github.host", "https://github.com"),
                Owner = owner,
                RepositoryName = repositoryName,
                BranchName = branchName
            };

            Github = new GitHubClient(
                new ProductHeaderValue("Chain.Github"),
                new Uri(Config.Get("github.host", "https://github.com")));
            Github.Credentials = new Credentials(
                Config.Get("github.id", ""), Config.Get("github.password", ""));
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            IEnumerable<Event> result = null;
            var commit = await Github.Repository.Commit.Get(
                Repo.Owner, Repo.RepositoryName, Repo.BranchName);

            if (LastCommitSha == null ||
                LastCommitSha == commit.Sha)
                result = new Event[] { };
            else
            {
                var commiter = new User()
                {
                    Name = commit.Author.Login
                };
                result = new Event[] {
                    new Git.GitRepository()
                    {
                        RemotePoint = Repo,
                        Sha = commit.Sha
                    },
                    new CommitPushedEvent() {
                        From = Repo,
                        User = commiter,
                        Sha = commit.Sha
                    }
                };
            }

            LastCommitSha = commit.Sha;

            return result;
        }
    }
}
