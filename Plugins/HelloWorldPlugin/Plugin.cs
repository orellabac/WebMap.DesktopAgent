using Mobilize;
using System.ComponentModel.Composition;

namespace HelloWorldPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        public string ImgSource
        {
            get
            {
                return null;
            }
        }

        public string Name
        {
            get
            {
                return "HelloWorld";
            }
        }

        public string InvokeAction(IPlugin Plugin, string Action, string ActionParams)
        {
            return "HelloWorld";
        }
    }
}
