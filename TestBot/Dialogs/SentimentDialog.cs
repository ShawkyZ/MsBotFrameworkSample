using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TestBot.Helpers;

namespace TestBot.Dialogs
{
    public class SentimentDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context, new AwaitableFromItem<object>(context.Activity));
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            
            var activity = await result as Activity;
            var sentimentResponse = await TextAnalyzeHelper.Analyze(activity?.Text);
            var replyMessage = CreateReplyMessage(sentimentResponse.Documents.First().Score);
            var replyActivity = activity?.CreateReply($"{replyMessage}");
            await context.PostAsync(replyActivity);
            context.Wait(MessageReceivedAsync);
        }
        private string CreateReplyMessage(double sentimentScore)
        {
            if (sentimentScore > 0.8)
                return "Glad to serve!";
            return sentimentScore < 0.2 ? "We're sorry" : "Alright";
        }
    }
}