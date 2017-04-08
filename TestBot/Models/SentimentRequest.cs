using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TestBot.Models
{
    public class SentimentRequest
    {
        [JsonProperty(PropertyName = "documents")]
        public IEnumerable<DocumentRequest> Documents { get; set; }
    }
}