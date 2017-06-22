using GWC.FileSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWC.WeatherUnderground.DataTypes
{
    class WeatherToken : IFileType
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
