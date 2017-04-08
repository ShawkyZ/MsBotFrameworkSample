using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using TestBot.Models;

namespace TestBot
{
    public static class TextAnalyzeHelper
    {
        public static async Task<SentimentResponse> Analyze(string text)
        {
            const string endpointUrl = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
            const string apiKey = "f79861a34f1f4f488529a45e40bcdfb2";
            var client = new HttpClient
            {
                DefaultRequestHeaders = {
                    {"Ocp-Apim-Subscription-Key", apiKey},
                    {"Accept","application/json" }
                }
            };
            var sentimentRequest = new SentimentRequest
            {
                Documents = new List<DocumentRequest>
                {
                    new DocumentRequest {Id = 1, Text = text}
                }
            };
            var serializedObject = JsonConvert.SerializeObject(sentimentRequest);
            var postRequest = await client.PostAsync(endpointUrl, new StringContent(serializedObject, Encoding.UTF8,"application/json"));
            return JsonConvert.DeserializeObject<SentimentResponse>(await postRequest.Content.ReadAsStringAsync());
        }
    }
}