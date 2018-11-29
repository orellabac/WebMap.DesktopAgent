using IWshRuntimeLibrary;
using Mobilize;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsScriptHost
{
    [Export(typeof(IPlugin))]
    public class WindowsScriptHost : IPlugin
    {
        public string Name => "WindowsScriptHostPlugin";

        public string ImgSource => null;

        private WshNetwork _network;

        private WshNetwork Network
        {
            get
            {
                if (_network == null)
                {
                    _network = new WshNetwork();
                }
                return _network;
            }
        }


        public string InvokeAction(IPlugin Plugin, string Action, string ActionParams)
        {
            switch (Action)
            {
                case "Network.UserName":
                    return Network.UserName;
                case "Network.UserDomain":
                    return Network.UserDomain;
                case "Network.ComputerName":
                    return Network.ComputerName;
                case "Network.EnumPrinters":
                    var printers = new List<JObject>();
                    foreach(var printer in Network.EnumPrinterConnections())
                    {
                        printers.Add(new JObject(new JProperty("Name", printer)));
                    }
                    return new JArray(printers.ToArray()).ToString();
                //case "FileSystem":
                //    new FileSystemObject()
                default:
                    throw new InvalidPluginAction(Action);
            }
           
        }
    }
}
