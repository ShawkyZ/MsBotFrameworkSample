using System.Collections.Generic;

namespace TestBot.Models.WeatherAPI
{
    public class OpenWeatherWeatherStateBatchResult
    {
        public IEnumerable<OpenWeatherWeatherStateResult> OpenWeatherWeatherStateResultList { get; set; }
    }
}