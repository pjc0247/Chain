using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    using Git;

    public class CommentEventPublisher : IEventPublisher
    {
        private GitHubClient Github;
        private GitRemotePoint Repo;

        private int LastCommentId = -1;

        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public CommentEventPublisher(string owner, string repositoryName)
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

        public async Task<IEnumerable<Event>> GetEvents()
        {
            IEnumerable<Event> result = null;
            var _comments = await Github.Issue.Comment.GetAllForRepository(
                Repo.Owner, Repo.RepositoryName);
            var comments = _comments.Reverse(); // TEMP

            if (LastCommentId == -1 ||
                LastCommentId == comments.FirstOrDefault()?.Id)
                result = new Event[] { };
            else
            {
                var resultList = new List<Event>();

                foreach (var comment in comments)
                {
                    if (comment.Id == LastCommentId)
                        break;

                    var user = new User()
                    {
                        Name = comment.User.Name
                    };
                    resultList.Add(new CommentEvent()
                    {
                        From = Repo,
                        User = user,
                        Message = comment.Body
                    });
                }

                result = resultList;
            }

            LastCommentId = comments.FirstOrDefault()?.Id ?? -1;

            return result;
        }
    }
}
