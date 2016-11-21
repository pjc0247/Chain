using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace Chain.Github
{
    public class GithubCredentials : IServiceCredentials
    {
        public string BaseUrl = "https://github.com";
        public string ProductName = "Chain.Github";
        public string Username;
        public string Password;

        public object CreateClient()
        {
            var client = new GitHubClient(
                new ProductHeaderValue(ProductName),
                new Uri(BaseUrl));
            client.Credentials = new Credentials(Username, Password);

            return client;
        }
    }
}
