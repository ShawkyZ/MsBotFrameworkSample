using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherResult
    {
        [JsonProperty(PropertyName = "dt_txt")]
        public string DateText { get; set; }
        [JsonProperty(PropertyName = "main")]
        public OpenWeatherMainResult OpenWeatherMainResult { get; set; }
        [JsonProperty(PropertyName = "weather")]
        public IEnumerable<OpenWeatherWeatherStateResult> OpenWeatherWeatherStateResultList { get; set; }
        [JsonProperty(PropertyName = "clouds")]
        public OpenWeatherCloudStateResult OpenWeatherCloudStateResult { get; set; }
        [JsonProperty(PropertyName = "wind")]
        public OpenWeatherWindStateResult OpenWeatherWindStateResult { get; set; }
        [JsonProperty(PropertyName = "rain")]
        public OpenWeatherRainStateResult OpenWeatherRainStateResult { get; set; }
    }
}