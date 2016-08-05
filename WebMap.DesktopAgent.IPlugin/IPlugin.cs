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
        string  InvokeAction(IPlugin Plugin, string Action,string ActionParams);

        string ImgSource { get; }
    }
}
