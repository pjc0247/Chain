using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Chain.HTTPServ
{
    public class HTTPServer : IEventPublisher
    {
        public static readonly int DefaultPort = 8080;

        private HttpListener Serv;

        private ConcurrentQueue<HttpListenerContext> RequestQueue
            = new ConcurrentQueue<HttpListenerContext>();

        public int DefaultPollingInterval
        {
            get
            {
                return 1000;
            }
        }

        public HTTPServer()
        {
            SetupServer($"http://+:{DefaultPort}/");
        }
        public HTTPServer(int port)
        {
            SetupServer($"http://+:{port}/");
        }
        public HTTPServer(string prefix)
        {
            SetupServer(prefix);
        }

        private void SetupServer(string prefix)
        {
            Serv = new HttpListener();

            Serv.Prefixes.Add(prefix);
            Serv.Start();

            new Thread(() => { WorkerThread(); }).Start();
        }

        private void WorkerThread()
        {
            while(true){
                var reqCtx = Serv.GetContext();

                // Request Too Long
                if (reqCtx.Request.ContentLength64 >= int.MaxValue)
                {
                    reqCtx.Response.Abort();
                    continue;
                }

                RequestQueue.Enqueue(reqCtx);
            }
        }

        public Task<IEnumerable<Event>> GetEvents()
        {
            var events = new List<Event>();

            while (RequestQueue.IsEmpty == false)
            {
                HttpListenerContext reqCtx;
                byte[] body = null;

                if (RequestQueue.TryDequeue(out reqCtx) == false)
                    break;

                if (reqCtx.Request.HasEntityBody) {
                    body = new byte[reqCtx.Request.ContentLength64];
                    reqCtx.Request.InputStream.Read(body, 0, (int)reqCtx.Request.ContentLength64);
                }

                events.Add(new HttpRequest()
                {
                    RawContext = reqCtx,
                    Method = reqCtx.Request.HttpMethod,
                    Headers = reqCtx.Request.Headers,
                    Body = body
                });
            }

            return Task.FromResult<IEnumerable<Event>>(events);
        }
    }
}
