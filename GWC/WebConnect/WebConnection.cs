using GWC.WebConnect.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWC.WebConnect
{
    public class WebConnection
    {
        public async Task<HttpWebResponse> MakeRequest(HttpConnection connection)
        {
            WebRequest HttpRequest = WebRequest.Create(connection.Url);
            HttpRequest.Headers.Add(connection.Headers);
            HttpRequest.Method = connection.Type;
            await connection.Setup(HttpRequest);
            try
            {
                return (HttpWebResponse)await HttpRequest.GetResponseAsync();
            }
            catch (WebException we)
            {
                return (HttpWebResponse)we.Response;
            }

        }
    }
}
