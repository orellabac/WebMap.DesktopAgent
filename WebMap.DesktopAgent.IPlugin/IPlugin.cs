using System;

namespace Mobilize
{

    /// <summary>
    /// Thrown when the plugin does not know the given action
    /// </summary>
    public class InvalidPluginAction : Exception
    {
        public readonly string ActionName;
        public InvalidPluginAction(string actionName) 
        {
            ActionName = actionName;
        }
    }
    /// <summary>
    /// Defines a contract that can be implemented to create a WebMap Desktop Agent plugin that 
    /// can add functionality so webmap migrated application can interact with the client Desktop
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// An unique identifier for the plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// This is the gateway of any call that is made to the plugin, 
        /// </summary>
        /// <param name="Plugin"></param>
        /// <param name="Action"></param>
        /// <param name="ActionParams"></param>
        /// <returns></returns>
        string  InvokeAction(IPlugin Plugin, string Action,string ActionParams);
        /// <summary>
        /// Indicate the Plugin Image Source Path 
        /// </summary>
        string ImgSource { get; }
    }
}
