using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlackAPI;

namespace Chain.Slack
{
    public class SlackCredentials : IServiceCredentials
    {
        public string AccessToken;

        public object CreateClient()
        {
            return new SlackSocketClient(AccessToken);
        }
    }
}
