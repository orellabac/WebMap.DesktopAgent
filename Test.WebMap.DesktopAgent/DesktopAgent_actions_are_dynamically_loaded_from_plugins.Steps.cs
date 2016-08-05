using LightBDD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Mobilize.DesktopAgentTest.Features
{
    public partial class DesktopAgent_actions_are_dynamically_loaded_from_plugins : FeatureFixture
    {

        DesktopAgentInteractionRequest _request;

        private void Given_a_request_for_Plugin_HelloWorldPluginNotFound_Action_CustomAction()
        {
            _request = new Mobilize.DesktopAgentInteractionRequest() { PluginName = "HelloWorldPluginNotFound", Action = "HelloWorld", ActionParams = "{'From':'Mau'}" };
        }


        private void Given_a_request_for_Plugin_HelloWorldPlugin_Action_NotExist() {
            _request = new Mobilize.DesktopAgentInteractionRequest() { PluginName = "HelloWorld", Action = "NotExists", ActionParams = "{'From':'Mau'}" };
        }
        private void  Given_a_request_for_Plugin_HelloWorldPlugin_Action_GreetingsWrong() {
            _request = new Mobilize.DesktopAgentInteractionRequest() { PluginName = "HelloWorld", Action = "GreetingsWrong", ActionParams = "{'From':'Mau'}" };
        }
        private void  Given_a_request_for_Plugin_HelloWorldPlugin_Action_Greetings() {
            _request = new Mobilize.DesktopAgentInteractionRequest() { PluginName = "HelloWorld", Action = "Greetings", ActionParams = "{'From':'Mau'}" };
        }



        private void When_plugin_is_installed()
        {
            Assert.IsTrue(DesktopAgent.IsPluginInstalled("HelloWorld"));
        }


        private void When_plugin_is_not_installed()
        {

            Assert.IsFalse(DesktopAgent.IsPluginInstalled("HelloWorldPluginNotFound"));
        }





        private void Then_response_should_be_Status_error_Info_plugin_not_found()
        {
            DesktopAgentInteractionResponse response = DesktopAgent.ProcessRequest(_request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == "error");
            Assert.IsNotNull(response.Info);
            Assert.AreEqual(JObject.Parse(response.Info)["Data"].Value<string>(),"plugin not found");
        }


        private void Then_response_should_be_Status_error_Info_Invalid_Action()
        {
            var response = DesktopAgent.ProcessRequest(_request);
            Assert.AreEqual(response.Status, "error");
            Assert.AreEqual(JObject.Parse(response.Info)["Data"].Value<string>(),"invalid action");
        }


        private void Then_response_should_be_Status_error_Info_Data_Action_Error()
        {
            var response = DesktopAgent.ProcessRequest(_request);
            Assert.AreEqual(response.Status, "error");
            Assert.AreEqual(JObject.Parse(response.Info)["Data"].Value<string>(),"action error");

        }

        private void  Then_response_should_be_Status_OK_Info_Data_Hello_World() {
            var response = DesktopAgent.ProcessRequest(_request);
            Assert.AreEqual(response.Status, "ok");
            Assert.AreEqual(JObject.Parse(response.Info)["Data"].Value<string>(),"Hello World");

        }




    }
}
