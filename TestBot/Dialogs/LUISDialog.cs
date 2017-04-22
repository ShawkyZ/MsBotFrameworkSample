using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using TestBot.Helpers;
using TestBot.Models;

namespace TestBot.Dialogs
{
    [LuisModel("a2b9a642-5c7e-49a1-bfc2-b4cb07cb4fbe", "d51d2236e9a047b1bf07b83050b285b4")]
    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class LUISDialog:LuisDialog<object>
    {
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
            else
            {
                context.UserData.SetValue("Location", location.Entity);
            }
            EntityRecommendation date;
            if (!result.TryFindEntity("builtin.datetime.date", out date) && !result.TryFindEntity("$datetime", out date))
            {
                date = new EntityRecommendation {Type = "builtin.datetime.date", Resolution = new Dictionary<string, string> { { "date", DateTime.Now.ToShortDateString().Replace(".","-") } } };
            }
            context.UserData.SetValue("Date", date.Resolution.FirstOrDefault().Value);
            if (location != null)
            await GetWeatherForecast(context, result);
        }
        [LuisIntent("GetFeedback")]
        public async Task GetFeedback(IDialogContext context, LuisResult result)
        {
            context.Call(new FormDialog<Feedback>(new Feedback(), Feedback.BuildForm,
                FormOptions.PromptInStart), Resume);
        }

        private async Task Resume(IDialogContext context, IAwaitable<Feedback> result)
        {
            await context.PostAsync("Thank you.");
        }

        private async Task GetWeatherForecast(IDialogContext context, LuisResult result = null)
        {
            context.UserData.TryGetValue("Date", out string date);
            context.UserData.TryGetValue("Location", out string location);
            var luisDateString = BotFrameworkHelper.ParseLuisDateString(date);
            if (luisDateString == null) return;
            var weatherResult = OpenWeatherApiHelper.GetWeatherBatchResult(location, 0)
                .OpenWeatherResult.FirstOrDefault(x => DateTime.Parse(x.DateText).Month == luisDateString.Value.Month && DateTime.Parse(x.DateText).Day == luisDateString.Value.Day);
            if (weatherResult == null)
            {
                await context.PostAsync("Sorry, I can't retrieve weather data for more than 5 days from now.!");
                return;
            }
            var replyMessage = string.Empty;
            var hasRainEntity = result.TryFindEntity("WeatherState::Rainy", out EntityRecommendation weatherStateEntity);
            var isRainy = weatherResult.OpenWeatherWeatherStateResultList.First().MainState.ToLower().Contains("rain");
            if (hasRainEntity && isRainy)
                replyMessage += $"Yes, you should get a {weatherStateEntity.Entity} ";
            else if(hasRainEntity)
                replyMessage += $"No, you shouldn't get a {weatherStateEntity.Entity} ";
            replyMessage += $"The weather in {location} on {luisDateString.Value.ToShortDateString()} is {weatherResult.OpenWeatherMainResult.Temprature}C and {weatherResult.OpenWeatherWeatherStateResultList.First().MainState}";
            await context.PostAsync(replyMessage);
            context.Wait(MessageReceived);
        }
        
        private async Task SelectCity(IDialogContext context, IAwaitable<string> result)
        {
            var location = await result;
            context.UserData.SetValue("Location", location);
            await GetWeatherForecast(context);
        }
    }
}