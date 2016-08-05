using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace Mobilize
{
    /// <summary>
    /// This is the Web API controller that is exposed thru self host, so the web browser can use it to interact with the Desktop devices and or applications
    /// </summary>
    public class InteractionController : ApiController
    {

        public DesktopAgentInteractionResponse Post(
            [ValueProvider(typeof(ApplicationTextValueProviderFactory))]
               DesktopAgentInteractionRequest request) 
        {
            return DesktopAgent.ProcessRequest(request);
        }

     
    }
}
