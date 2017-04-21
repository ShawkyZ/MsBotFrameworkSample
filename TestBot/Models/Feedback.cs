using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace TestBot.Models
{
    public enum State
    {
        Happy,
        Sad
    }
    [Serializable]
    public class Feedback
    {
        public State state { get; set; }
        public string AdditionalMessage { get; set; }
        public static IForm<Feedback> BuildForm()
        {
            return new FormBuilder<Feedback>()
                .Message("Write your feedback")
                .Build();
        }
    }
}