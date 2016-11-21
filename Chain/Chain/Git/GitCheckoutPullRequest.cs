using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Chain.Git
{
    public class GitCheckoutPullRequest : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Repo = nameof(Repo);
        public static readonly string IN_PullRequestNo = nameof(PullRequestNo);
        public static readonly string IN_LocalPath = nameof(LocalPath);
        #endregion

        private GitRemotePoint Repo;
        private int PullRequestNo;
        private string LocalPath;

        public GitCheckoutPullRequest(string localPath)
        {
            LocalPath = localPath;
        }
        public GitCheckoutPullRequest()
        {
        }

        [Ev2Param(typeof(GitPullRequest))]
        private void OnGitRepository(GitPullRequest ev)
        {
            Repo = ev.RemotePoint;
            PullRequestNo = ev.No;
        }

        public override void OnExecute()
        {
            var co = new CloneOptions();
            co.CredentialsProvider = Context.CredentialProvider.Get<GitCredential, CredentialsHandler>();

            if (LocalPath == null)
                LocalPath = $"./{Context.WorkingDirectory}/{Repo.Owner}_{Repo.RepositoryName}_PR_{PullRequestNo}";

            if (Directory.Exists(LocalPath))
            {
                Console.WriteLine($"Path({LocalPath}) already exists. clean.");
                Directory.Delete(LocalPath);
            }

            Repository.Clone(Repo.Url, LocalPath, co);

            using (var repo = new Repository(LocalPath))
            {
                FetchOptions options = new FetchOptions();

                Commands.Fetch(repo, "origin", new string[] {
                    $"pull/{PullRequestNo}/head"
                }, options, "");
                repo.Checkout(Repo.BranchName);
            }

            Context.Set(new LocalCopy()
            {
                Name = Repo.Url,
                Path = LocalPath
            });
        }
    }
}
