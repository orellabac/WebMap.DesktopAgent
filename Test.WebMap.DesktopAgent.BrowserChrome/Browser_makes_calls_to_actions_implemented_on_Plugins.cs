using System;
using LightBDD;



#if MSTEST
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
    using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#else
    using NUnit.Framework;
#endif


namespace Test.WebMap.DesktopAgent.BrowserChrome
{
    public partial class Browser_makes_calls_to_actions_implemented_on_Plugins
    {

        [Test]
        [Ignore("Do not force on build")]
        public void An_Ajax_call_is_performed_to_the_DesktopAgent_for_an_existing_HellowWorld_plugin_and_action()
        {
            Runner.RunScenario(
                    Given_a_sample_page_loaded_on_browser,
                    When_and_ajax_request_for_plugin_HelloWorldPLugin_and_Action_Greetings_Is_Performed,
                    Then_Response_should_be_Status_ok_Info_Data_Hello_World
                ) ;
        }


        [Test]
        [Ignore("Do not force on build")]
        public void Some_Ajax_calls_are_performed_to_the_DesktopAgent_for_an_existing_OfficeApps_plugin_and_action()
        {
            Runner.RunScenario(
                    Given_a_sample_page_loaded_on_browser,
                    When_and_ajax_request_for_plugin_OfficeApps_and_Action_OpenExcel,
                    And_and_ajax_call_for_plugin_OfficeApps_And_Action_SetCell,
                    Then_Response_should_be_Status_ok_and_Info_Is_EmptyJson
                );
        }
    }
}
