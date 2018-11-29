using AForge.Video;
using AForge.Video.DirectShow;
using BitMiracle.LibJpeg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;


namespace CameraPlugin
{
    /// <summary>
    /// Camera: ELP 2.8mm wide angle lens 1080p HD USB Camera Module (ELP-USBFHD01M-L28)
    /// </summary>
    public class Camera
    {
        public static Camera camera;
        public byte[] Frame { get; set; }
        

        private int _threadsCount = 0;
        private int _stoppedThreads = 0;
        private bool _stopThreads = false;

        private volatile Stopwatch _lastFrameAdded = new Stopwatch();
        private volatile object _lastFrameAddedLock = new object();

        VideoCaptureDevice videoSource;
        private int Quality;
        private Thread capturingThread;

        public void Initialize(string moniker, int capability, int quality = 100)
        {
            this.Quality = quality;
            // enumerate video devices
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            // create video source

            videoSource = new VideoCaptureDevice(moniker);
            // set NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.VideoResolution = videoSource.VideoCapabilities[capability];

            // ...
            // signal to stop when you no longer need capturing
           // videoSource.SignalToStop();
            _lastFrameAdded.Start();
                
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // get new frame
            Bitmap bitmap = eventArgs.Frame;
            // // process the frame
            // ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // // Create an Encoder object based on the GUID
            // // for the Quality parameter category.
            // System.Drawing.Imaging.Encoder myEncoder =
            //     System.Drawing.Imaging.Encoder.Quality;

            // // Create an EncoderParameters object.
            // // An EncoderParameters object has an array of EncoderParameter
            // // objects. In this case, there is only one
            // // EncoderParameter object in the array.
            // EncoderParameters myEncoderParameters = new EncoderParameters(1);

            // EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
            //     50L);
            // myEncoderParameters.Param[0] = myEncoderParameter;
            // var memStream = new MemoryStream();
            //// bitmap.Save(memStream, ImageFormat.Jpeg);
            //bitmap.Save(memStream, jgpEncoder, myEncoderParameters);
            // this.Frame = memStream.ToArray();
            using (var memStream = new MemoryStream()) {

                CompressionParameters parameters = new CompressionParameters();
                parameters.Quality = this.Quality;
                BitMiracle.LibJpeg.JpegImage.FromBitmap(bitmap).WriteJpeg(memStream, parameters);
                this.Frame = memStream.ToArray();
                    }
            
            
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GarbageCollectorCanWorkHere() { }



        public void Start()
        {
            this.capturingThread = new Thread(() => videoSource.Start());
            this.capturingThread.Start();
        }

        public async Task Stop()
        {
            videoSource.SignalToStop();
            this.capturingThread.Abort();
        }
    }
}