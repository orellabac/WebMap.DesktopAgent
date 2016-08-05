using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Mobilize
{
    /// <summary>
    /// The Mobilize Desktop Agent provides a mechanism so WebMap Applications
    /// can interact with Desktop Applications and Devices
    /// It installs itself on the system tray. 
    /// And it starts up a Web API Selfhost service (by default at 60064)
    /// This Web API can then be called from Web Pages to allow access to the devices.
    /// </summary>
    public static class DesktopAgent
    {

        static string JSON_PLUGINNOTFOUND = JsonConvert.SerializeObject(new { Data = "plugin not found"});

        static string JSON_INVALIDACTION = JsonConvert.SerializeObject(new { Data = "invalid action" });



        const string DEFAULT_SEARCH_PATH = ".\\Plugins";

        public static bool IsPluginInstalled(string pluginName)
        {
            return Plugins.Any(x => x.Key == pluginName);
        }

        public static IDictionary<string, IPlugin> Plugins
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

        public static DesktopAgentInteractionResponse ProcessRequest(DesktopAgentInteractionRequest request)
        {

            DesktopAgentInteractionResponse response;
            var st = new Stopwatch();
            st.Start();
            Trace.TraceInformation("Interaction Message Start. Plugin {0} ActionParams  ", request.PluginName, request.ActionParams);

            IPlugin plugin = null;
            if (request.PluginName!=null && DesktopAgent.Plugins.TryGetValue(request.PluginName, out plugin))
            {
                try
                {
                    var result = plugin.InvokeAction(plugin, request.Action, request.ActionParams);
                    response = new DesktopAgentInteractionResponse()
                    {
                        Status = "ok", 
                        Info = result
                    };
                }
                catch(InvalidPluginAction)
                {
                    response = new DesktopAgentInteractionResponse()
                    {
                        Status = "error",
                        Info = JSON_INVALIDACTION
                    };
                }
                catch (Exception ex)
                {
                    response = new DesktopAgentInteractionResponse()
                    {
                        Status = "error",
                        Info = new JObject(new JProperty("Data","action error"),new JProperty("Message",ex.Message)).ToString()
                    };

                }
            }
            else
            {
                Trace.TraceError("Plugin not found {0}", request.PluginName);
                response = new DesktopAgentInteractionResponse()
                {
                    Status = "error",
                    Info = JSON_PLUGINNOTFOUND
                };
            }
            st.Stop();
            Trace.TraceInformation("Interaction Message End. Elapsed {0}", st.ElapsedMilliseconds);
            return response;
        }

        private static Dictionary<string, IPlugin> _Plugins;
        private static void LoadPlugins()
        {
            var st = new Stopwatch();
            st.Start();
            Trace.TraceInformation("Start loading Plugins");
            ///First determine the path where we will look for dlls that can implement a Desktop Agent plugin
            var searchPath = ConfigurationManager.AppSettings["PLUGIN_SEARCH_PATH"];
            searchPath = searchPath ?? DEFAULT_SEARCH_PATH;

            try
            {
                Trace.TraceInformation("using Path {0}", Path.GetFullPath(searchPath));
            }
            catch
            {
                Trace.TraceInformation("using Path {0}", searchPath);
            }
            PluginsLoader<IPlugin> loader = new PluginsLoader<IPlugin>(searchPath);
            //Each plugin will then be registered
            _Plugins = new Dictionary<string, IPlugin>();
            IEnumerable<IPlugin> plugins = loader.Plugins;
            foreach (var item in plugins)
            {
                Trace.TraceInformation("Adding plugin {0}", item.Name);
                _Plugins.Add(item.Name, item);
            }
            st.Stop();
            Trace.TraceInformation("End loading plugins. Elapse {0}", st.ElapsedMilliseconds);
        }


        internal const string default_AGENT_PORT = "60064";

        internal static string agent_listening_port;


      
    }
}
