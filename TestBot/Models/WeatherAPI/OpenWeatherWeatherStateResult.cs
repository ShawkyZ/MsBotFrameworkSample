using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherWeatherStateResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "main")]
        public string MainState { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
    }
}