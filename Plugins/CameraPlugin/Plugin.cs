using Mobilize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AForge.Video.DirectShow;
using Fleck;
using System.ComponentModel.Composition;
using System.Threading;

namespace CameraPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        public string Name => "CameraPlugin";

        public string ImgSource => null;
        private Camera current;
        private WebSocketServer server;

        public string InvokeAction(IPlugin Plugin, string Action, string ActionParams)
        {
            switch (Action)
            {
                case "Devices":
                    {
                        var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                        var devices = videoDevices.Cast<FilterInfo>()
                            .Select(x => new JObject(new JProperty("Name", x.Name), new JProperty("Moniker", x.MonikerString)));
                        return new JArray(devices).ToString();
                    }
                case "Capabilities":
                    {
                        var moniker = ActionParams;
                        var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                        var videoSource = new VideoCaptureDevice(moniker);
                        int counter = 0;


                        var capabilities = videoSource.VideoCapabilities
                            .Select(x => 
                            new JObject(
                                new JProperty("ID", counter++),
                                new JProperty("AverageFrameRate", x.AverageFrameRate),
                                new JProperty("BitCount",x.BitCount),
                                new JProperty("FrameSize",
                                    new JObject(
                                        new JProperty("Width", x.FrameSize.Width), 
                                        new JProperty("Height", x.FrameSize.Height))),
                                new JProperty("MaximumFrameRate", x.MaximumFrameRate))
                            );
                        return new JArray(capabilities).ToString();
                    }
                case "Start":
                    {
                        var parameters = ActionParams.Split(',');
                        var moniker = parameters[0];
                        var capability = int.Parse(parameters[1]);
                        var quality = int.Parse(parameters[2]);
                        this.StartStreaming(moniker, capability, quality);
                        //this.current = new Camera();
                        //this.current.Initialize(moniker, capability, quality);
                        //this.current.Start();
                        return "OK";
                    }
                case "Stop":
                    {
                        this.current.Stop();
                        this.server.Dispose();
                        return "OK";
                    }
                default:
                    throw new InvalidPluginAction(Action);
            }

        }

        private void StartStreaming(string moniker, int capability, int quality)
        {
            var url = "http://+:8080";
            Camera cam = new Camera();
            cam.Initialize(moniker, capability, quality);
            cam.Start();
         
            this.current = cam;
            //webapp = WebApp.Start<Startup>(url);

            this.server = new WebSocketServer("ws://0.0.0.0:8080");
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message =>
                {
                    switch (message)
                    {
                        case "VideoFrame":
                            var cameraFrame = this.current.Frame ?? new byte[0];
                            //this.WriteFrame(cameraFrame, OpCode.Binary, socket);
                            socket.Send(cameraFrame);
                            break;
                    }
                };
            });

        }
    }
}
