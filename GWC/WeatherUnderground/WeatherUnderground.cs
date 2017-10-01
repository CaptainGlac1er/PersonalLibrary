using GWC.FileSystem;
using GWC.WeatherUnderground.DataTypes;
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

namespace GWC.WeatherUnderground
{
    public class WeatherUnderground
    {
        private readonly string WEATHERUNDERGROUNDENDPOINT = "http://api.wunderground.com/api/";
        private WeatherToken WeatherUndergroundConnection;
        private WebConnection WebConnection;
        public enum QueryType
        {
            satellite,
            conditions
        }
        public WeatherUnderground(WebConnection webconnection)
        {
            WebConnection = webconnection;
        }
        public async Task<bool> LoadConfig(FileData configFile)
        {
            try
            {
                WeatherUndergroundConnection = await configFile.GetObjectFromJson<WeatherToken>();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<WeatherUndergroundResponse> QuerySearch(QueryType type, string query)
        {
            string url = string.Format("{0}{1}/{2}/q/{3}.json", WEATHERUNDERGROUNDENDPOINT, WeatherUndergroundConnection.Token, type.ToString(), query);
            HttpWebResponse request = await WebConnection.MakeRequest(new GetQuery(url, new NameValueCollection()));
            if (request.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                string weatherJson;
                using (StreamReader reader = new StreamReader(request.GetResponseStream()))
                {
                    weatherJson = await reader.ReadToEndAsync();
                }
                return JsonConvert.DeserializeObject<WeatherUndergroundResponse>(weatherJson);
            }
        }
        public async Task<string> SaveConfig(FileData ConfigFile)
        {
            return await ConfigFile.WriteObject(WeatherUndergroundConnection);
        }
    }
}
