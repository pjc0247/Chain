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
        private string LocalPath;

        public GitCheckout(string localPath)
        {
            LocalPath = localPath;
        }
        public GitCheckout()
        {
        }
        
        public override void OnExecute()
        {
            Require<GitRepository>();

            var co = new CloneOptions();
            co.CredentialsProvider = Context.CredentialProvider.Get<GitCredential, CredentialsHandler>();

            var gitRepo = Context.Get<GitRepository>();

            if (LocalPath == null)
                LocalPath = $"./{gitRepo.RemotePoint.Owner}_{gitRepo.RemotePoint.RepositoryName}_{gitRepo.Sha}";

            if (Directory.Exists(LocalPath))
            {
                Console.WriteLine($"Path({LocalPath}) already exists. clean.");
                Directory.Delete(LocalPath);
            }

            Repository.Clone(gitRepo.RemotePoint.Url, LocalPath, co);
            using (var repo = new Repository(LocalPath))
            {
                repo.Reset(ResetMode.Hard, gitRepo.Sha);
            }

            Context.Set(new LocalCopy()
            {
                Name = gitRepo.RemotePoint.Url,
                Path = LocalPath
            });
        }
    }
}
