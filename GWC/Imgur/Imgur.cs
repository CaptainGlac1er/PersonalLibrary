using GWC.FileSystem;
using GWC.Imgur.DataTypes;
using GWC.WebConnect;
using GWC.WebConnect.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWC.Imgur
{
    public class Imgur
    {
        private readonly string IMGURENDPOINT = "https://api.imgur.com/3/";
        ImgurInfo ImgurConnection;
        WebConnection WebConnection;
        public Imgur(WebConnection web)
        {
            WebConnection = web;

        }
        public async Task<bool> LoadConfig(FileData ConfigFile)
        {
            try
            {
                ImgurConnection = await ConfigFile.GetObjectFromJson<ImgurInfo>();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<ImgurAlbum> QuerySearch(string query)
        {
            NameValueCollection header = new NameValueCollection()
            {
                { "Authorization" , string.Format("Bearer {0}", ImgurConnection.ImgurConnectToken) }
            };
            HttpWebResponse response = await WebConnection.MakeRequest(new GetQuery(string.Format("{0}gallery/search?q={1}", IMGURENDPOINT, query), header));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                string json;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    json = await reader.ReadToEndAsync();
                }
                return JsonConvert.DeserializeObject<ImgurAlbum>(json);
            }
        }
        public async Task<bool> SetupConnection()
        {
            NameValueCollection header = new NameValueCollection()
            {
                { "Authorization" , string.Format("Bearer {0}", ImgurConnection.ImgurConnectToken) }
            };
            NameValueCollection post = new NameValueCollection()
            {
                { "grant_type", "refresh_token" },
                { "client_id", ImgurConnection.ClientID },
                { "client_secret",  ImgurConnection.ImgurSecret},
                { "refresh_token", ImgurConnection.ImgurRefreshToken }
            };
            PostQuery postReq = new PostQuery("https://api.imgur.com/oauth2/token", header, post);

            HttpWebResponse response = await WebConnection.MakeRequest(postReq);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            else
            {
                string json;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    json = await reader.ReadToEndAsync();
                }
                RefreshData refreshData = JsonConvert.DeserializeObject<RefreshData>(json);
                ImgurConnection.ImgurConnectToken = refreshData.AccessToken;
                ImgurConnection.ImgurRefreshToken = refreshData.RefreshToken;
                return true;
            }
        }
        public async Task<string> SaveConfig(FileData ConfigFile)
        {
            return await ConfigFile.WriteObject(ImgurConnection);
        }
    }
}
