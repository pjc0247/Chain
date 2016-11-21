using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

namespace Chain.Google.Gmail
{
    public class SendMail : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Subject = nameof(Subject);
        public static readonly string IN_Body = nameof(Body);
        #endregion

        private string Subject;
        private string Body;

        public SendMail()
        {
        }

        public override void OnExecute()
        {
            GoogleWebAuthorizationBroker.Folder = "./";

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets()
                {
                    ClientId = "902758479430-rsobsqnqidq7uffdan3ne80hqqgtpg0i.apps.googleusercontent.com",
                    ClientSecret = "ChgOQ-_u2fmTcBvX8a9CsOGC"
                },
                new string[] { GmailService.Scope.GmailSend, GmailService.Scope.GmailReadonly },
                "user",
                CancellationToken.None).Result;

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "HELLO",
            });

            var a = new Message();

            var myAddr = service.Users.GetProfile("me").Execute().EmailAddress;
            Console.WriteLine(myAddr);

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(myAddr);
            mailMessage.To.Add("pjc0247@gmail.com");
            mailMessage.Subject = "SUBJECT";
            mailMessage.Body = "BODY";
            mailMessage.IsBodyHtml = true;

            service.Users.Messages.Send(new Message()
            {
                Raw = Encode(MimeKit.MimeMessage.CreateFromMailMessage(mailMessage).ToString())
            }, myAddr).Execute();
        }

        private static string Encode(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
