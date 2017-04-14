using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using TestBot.Helpers;

namespace TestBot.Dialogs
{
    [LuisModel("a2b9a642-5c7e-49a1-bfc2-b4cb07cb4fbe", "d51d2236e9a047b1bf07b83050b285b4")]
    [Serializable]
    public class WeatherDialog:LuisDialog<object>
    {
        EntityRecommendation date;

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.Forward(new SentimentDialog(),Resume, result, CancellationToken.None);
            context.Wait(MessageReceived);
        }

        public async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
        }

        [LuisIntent("GetWeatherData")]
        public async Task GetWeatherData(IDialogContext context, LuisResult result)
        {
            EntityRecommendation location;
            if (!result.TryFindEntity("Location", out location) && !result.TryFindEntity("builtin.geography.city", out location))
            {
                PromptDialog.Text(context,
                                    SelectCity, 
                                    "In which city do you want to know the weather forecast?");
                
            }
            if (!result.TryFindEntity("builtin.datetime.date", out date) && !result.TryFindEntity("$datetime", out date))
            {
                date = new EntityRecommendation {Type = "builtin.datetime.date", Resolution = new Dictionary<string, string> { { "date", DateTime.Now.ToShortDateString().Replace(".","-") } } };
            }
            if(location != null)
            await GetWeatherForecast(context, location, date, result);
        }

        private async Task GetWeatherForecast(IDialogContext context, EntityRecommendation location, EntityRecommendation date, LuisResult result = null)
        {
            if (location == null || date == null)
            {
                await context.PostAsync("Error!");
                return;
            }
            var luisDateString = BotFrameworkHelper.ParseLuisDateString(date.Resolution.First().Value);
            if (luisDateString == null) return;
            var weatherResult = OpenWeatherApiHelper.GetWeatherBatchResult(location.Entity, 0)
                .OpenWeatherResult.FirstOrDefault(x => DateTime.Parse(x.DateText).Month == luisDateString.Value.Month && DateTime.Parse(x.DateText).Day == luisDateString.Value.Day);
            if (weatherResult == null)
            {
                await context.PostAsync("Sorry, I can't retrieve weather data for more than 5 days from now.!");
                return;
            }

            if(result != null && result.TryFindEntity("WeatherState::Rainy",out EntityRecommendation state) && weatherResult.OpenWeatherWeatherStateResultList.First().MainState.ToLower().Contains("rain"))
                await context.PostAsync($"The weather in {location.Entity} on {luisDateString.Value.ToShortDateString()} is {weatherResult.OpenWeatherMainResult.Temprature}C and {weatherResult.OpenWeatherWeatherStateResultList.First().MainState} and Yes you should get a {state.Entity}");
            else
                await context.PostAsync($"The weather in {location.Entity} on {luisDateString.Value.ToShortDateString()} is {weatherResult.OpenWeatherMainResult.Temprature}C and {weatherResult.OpenWeatherWeatherStateResultList.First().MainState}");

            context.Wait(MessageReceived);
        }
        
        private async Task SelectCity(IDialogContext context, IAwaitable<string> result)
        {
            var location = await result;
            var entityLocation = new EntityRecommendation { Type = "builtin.geography.city", Entity = location };
            await GetWeatherForecast(context, entityLocation, date);
        }
    }
}