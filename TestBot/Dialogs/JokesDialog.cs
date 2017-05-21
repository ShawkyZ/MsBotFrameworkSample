using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using TestBot.Helpers;

namespace TestBot.Dialogs
{
    public class JokesDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context, new AwaitableFromItem<object>(context.Activity));
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(await JokesHelper.GetJoke());
            context.Wait(MessageReceivedAsync);
        }
    }
}