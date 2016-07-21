using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobilize
{
    /// <summary>
    /// In order to enable comunication between the WebBrowser and the 
    /// Desktop Agent we must enable CORS
    /// </summary>
    public class CORSHeaderHandler : System.Net.Http.DelegatingHandler
    {
        protected override Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith((task) =>
                {
                    System.Net.Http.HttpResponseMessage response = task.Result;
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("Access-Control-Allow-Methods", "POST");
                    response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With, Session");
                    return response;
                });
        }
    }
}
