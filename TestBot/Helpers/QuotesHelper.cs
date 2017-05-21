using System.Net;

namespace TestBot.Helpers
{
    public class QuotesHelper
    {
        public static string GetQuote(string messageText)
        {
            var wb = new WebClient();
            var response = wb.DownloadString("http://www.all-famous-quotes.com/quotes_generator.html");
            var rawResponse = response.Substring(response.IndexOf("<blockquote class=\"new\">"));
            return rawResponse.Remove(rawResponse.IndexOf("- <a href=\"http://www.all-famous-quotes.com")).Replace("<blockquote class=\"new\">", "");
        }
    }
}