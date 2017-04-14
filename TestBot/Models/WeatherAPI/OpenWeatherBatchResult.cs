using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherBatchResult
    {
        [JsonProperty(PropertyName = "cod")]
        public string Cod { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "cnt")]
        public string Count { get; set; }
        [JsonProperty(PropertyName = "list")]
        public IEnumerable<OpenWeatherResult> OpenWeatherResult { get; set; }
    }
}