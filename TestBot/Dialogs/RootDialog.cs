using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TestBot.Helpers;
using TestBot.Models;
using TestBot.Models.WeatherAPI;

namespace TestBot.Dialogs
{
    [LuisModel("a2b9a642-5c7e-49a1-bfc2-b4cb07cb4fbe", "d51d2236e9a047b1bf07b83050b285b4")]
    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class RootDialog:LuisDialog<object>
    {
        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var msg = await message;
            await context.Forward(new BasicConversationDialog(),BaisConversationCallBack, msg, CancellationToken.None);
        }

        public async Task BaisConversationCallBack(IDialogContext context, IAwaitable<object> result)
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
                date = new EntityRecommendation {Type = "builtin.datetime.date", Resolution = new Dictionary<string, object> { { "date", DateTime.Now.ToShortDateString().Replace(".","-") } } };
            }
            context.UserData.SetValue("Date", date.Resolution.FirstOrDefault().Value);
            if (location != null)
            await GetWeatherForecast(context, result);
        }
        [LuisIntent("GetQuote")]
        public async Task GetQuote(IDialogContext context,IAwaitable<IMessageActivity> message, LuisResult result)
        {
            await context.Forward(new QuoteDialog(), QuoteCallBack, await message, CancellationToken.None);
        }

        private async Task QuoteCallBack(IDialogContext context, IAwaitable<object> result)
        {
            
        }
        [LuisIntent("GetJoke")]
        public async Task GetJoke(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            await context.Forward(new JokesDialog(), JokeCallBack, await message, CancellationToken.None);
        }

        private async Task JokeCallBack(IDialogContext context, IAwaitable<object> result)
        {

        }
        [LuisIntent("GetFeedback")]
        public async Task GetFeedback(IDialogContext context, LuisResult result)
        {
            context.Call(new FormDialog<Feedback>(new Feedback(), Feedback.BuildForm,
                FormOptions.PromptInStart), Resume);
        }
        [LuisIntent("TurnLightsOnOff")]
        public async Task TurnLightsOnOff(IDialogContext context, LuisResult result)
        {
            if (result.TryFindEntity("LedColor", out EntityRecommendation ledColor) &&
                result.TryFindEntity("LedState", out EntityRecommendation ledState))
            {
                await context.PostAsync($"{ledColor.Entity};{ledState.Entity}");
            }
            context.Wait(MessageReceived);
        }

        private async Task Resume(IDialogContext context, IAwaitable<Feedback> result)
        {
            await context.PostAsync("Thank you.");
        }

        private async Task GetWeatherForecast(IDialogContext context, LuisResult result = null)
        {
            try
            {
                context.UserData.TryGetValue("Date", out string date);
                context.UserData.TryGetValue("Location", out string location);
                DateTime? luisDateString;
                OpenWeatherBatchResult weatherBatchResult;
                try
                {
                    luisDateString = BotFrameworkHelper.ParseLuisDateString(date);
                    weatherBatchResult = OpenWeatherApiHelper.GetWeatherBatchResult(location, 0);
                }
                catch
                {
                    await context.PostAsync($"Couldn't get weather forecast for {location}.");
                    return;
                }
                var weatherResult = weatherBatchResult
                    .OpenWeatherResult.FirstOrDefault(
                        x => DateTime.Parse(x.DateText).Month == luisDateString.Value.Month &&
                             DateTime.Parse(x.DateText).Day == luisDateString.Value.Day);
                if (weatherResult == null)
                {
                    await context.PostAsync("Sorry, I can't retrieve weather data for more than 5 days from now.!");
                    context.Wait(MessageReceived);
                    return;
                }
                var replyMessage = string.Empty;
                if (result != null)
                {
                    var hasRainEntity = result.TryFindEntity("WeatherState::Rainy",
                        out EntityRecommendation rainyWeatherStateEntity);
                    var hasWarmEntity = result.TryFindEntity("WeatherState::Warm",
                        out EntityRecommendation warmWeatherStateEntity);
                    var hasColdEntity = result.TryFindEntity("WeatherState::Cold",
                        out EntityRecommendation coldWeatherStateEntity);
                    var isWarm = weatherResult.OpenWeatherMainResult.Temprature > 25;
                    var isCold = weatherResult.OpenWeatherMainResult.Temprature <= 18;
                    var isRainy = weatherResult.OpenWeatherWeatherStateResultList.First()
                        .MainState.ToLower()
                        .Contains("rain");
                    if (hasRainEntity && isRainy)
                        replyMessage += $"Yes, it's raining and you should get a {rainyWeatherStateEntity.Entity} ";
                   else if(hasColdEntity && isCold)
                        replyMessage += $"Yes, it's cold and you should wear a {coldWeatherStateEntity.Entity} ";
                   else if (hasWarmEntity && isWarm)
                        replyMessage += $"Yes, you should get a {warmWeatherStateEntity.Entity} ";
                   else if(hasRainEntity || hasWarmEntity || hasColdEntity)
                        replyMessage += "No, you shouldn't ";

                }
                replyMessage +=
                    $"The weather in {location} on {luisDateString.Value.ToShortDateString()} is {weatherResult.OpenWeatherMainResult.Temprature}C and {weatherResult.OpenWeatherWeatherStateResultList.First().MainState}";
                await context.PostAsync(replyMessage);
            }
            catch (Exception ex)
            {
                await context.PostAsync(ex.StackTrace);
            }
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
