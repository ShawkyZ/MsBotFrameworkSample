using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TestBot.Helpers;

namespace TestBot.Dialogs
{
    public class QuoteDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context, new AwaitableFromItem<object>(context.Activity));
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync(QuotesHelper.GetQuote(activity?.Text));
            context.Wait(MessageReceivedAsync);
        }
    }
}