using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Mobilize;
using System.Configuration;
using System.Web.Http.ValueProviders;

namespace Mobilize
{

    public class InteractionController : ApiController
    {

        public DesktopAgentInteractionResponse Post(
            [ValueProvider(typeof(ApplicationTextValueProviderFactory))]
               DesktopAgentInteractionRequest interactionOB) 
        {

            IPlugin plugin = null;
            if (DesktopAgent.Plugins.TryGetValue(interactionOB.PluginName,out plugin))
            {
                var actionParams = Newtonsoft.Json.Linq.JObject.Load(new Newtonsoft.Json.JsonTextReader(new System.IO.StringReader(interactionOB.ActionParams.ToString()))); 
                var result = plugin.InvokeAction(plugin, interactionOB.Action, interactionOB.ActionParams);
                return new DesktopAgentInteractionResponse()
                {
                    Action = interactionOB.Action,
                    PluginName = interactionOB.PluginName,
                    Status = result
                };
            }
            else
            {
                return new DesktopAgentInteractionResponse()
                {
                    Action = interactionOB.Action,
                    PluginName = interactionOB.PluginName,
                    Status = "notfound"
                };
            }
            
        }

     
    }
}
