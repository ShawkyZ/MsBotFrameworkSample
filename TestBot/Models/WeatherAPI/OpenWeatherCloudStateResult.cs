using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherCloudStateResult
    {
        [JsonProperty(PropertyName = "all")]
        public int All { get; set; }
    }
}