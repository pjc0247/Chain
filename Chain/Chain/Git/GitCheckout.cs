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
    public class GitCheckout : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Repo = nameof(Repo);
        public static readonly string IN_Sha = nameof(Sha);
        public static readonly string IN_LocalPath = nameof(LocalPath);
        #endregion

        private GitRemotePoint Repo;
        private string Sha;
        private string LocalPath;

        public GitCheckout(string localPath)
        {
            LocalPath = localPath;
        }
        public GitCheckout()
        {
        }
        
        [Ev2Param(typeof(GitRepository))]
        private void OnGitRepository(GitRepository ev)
        {
            Repo = ev.RemotePoint;
            Sha = ev.Sha;
        }

        public override void OnExecute()
        {
            var co = new CloneOptions();
            co.CredentialsProvider = Context.CredentialProvider.Get<GitCredential, CredentialsHandler>();

            if (LocalPath == null)
                LocalPath = $"./{Context.WorkingDirectory}/{Repo.Owner}_{Repo.RepositoryName}_{Sha}";

            if (Directory.Exists(LocalPath))
            {
                Console.WriteLine($"Path({LocalPath}) already exists. clean.");
                Directory.Delete(LocalPath);
            }

            Repository.Clone(Repo.Url, LocalPath, co);

            // 지정된 SHA로 리셋
            if (string.IsNullOrEmpty(Sha) == false)
            {
                using (var repo = new Repository(LocalPath))
                {
                    repo.Reset(ResetMode.Hard, Sha);
                }
            }

            Context.Set(new LocalCopy()
            {
                Name = Repo.Url,
                Path = LocalPath
            });
        }
    }
}
