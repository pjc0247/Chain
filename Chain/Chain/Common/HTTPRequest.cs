using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Chain.Common
{
    public class HttpGetRequest : ChainTask
    {
        private string Url;
        private HttpStatusCode ExpectedStatusCode;

        private HttpClient Http;

        public HttpGetRequest(string url)
        {
            Url = url;
            ExpectedStatusCode = HttpStatusCode.OK;

            Http = new HttpClient();
        }
        public HttpGetRequest(string url, HttpStatusCode expectedStatusCode)
        {
            Url = url;
            ExpectedStatusCode = expectedStatusCode;

            Http = new HttpClient();
        }

        public override void OnExecute()
        {
            var response = Http.GetAsync(Url).Result;

            if (response.StatusCode == ExpectedStatusCode)
                throw new InvalidOperationException($"HttpGetResponseCode => {response.StatusCode}");
        }
    }
}
