using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chain.HTTPServ
{
    public class HttpRequest : PendingRequest
    {
        public string Method;
        public NameValueCollection Headers;

        public byte[] Body;

        public HttpListenerContext RawContext;
    }
}
