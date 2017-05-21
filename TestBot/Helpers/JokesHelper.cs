using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace TestBot.Helpers
{
    public class JokesHelper
    {
        public static async Task<string> GetJoke()
        {
            var wb = new WebClient();
            var document = new HtmlDocument();
            var rawhtml = await wb.DownloadStringTaskAsync("http://www.ajokeaday.com/ChisteAlAzar.asp");
            document.LoadHtml(rawhtml);
            var allElementsWithClassFloat =
                document.DocumentNode.SelectNodes("//*[contains(@class,'jd-body jubilat')]").FirstOrDefault();
            if (allElementsWithClassFloat == null) return string.Empty;
            var joke = HttpUtility.HtmlDecode(allElementsWithClassFloat.InnerText.Trim());
            return joke;
        }
    }
}