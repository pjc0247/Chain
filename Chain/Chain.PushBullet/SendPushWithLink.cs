using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PushbulletSharp;

namespace Chain.PushBullet
{
    public class SendPushWithLink : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Title = nameof(Title);
        public static readonly string IN_Body = nameof(Body);
        public static readonly string IN_Link = nameof(Link);
        #endregion

        private string Title;
        private string Body;
        private string Link;

        private PushbulletClient PushBullet;

        public SendPushWithLink()
        {
        }
        public SendPushWithLink(string title, string body, string link)
        {
            Title = title;
            Body = body;
            Link = link;
        }

        public override void OnExecute()
        {
            PushBullet = Context.CredentialProvider.Get<PushBulletCredential, PushbulletClient>();

            var response = PushBullet.PushLink(new PushbulletSharp.Models.Requests.PushLinkRequest()
            {
                Title = Title,
                Body = Body,
                Url = Link
            });
        }
    }
}
