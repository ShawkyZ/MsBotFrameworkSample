using Newtonsoft.Json;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherMainResult
    {
        [JsonProperty(PropertyName = "temp")]
        public double Temprature { get; set; }
        [JsonProperty(PropertyName = "temp_min")]
        public double MinimumTemprature { get; set; }
        [JsonProperty(PropertyName = "temp_max")]
        public double MaximumTemprature { get; set; }
        [JsonProperty(PropertyName = "pressure")]
        public double Pressure { get; set; }
        [JsonProperty(PropertyName = "sea_level")]
        public double SeaLevel { get; set; }
        [JsonProperty(PropertyName = "grnd_level")]
        public double GroundLevel { get; set; }
        [JsonProperty(PropertyName = "humidity")]
        public double Humidity { get; set; }
        [JsonProperty(PropertyName = "temp_kf")]
        public double TempratureKf { get; set; }
    }
}