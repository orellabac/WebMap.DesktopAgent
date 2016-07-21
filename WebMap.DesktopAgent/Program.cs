using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
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
    static class DesktopAgent
    {


        const string DEFAULT_SEARCH_PATH = "..\\..\\..\\Plugins";

        public static IDictionary<string,IPlugin> Plugins
        {
            get
            {
                if (_Plugins == null)
                {
                    LoadPlugins();
                }
                return _Plugins;
            }
        }
        private static Dictionary<string, IPlugin> _Plugins;
        private static void LoadPlugins()
        {

            ///First determine the path where we will look for dlls that can implement a Desktop Agent plugin
            var searchPath = ConfigurationManager.AppSettings["PLUGIN_SEARCH_PATH"];
            searchPath = searchPath ?? DEFAULT_SEARCH_PATH;

            PluginsLoader<IPlugin> loader = new PluginsLoader<IPlugin>(searchPath);
            //Each plugin will then be registered
            _Plugins = new Dictionary<string, IPlugin>();
            IEnumerable<IPlugin> plugins = loader.Plugins;
            foreach (var item in plugins)
            {
                _Plugins.Add(item.Name, item);
            }
        }


        const string default_AGENT_PORT = "60064";

        internal static string agent_listening_port;


        /// <summary>
        /// Main Entry for Desktop Agent
        /// </summary>
        [STAThread]
        static void Main()
        {

            agent_listening_port = ConfigurationManager.AppSettings["AGENT_PORT"];

            agent_listening_port = agent_listening_port ?? default_AGENT_PORT;


            var _baseAddress = new Uri(string.Format("http://localhost:{0}/", agent_listening_port));
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
