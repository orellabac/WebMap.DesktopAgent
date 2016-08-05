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


        private JObject PerformPluginAjaxCall(string host, string port, string pluginName, string Action, string ActionParams)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var script = "$.ajax({{url:'http://{0}:{1}/api/Interaction',type:'POST',data:JSON.stringify({{PluginName:\"{2}\",Action:\"{3}\",ActionParams:\"{4}\"}}),success: function(data) {{ window.result = data; }}}});";
            script = string.Format(script, host, port, pluginName, Action, ActionParams);
            js.ExecuteScript(script);
            System.Threading.Thread.Sleep(3000);
            string result = js.ExecuteScript("return JSON.stringify(window.result)").ToString();
            Assert.NotNull(result);
            var obj = JObject.Parse(result);
            return obj;
        }

        private string excelID = null;

        private void Given_a_sample_page_loaded_on_browser() {
            var fullPath = System.IO.Path.GetFullPath("./SampleHTML.html");
            var uri = new System.Uri(fullPath);
            driver.Url = uri.AbsoluteUri;
        }
        private void Then_Response_should_be_Status_ok_Info_Data_Hello_World() { }
        private void When_and_ajax_request_for_plugin_HelloWorldPLugin_and_Action_Greetings_Is_Performed()
        {
            JObject obj = PerformPluginAjaxCall("localhost", "60064", "HelloWorld", "Greetings","");
            Assert.NotNull(obj);
            Assert.AreEqual(obj["Status"].Value<string>(), "ok");
            Assert.AreEqual(obj["Info"].Value<string>(), "{\"Data\":\"Hello World\"}");
        }



        private void And_and_ajax_call_for_plugin_OfficeApps_And_Action_SetCell() {

            var actionParams = string.Format("{{ExcelID:\'{0}\',row:\'1\',column:\'A\',value:\'Hello from Mobilize\'}}", excelID);
            JObject obj = PerformPluginAjaxCall("localhost", "60064", "OfficeApps", "SetCell", actionParams);
            Assert.NotNull(obj);
            Assert.AreEqual(obj["Status"].Value<string>(), "ok");
            Assert.IsNotEmpty(obj["Info"].Value<string>());

            var infoTxt = obj["Info"].Value<string>();
            var info = JObject.Parse(infoTxt);
            Assert.AreEqual(infoTxt,"{}");

        }
        private void Then_Response_should_be_Status_ok_and_Info_Is_EmptyJson() {


        }
        private void When_and_ajax_request_for_plugin_OfficeApps_and_Action_OpenExcel() {
            JObject obj = PerformPluginAjaxCall("localhost", "60064", "OfficeApps", "OpenExcel", "");
            Assert.NotNull(obj);
            Assert.AreEqual(obj["Status"].Value<string>(), "ok");
            Assert.IsNotEmpty(obj["Info"].Value<string>());

            var infoTxt = obj["Info"].Value<string>();
            var info = JObject.Parse(infoTxt);
            excelID = info["ExcelID"].Value<string>();
            Assert.IsNotEmpty(excelID);
        }



    }
}
