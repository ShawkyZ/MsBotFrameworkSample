using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TestBot.Models
{
    public class SentimentResponse
    {
        [JsonProperty(PropertyName = "documents")]
        public IEnumerable<DocumentResponse> Documents { get; set; }
    }
}