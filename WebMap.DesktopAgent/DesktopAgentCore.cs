using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows.Forms;

namespace Mobilize
{
    /// <summary>
    /// The Mobilize Desktop Agent provides a mechanism so WebMap Applications
    /// can interact with Desktop Applications and Devices
    /// It installs itself on the system tray. 
    /// And it starts up a Web API Selfhost service (by default at 60064)
    /// This Web API can then be called from Web Pages to allow access to the devices.
    /// </summary>
    public static class DesktopAgentCore
    {
        /// <summary>
        /// Main Entry for Desktop Agent
        /// </summary>
        public static void ListenerStart()
        {
            try
            {
                DesktopAgent.agent_listening_Host= ConfigurationManager.AppSettings["AGENT_HOST"];
                DesktopAgent.agent_listening_Port = ConfigurationManager.AppSettings["AGENT_PORT"];
                    

                var _baseUrl = new Uri(string.Format("http://"+ DesktopAgent.agent_listening_Host+ ":{0}/", DesktopAgent.agent_listening_Port));
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseUrl);

                config.MessageHandlers.Add(new CORSHeaderHandler());
                config.Routes.MapHttpRoute(
                    name: "DesktopInterationDefaultRouteApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                config.Services.Add(typeof(System.Web.Http.ValueProviders.ValueProviderFactory), new ApplicationTextValueProviderFactory());

                // Create server
                using (var server = new HttpSelfHostServer(config))
                {
                    // Start listening
                    server.OpenAsync().Wait();
                    StartGUI();
                }
            }
            catch (Exception ex)
            {
                ErrorLog Err = new ErrorLog();
                Err.generateErrorLog("Error in Agent Configuration: " + ex.Message, ex.StackTrace);
                MessageBox.Show("Error in Agent Configuration: " + ex.Message);
            }

        }


        public static Action StartGUI;
        
    }
}
