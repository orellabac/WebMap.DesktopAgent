using LightBDD;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace Test.WebMap.DesktopAgent.BrowserChrome
{

    public partial class Browser_makes_calls_to_actions_implemented_on_Plugins :  FeatureFixture
    {
        IWebDriver driver;

        [SetUp]
        public void Initialize()
        {
            Process.Start(@"..\..\..\WebMap.DesktopAgent\bin\Debug\WebMap.DesktopAgent.exe");
            driver = new ChromeDriver();
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
            foreach (var process in Process.GetProcessesByName("WebMap.DesktopAgent"))
            {
                process.Kill();
            }
        }

        private void Given_a_sample_page_loaded_on_browser() {
            var fullPath = System.IO.Path.GetFullPath("./SampleHTML.html");
            var uri = new System.Uri(fullPath);
            driver.Url = uri.AbsoluteUri;
        }
        private void Then_Response_should_be_Status_ok_Info_Data_Hello_World() { }
        private void When_and_ajax_request_for_plugin_HelloWorldPLugin_and_Action_Greetings_Is_Performed()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var script = "$.ajax({url:'{0}',method:'POST',data:{1}}).success(function(data) { window.result = data});";
            script = string.Format(script, "localhost:60069", "{}");
            js.ExecuteScript(script);
            System.Threading.Thread.Sleep(200);
            string result = (string)js.ExecuteScript("return window.result");
            Assert.NotNull(result);
            var obj = JObject.Parse(result);
            Assert.NotNull(obj);
        }


    }
}
