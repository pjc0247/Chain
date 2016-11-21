using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PushbulletSharp;

namespace Chain.PushBullet
{
    public class SendPush : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Title = nameof(Title);
        public static readonly string IN_Body = nameof(Body);
        #endregion

        private PushbulletClient PushBullet;

        private string Title;
        private string Body;

        public SendPush()
        {
        }
        public SendPush(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public override void OnExecute()
        {
            PushBullet = Context.CredentialProvider.Get<PushBulletCredential, PushbulletClient>();

            var response = PushBullet.PushNote(new PushbulletSharp.Models.Requests.PushNoteRequest()
            {
                Title = Title,
                Body = Body
            });
        }
    }
}
