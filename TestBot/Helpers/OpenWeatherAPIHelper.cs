using System;
using System.Net;
using Newtonsoft.Json;
using TestBot.Models.WeatherAPI;

namespace TestBot.Helpers
{
    public static class OpenWeatherApiHelper
    {
        public static OpenWeatherBatchResult GetWeatherBatchResult(string cityName,int daysCount)
        {
            const string apiKey = "3b4181dcc924d0690f13db3f580f1a8d";
            const string apiUrl = "http://api.openweathermap.org/data/2.5/forecast?";
            var webClient = new WebClient();
            return JsonConvert.DeserializeObject<OpenWeatherBatchResult>(webClient.DownloadString(new Uri($"{apiUrl}q={cityName}&appid={apiKey}&type=like&cnt={daysCount}&units=metric")));
        }
    }
}