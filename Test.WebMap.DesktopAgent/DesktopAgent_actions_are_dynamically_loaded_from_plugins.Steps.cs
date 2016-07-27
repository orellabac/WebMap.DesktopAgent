using System;
using LightBDD;
using NUnit.Framework;

namespace Test.WebMap.DesktopAgent
{
    public partial class DesktopAgent_actions_are_dynamically_loaded_from_plugins : FeatureFixture
    {
        private void Given_a_request_for_Plugin_HelloWorldPluginNotFound_Action_CustomAction() { }


        private void When_plugin_is_not_installed()
        {
            
        }

        private void Then_response_should_be_Status_error_Info_plugin_not_found()
        {
        }


    }
}
