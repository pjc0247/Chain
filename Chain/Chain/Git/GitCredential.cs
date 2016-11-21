using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Chain.Git
{
    public class GitCredential : IServiceCredentials
    {
        public string Username;
        public string Password;

        public object CreateClient()
        {
            return new CredentialsHandler((_url, _user, _cred) => new UsernamePasswordCredentials
            {
                Username = Username,
                Password = Password
            });
        }
    }
}
