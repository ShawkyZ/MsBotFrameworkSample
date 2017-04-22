using System;
using Microsoft.Bot.Builder.FormFlow;

namespace TestBot.Models
{
    public enum State
    {
        Happy = 1,
        Sad
    }
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Sorry, I didn't get {0}.", "I didn't understand {0}")]
    [Template(TemplateUsage.NoPreference, "No Preference", "No Preference")]
    [Template(TemplateUsage.Integer, "Choose a number from 1 to 5", "Choose a number from 1 to 5")]
    public class Feedback
    {
        [Numeric(1, 5)]
        [Prompt("Rate your experience with us from 1-5")]
        public int Rating { get; set; }
        [Optional]
        [Prompt("Choose a {&}. {||}")]
        public State? State { get; set; }
        [Prompt("Tell us why.")]
        public string AdditionalMessage { get; set; }
        public static IForm<Feedback> BuildForm()
        {
            return new FormBuilder<Feedback>()
                .Message("Write your feedback")
                .Build();
        }
    }
}