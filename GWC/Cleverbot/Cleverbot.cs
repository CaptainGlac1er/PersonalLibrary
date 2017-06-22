using GWC.Cleverbot.DataTypes;
using GWC.FileSystem;
using GWC.WebConnect;
using GWC.WebConnect.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWC.Cleverbot
{
    public class Cleverbot
    {
        private readonly string CLEVERBOTENDPOINT = "http://www.cleverbot.com";
        private CleverbotToken CleverbotConnection;
        private string Conversation;
        private WebConnection WebConnection;
        public Cleverbot(WebConnection webconnection)
        {
            WebConnection = webconnection;
            Conversation = "";
        }
        public async Task<bool> LoadConfig(FileData configFile)
        {
            try
            {
                CleverbotConnection = await configFile.GetObjectFromJson<CleverbotToken>();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        private async Task<CleverbotReply> GetReplyObject(string input)
        {
            HttpWebResponse Reply = await WebConnection.MakeRequest(new GetQuery($"{CLEVERBOTENDPOINT}/getreply?key={CleverbotConnection.Token}&input={input}&cs={Conversation ?? ""}", null));
            string jsonReply;
            using (StreamReader reader = new StreamReader(Reply.GetResponseStream()))
            {
                jsonReply = await reader.ReadToEndAsync();
            }
            CleverbotReply output = JsonConvert.DeserializeObject<CleverbotReply>(jsonReply);
            Conversation = output.cs;
            return output;
        }
        public async Task<string> GetReply(string input)
        {
            CleverbotReply reply = await GetReplyObject(input);
            if (reply.clever_accuracy > 90)
                return reply.clever_output;
            else
                return reply.output;
        }
    }
}
