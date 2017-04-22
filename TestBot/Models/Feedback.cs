using System;
using Microsoft.Bot.Builder.FormFlow;

namespace TestBot.Models
{
    [Serializable]
    public class Feedback
    {
        [Numeric(1, 5)]
        [Describe("Rate your experience with us from 1-5")]
        public int StarsNumber { get; set; }
        [Optional]
        [Describe("Wanna send us a message?")]
        public string AdditionalMessage { get; set; }
        public static IForm<Feedback> BuildForm()
        {
            return new FormBuilder<Feedback>()
                .Message("Write your feedback")
                .Build();
        }
    }
}