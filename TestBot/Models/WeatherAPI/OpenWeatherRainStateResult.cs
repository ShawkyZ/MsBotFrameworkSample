using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherRainStateResult
    {
        [JsonProperty(PropertyName = "3h")]
        public double ThreeHoursState { get; set; }
    }
}