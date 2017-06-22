using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWC.WebConnect.Types
{
    class PostQuery : HttpConnection
    {
        public PostQuery(string url, NameValueCollection headers, NameValueCollection postdata, string contentType = "application/x-www-form-urlencoded") : base(url, headers, contentType)
        {
            Type = "POST";
            PostData = postdata;
        }

        public async override Task Setup(WebRequest request)
        {
            request.ContentType = ContentType;
            StringBuilder postDataString = new StringBuilder();
            foreach (string key in PostData)
                postDataString.Append(string.Format("{0}={1}&", key, PostData[key]));
            postDataString.Remove(postDataString.Length - 1, 1);
            byte[] postData = Encoding.UTF8.GetBytes(postDataString.ToString());
            request.ContentLength = postData.Length;
            using (Stream datastream = await request.GetRequestStreamAsync())
            {
                await datastream.WriteAsync(postData, 0, postData.Length);
            }
        }
    }
}
