using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherWindStateResult
    {
        [JsonProperty(PropertyName = "speed")]
        public double Speed { get; set; }
        [JsonProperty(PropertyName = "deg")]
        public double Degree { get; set; }
    }
}