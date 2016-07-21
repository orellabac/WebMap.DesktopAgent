using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace Mobilize
{
    // ValueProvideFactory. This is registered in the Service resolver like so:
    //    config.Services.Add(typeof(ValueProviderFactory), new HeaderValueProviderFactory());


    /// <summary>
    /// Some browsers do not allow to make CORS ajax calls when the content type is different from text/plain
    /// The standard WebAPI value providers will only deserialize JSON into method parameters if the content type is
    /// application/json.
    /// This Value providers is just used to overcome this limitation
    /// </summary>
    public class ApplicationTextValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            var content = actionContext.ControllerContext.Request.Content;
            return new ApplicationTextValueProvider(content);
        }
    }

    // ValueProvider for extracting data from headers for a given request message. 
    public class ApplicationTextValueProvider : IValueProvider
    {
        readonly HttpContent _content;

        public ApplicationTextValueProvider(HttpContent content)
        {
            _content = content;
        }

        // Headers doesn't support property bag lookup interface, so grab it with reflection.
        PropertyInfo GetProp(string name)
        {
            var p = typeof(HttpRequestHeaders).GetProperty(name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            return p;
        }

        public bool ContainsPrefix(string prefix)
        {
            //var p = GetProp(prefix);
            //return p != null;
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            ///Get the json text
            var str = _content.ReadAsStringAsync().Result;
            //Deserialize JSON into out request object
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Mobilize.DesktopAgentInteractionRequest>(str);
            return new ValueProviderResult(obj,"",CultureInfo.CurrentCulture); // none
        }
    }
}