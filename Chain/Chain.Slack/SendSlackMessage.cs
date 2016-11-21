using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlackAPI;

namespace Chain.Slack
{
    public class SendSlackMessage : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Channel = nameof(Channel);
        public static readonly string IN_Message = nameof(Message);
        #endregion

        private string Channel;
        private string Message;

        private SlackClient Slack;

        public SendSlackMessage()
        {
        }
        public SendSlackMessage(string channel, string message)
        {
            Channel = channel;
            Message = message;
        }

        public override void OnExecute()
        {
            Slack = Context.CredentialProvider.Get<SlackCredentials, SlackClient>();

            Slack.PostMessage(
                _ => { },
                Channel, Message,
                botName: "github");
        }
    }
}
