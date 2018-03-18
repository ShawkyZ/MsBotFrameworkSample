using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace TestBot
{
    public class CancelScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;
        public CancelScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            if (activity is IMessageActivity message && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals("cancel", StringComparison.InvariantCultureIgnoreCase))
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
            this.task.Reset();
        }
    }
}