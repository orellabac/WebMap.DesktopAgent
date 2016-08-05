using System;
using LightBDD;
using Microsoft.VisualStudio.TestTools.UnitTesting;


#if MSTEST
    using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
    using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#else
using NUnit.Framework;
#endif

namespace Mobilize.DesktopAgentTest.Features
{

    [FeatureDescription(
    @"In Order to provide extensibility 
    Desktop Agent loads 'plugins from a predetermined directory
    And they are then available for client use"
    )]
    [Label("Story-1")]
    [TestFixture]
    public partial class DesktopAgent_actions_are_dynamically_loaded_from_plugins
    {


        [Test]
        public void The_requested_plugin_does_not_exists_or_could_not_be_loaded()
        {

            Runner.RunScenario(
                Given_a_request_for_Plugin_HelloWorldPluginNotFound_Action_CustomAction,
                When_plugin_is_not_installed,
                Then_response_should_be_Status_error_Info_plugin_not_found
                );
        }


        [Test]
        public void The_requested_plugin_exists_but_the_requested_action_does_not()
        {

            Runner.RunScenario(
                Given_a_request_for_Plugin_HelloWorldPlugin_Action_NotExist,
                When_plugin_is_installed,
                Then_response_should_be_Status_error_Info_Invalid_Action
                );
        }



        [Test]
        public void The_requested_plugin_exists_and_the_requested_action_exists_but_an_error_occurs_when_invoking_action()
        {

            Runner.RunScenario(
                Given_a_request_for_Plugin_HelloWorldPlugin_Action_GreetingsWrong,
                When_plugin_is_installed,
                Then_response_should_be_Status_error_Info_Data_Action_Error
                );
        }



        [Test]
        public void The_requested_plugin_exists_and_the_requested_action_exists_and_action_is_executed_successfully()
        {

            Runner.RunScenario(
                Given_a_request_for_Plugin_HelloWorldPlugin_Action_Greetings,
                When_plugin_is_installed,
                Then_response_should_be_Status_OK_Info_Data_Hello_World
                );
        }


    }
}
