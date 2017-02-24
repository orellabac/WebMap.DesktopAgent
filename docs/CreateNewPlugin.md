Create a new Plugin class is very simply you just need implement the Iplugin interface in one c# class, 
inside this interface are the properties that you should implement. 
- string Name { get; }: 
indicate the Name of the Plugin 
- string ImgSource { get; }
: indicate the path plugin Icon 
- string  InvokeAction (IPlugin Plugin, string Action, string ActionParams)This is the gateway of any call that is made to the plugin, the parameters provided in the method allows you to send parameters between the agent and any Ajax call.
