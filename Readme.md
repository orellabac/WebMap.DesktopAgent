WebMap Desktop Agent
=====================

When you are developing applications for a Desktop Environment you have access to a very rich environment. There are a lot of “services” available and a lot of low-level tasks are performed on your behalf by the .net framework. 
By contrast on a web application, a lot of these “services” are not provided. 

During [WebMap](http://www.mobilize.net/webmap) migrations this is a very common scenario because WebMap upgraded applications come usually from VB6 or Windows Forms migrations to ASP.NET MVC.


Why?
----
WebMap applications might need to interact with the client Desktop.
You either need to write to special directory, interact with a locally installed program or 
use an special device.

Solution
--------

The WebMap Desktop Agent provide a mean to load special functionality package in dlls that we call
'Plugins'.

The WebMap Desktop Agent uses a self host. For example at localhost:60064 and allows the browser to comunicate with it.

The Desktop Agent will then forward these request to the plugins which can perform the special funcionality that you need

The solution currently bundles two plugins

HelloWorld: This is just an example plugin which return 'Hello World'

OfficeApps: This is a very basic plugins that allow the web app to start either Word or Excel and interact with them.


These plugins are very basic they have been provided just to illustrate the concept of WebMap Desktop Agent plugins




### Build Status

 - Windows ![https://ci.appveyor.com/api/projects/status/github/orellabac/WebMap.DesktopAgent](https://ci.appveyor.com/api/projects/status/github/orellabac/WebMap.DesktopAgent)
