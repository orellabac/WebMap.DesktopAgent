using Mobilize;
using Newtonsoft.Json.Linq;
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
            switch (Action)
            {
                case "Greetings": return new JObject(new JProperty("Data", "Hello World")).ToString();
                case "GreetingsWrong": throw new System.Exception();
                default:
                    throw new InvalidPluginAction(Action);
            }
            
        }
    }
}
