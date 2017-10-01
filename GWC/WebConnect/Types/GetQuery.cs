using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWC.WebConnect.Types
{
    public class GetQuery : HttpConnection
    {
        public GetQuery(string url, NameValueCollection headers, string contentType = "application/x-www-form-urlencoded") : base(url, headers, contentType)
        {
            Type = "GET";
        }

        public async override Task Setup(WebRequest request)
        {
            await Task.CompletedTask;
        }
    }
}
