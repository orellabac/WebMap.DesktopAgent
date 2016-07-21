using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Mobilize
{
    [DataContract]
    public class DesktopAgentInteractionRequest
    {
        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string PluginName { get; set; }
        
        [DataMember]
        public string Status { get; set; }
        
        [DataMember]
        public string ActionParams { get; set; }
    }
}
