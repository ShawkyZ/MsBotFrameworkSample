using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace TestBot
{
    public class HelpScorable : ScorableBase<IActivity, string, double>
    {
        private IDialogTask task;
        public HelpScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            if (activity is IMessageActivity message && !string.IsNullOrEmpty(message.Text))
            {
                if (message.Text.ToLower() == "help")
                {
                    return message.Text;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item is IMessageActivity;
            if (!message)
                return;
            var helpDialog = new HelpDialog();
            var interruptor = helpDialog.Void<object, IMessageActivity>();
            this.task.Call(interruptor, null);
            await this.task.PollAsync(token);
        }
    }

    public class HelpDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("How Can I help you?");
            context.Wait(MessageRecievedAsync);
        }

        public async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done(true);
        }
    }
}