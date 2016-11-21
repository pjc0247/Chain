using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Chain.HTTPServ
{
    public class HTTPResponseString : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Message = nameof(Message);
        #endregion

        private string Message = "";

        public HTTPResponseString()
        {
        }
        public HTTPResponseString(string message)
        {
            Message = message;
        }

        public override void OnExecute()
        {
            Require<HttpRequest>();

            var request = Context.Get<HttpRequest>();
            var bytes = Encoding.UTF8.GetBytes(Message);

            request.RawContext.Response.Close(bytes, true);
        }
    }
}
