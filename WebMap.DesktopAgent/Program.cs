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
    static class ProgramDesktopAgent
    {



        /// <summary>
        /// Main Entry for Desktop Agent
        /// </summary>
        [STAThread]
        static void Main()
        {

            DesktopAgent.agent_listening_port = ConfigurationManager.AppSettings["AGENT_PORT"];

            DesktopAgent.agent_listening_port = DesktopAgent.agent_listening_port ?? DesktopAgent.default_AGENT_PORT;


            var _baseAddress = new Uri(string.Format("http://localhost:{0}/", DesktopAgent.agent_listening_port));
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
           
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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run( new DesktopAgentWindow());
            }
        }
    }
}
