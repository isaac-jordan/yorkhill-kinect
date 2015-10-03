using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities
{
    public class KinectAdapter : IDisposable
    {
        private KinectSensor iSensor;
        private ColorFrameReader iFrameReader;
        private Texture2D iRGBVideo;
        private GraphicsDevice iGraphicsDevice;

        private byte[] iColourImageBuffer;
        private readonly object iLock = new object();


        private const int kWidth = 1920;
        private const int kHeight = 1080;

        public KinectAdapter(GraphicsDevice aGraphicsDevice)
        {
            iSensor = KinectSensor.GetDefault();
            iGraphicsDevice = aGraphicsDevice;
        }

        /// <summary>
        /// Gets if the Kinect Sensor is currently open
        /// </summary>
        public bool IsOpen
        {
            get; private set;
        }

        /// <summary>
        /// Gets if a Kinect is available
        /// </summary>
        public bool IsAvailable
        {
            get; private set;
        }

        /// <summary>
        /// Gets a string representing the connection status of the Kinect
        /// </summary>
        public string ConnectedStatus
        {
            get
            {
                return string.Format("Source {0}AVAILABLE", (IsAvailable) ? string.Empty : "NOT ");
            }
        }

        /// <summary>
        /// Gets the current RGBVideo stream from the Kinect sensor.
        /// </summary>
        public Texture2D KinectRGBVideo
        {
            get
            {
                lock (iLock)
                {
                    return iRGBVideo;
                }
            }
        }

        /// <summary>
        /// Opens up a Kinect sensor attaching to the right events
        /// </summary>
        /// <param name="aHandler"></param>
        public void OpenSensor()
        {
            if (!IsOpen)
            {
                //Open Sensor so we can use it!
                iSensor.Open();
                IsOpen = true;

                iSensor.IsAvailableChanged += KSensor_IsAvailableChanged;
            
                //Setup the video feed from the Kinect Camera!
                iFrameReader = iSensor.ColorFrameSource.OpenReader();
                iFrameReader.FrameArrived += KFrameReader_FrameArrived;
            }
        }

        /// <summary>
        /// Releases resources used by the Kinect objects
        /// </summary>
        public void CloseSensor()
        {
            if (IsOpen)
            {
                iSensor.IsAvailableChanged -= KSensor_IsAvailableChanged;
                iFrameReader.Dispose();
                iFrameReader = null;

                iSensor.Close();
                IsOpen = false;
            }
        }


        //Processes the Frame data from the Kinect camera.
        //Since events are called synchronously, this would bottleneck and cause an issue with framerate
        //By threading, we process the info on seperate threads, allowing execution to coninue with the rest of the game
        private void KFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                using (ColorFrame colorImageFrame = e.FrameReference.AcquireFrame())
                {
                    if (colorImageFrame != null)
                    {
                        if ((iColourImageBuffer == null) || (iColourImageBuffer.Length != kWidth * kHeight * /*colorImageFrame.FrameDescription.BytesPerPixel*/ 4))
                        {
                            iColourImageBuffer = new byte[kWidth * kHeight * /*colorImageFrame.FrameDescription.BytesPerPixel*/ 4];
                        }

                        colorImageFrame.CopyConvertedFrameDataToArray(iColourImageBuffer, ColorImageFormat.Rgba);

                        Color[] color = new Color[kHeight * kWidth];

                        // Go through each pixel and set the bytes correctly
                        // Remember, each pixel got a Red, Green and Blue
                        int index = 0;
                        for (int y = 0; y < kWidth; y++)
                        {
                            for (int x = 0; x < kHeight; x++)
                            {
                                Color c = new Color(iColourImageBuffer[index + 0], iColourImageBuffer[index + 1], iColourImageBuffer[index + 2], iColourImageBuffer[index + 3]);
                                color[y * kHeight + x] = c;
                                index += 4;
                            }
                        }

                        lock (iLock)
                        {
                                iRGBVideo = new Texture2D(iGraphicsDevice, kWidth, kHeight);
                                // Set pixeldata from the ColorImageFrame to a Texture2D
                                iRGBVideo.SetData(color);
                        }
                    }
                }
            });
        }

        //Process a change in the availability of Kinect
        private void KSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            IsAvailable = e.IsAvailable;
            iRGBVideo = null;
        }

        //Frees resources used by the adapter
        public void Dispose()
        {
            if (IsOpen)
            {
                CloseSensor();
            }

            if (iRGBVideo != null)
            {
                iRGBVideo.Dispose();
            }

            iFrameReader = null;
            iSensor = null;
        }
    }
}
