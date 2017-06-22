using GWC.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWC.Imgur.DataTypes
{
    class ImgurInfo : IFileType
    {
        public ImgurInfo(string token, string refresh, string client, string secret)
        {
            ImgurConnectToken = token;
            ImgurRefreshToken = refresh;
            ClientID = client;
            ImgurSecret = secret;
        }
        public string ImgurConnectToken { get; set; }
        public string ImgurRefreshToken { get; set; }
        public string ClientID { get; set; }
        public string ImgurSecret { get; set; }
        public override string ToString()
        {
            return "connect: " + ImgurConnectToken + " refresh: " + ImgurRefreshToken;
        }
    }
}
