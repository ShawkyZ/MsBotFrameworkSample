using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using TestBot.Helpers;
using BestMatchDialog;

namespace TestBot.Dialogs
{
    [Serializable]
    public class BasicConversationDialog : BestMatchDialog<object>
    {
        [BestMatch(new[] { "Hi", "Hi There", "Hello there", "Hey", "Hello",
                "Hey there", "Greetings", "Good morning", "Good afternoon", "Good evening", "Good day", "Yo","are you there?","there ?","Hello there" },
            0.5, true, false)]
        public async Task WelcomeGreeting(IDialogContext context, string messageText)
        {
            await context.PostAsync("Hello there. How can I help you?");
            context.Done(true);
        }

        [BestMatch(new[] { "bye", "bye bye", "got to go",
            "see you later", "laters", "adios","remain silent", "shut up" })]
        public async Task FarewellGreeting(IDialogContext context, string messageText)
        {
            await context.PostAsync("Ok, Bye. Have a good day.");
            context.Done(true);
        }

        [BestMatch(new[] { "how goes it", "how do", "hows it going", "how are you",
            "how do you feel", "whats up", "sup", "hows things","how is it going" })]
        public async Task HandleStatusRequest(IDialogContext context, string messageText)
        {
            await context.PostAsync("I am great.");
            context.Wait(MessageReceived);
        }

        [BestMatch(new[] { "who are you", "introduce yourself", "what can you do", "tell me more about you",
            "what can you do for me", "what are your abilities", "your skills","with whom Im talking" })]
        public async Task HandleIntroductionRequest(IDialogContext context, string messageText)
        {
            await context.PostAsync("I'm Dumb! You can ask me for the weather forecast, to tell a joke, a quote or just to remain silent.");
            context.Wait(MessageReceived);
        }

        public override async Task NoMatchHandler(IDialogContext context, string messageText)
        {
            var sentimentResponse = await TextAnalyzeHelper.Analyze(messageText);
            var replyMessage = CreateReplyMessage(sentimentResponse.Documents.First().Score);
            await context.PostAsync(replyMessage);
            context.Done(false);
        }
      

        private string CreateReplyMessage(double sentimentScore)
        {
            if (sentimentScore > 0.8)
                return "Glad to serve!";
            return sentimentScore < 0.2 ? "We're sorry" : "Sorry I didn't get that";
        }
    }
}