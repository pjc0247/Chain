using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Chain.KakaoTalk
{
    public class SendKakaoTalkMessage : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Receiver = nameof(Receiver);
        public static readonly string IN_Message = nameof(Message);
        #endregion

        private static readonly string Url = "http://";

        private string Receiver;
        private string Message;

        public SendKakaoTalkMessage(string receiver, string message)
        {
            Receiver = receiver;
            Message = message;
        }

        public override void OnExecute()
        {
            var http = new HttpClient();
            var content = new FormUrlEncodedContent(
                new Dictionary<string, string>()
                {
                    // ㅎ
                });

            http.PostAsync(Url, content).Wait();
        }
    }
}
