using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWC.WebConnect.Types
{
    public abstract class HttpConnection
    {
        public string Url { get; set; }
        public NameValueCollection Headers { get; set; }
        public NameValueCollection PostData { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }

        public HttpConnection(string url, NameValueCollection headers, string contentType)
        {
            PostData = new NameValueCollection();
            Url = Uri.EscapeUriString(url);
            Headers = headers ?? new NameValueCollection();
            ContentType = contentType;
        }
        public abstract Task Setup(WebRequest request);
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Url, Headers.ToString(), PostData.ToString(), Type, ContentType);
        }
    }
}
