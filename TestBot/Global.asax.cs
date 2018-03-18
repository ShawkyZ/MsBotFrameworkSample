using System.Web;
using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace TestBot
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RegisterGlobalMessageHandlers();
            GlobalConfiguration.Configure(WebApiConfig.Register);

        }
        private void RegisterGlobalMessageHandlers()
        {
            Conversation.UpdateContainer(builder =>
            {
                builder.RegisterModule(new ReflectionSurrogateModule());
                builder.RegisterModule<GlobalMessageHandlersBotModule>();
            });
        }
    }
}
