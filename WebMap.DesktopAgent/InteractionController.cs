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
using System.Diagnostics;

namespace Mobilize
{

    public class InteractionController : ApiController
    {

        public DesktopAgentInteractionResponse Post(
            [ValueProvider(typeof(ApplicationTextValueProviderFactory))]
               DesktopAgentInteractionRequest interactionOB) 
        {
            DesktopAgentInteractionResponse response;
            var st = new Stopwatch();
            st.Start();
            Trace.TraceInformation("Interaction Message Start. Plugin {0} ActionParams  ", interactionOB.PluginName, interactionOB.ActionParams);

            IPlugin plugin = null;
            if (DesktopAgent.Plugins.TryGetValue(interactionOB.PluginName,out plugin))
            {
                var actionParams = Newtonsoft.Json.Linq.JObject.Load(new Newtonsoft.Json.JsonTextReader(new System.IO.StringReader(interactionOB.ActionParams.ToString())));
                try {
                    var result = plugin.InvokeAction(plugin, interactionOB.Action, interactionOB.ActionParams);
                    response = new DesktopAgentInteractionResponse()
                    {
                        Action = interactionOB.Action,
                        PluginName = interactionOB.PluginName,
                        Status = result
                    };
                }
                catch(Exception ex)
                {
                    response = new DesktopAgentInteractionResponse()
                    {
                        Action = interactionOB.Action,
                        PluginName = interactionOB.PluginName,
                        Status = "pluginerror",
                        Info = ex.Message
                    };

                }
            }
            else
            {
                Trace.TraceError("Plugin not found {0}", interactionOB.PluginName);
                response = new DesktopAgentInteractionResponse()
                {
                    Action = interactionOB.Action,
                    PluginName = interactionOB.PluginName,
                    Status = "notfound"
                };
            }
            st.Stop();
            Trace.TraceInformation("Interaction Message End. Elapsed {0}",st.ElapsedMilliseconds);
            return response;
        }

     
    }
}
