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
    public class KinectAdapter
    {
        private KinectSensor iSensor;
        private ColorFrameReader iFrameReader;
        private Texture2D iRGBVideo;
        private GraphicsDevice iGraphicsDevice;

        private byte[] _colorImageBuffer;
        private bool _colorIsDrawing;

        private readonly object iLock = new object();

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
        public string ContectedStatus
        {
            get
            {
                return string.Format("Source {0}AVAILABLE", (IsAvailable) ? string.Empty : "NOT ");
            }
        }

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

        public void OpenSensor()
        {
            if (!IsOpen)
            {
                iSensor.Open();
                IsOpen = true;

                iSensor.IsAvailableChanged += KSensor_IsAvailableChanged;
       
                iFrameReader = iSensor.ColorFrameSource.OpenReader();
                iFrameReader.FrameArrived += KFrameReader_FrameArrived;
            }
        }

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


        private void KFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (_colorIsDrawing) return;
            _colorIsDrawing = true;
            using (ColorFrame colorImageFrame = e.FrameReference.AcquireFrame())
            {
                if (colorImageFrame != null)
                {
                    int width = colorImageFrame.FrameDescription.Width;
                    int height = colorImageFrame.FrameDescription.Height;

                    if ((_colorImageBuffer == null) || (_colorImageBuffer.Length != width * height * /*colorImageFrame.FrameDescription.BytesPerPixel*/ 4))
                    {
                        _colorImageBuffer = new byte[width * height * /*colorImageFrame.FrameDescription.BytesPerPixel*/ 4];
                    }

                    colorImageFrame.CopyConvertedFrameDataToArray(_colorImageBuffer, ColorImageFormat.Rgba);

                    Color[] color = new Color[height * width];

                    // Go through each pixel and set the bytes correctly
                    // Remember, each pixel got a Red, Green and Blue
                    int index = 0;
                    for (int y = 0; y < width; y++)
                    {
                        for (int x = 0; x < height; x++)
                        {
                            Color c = new Color(_colorImageBuffer[index + 0], _colorImageBuffer[index + 1], _colorImageBuffer[index + 2], _colorImageBuffer[index + 3]);
                            color[y * height + x] = c;
                            index += 4;
                        }
                    }

                    lock (iLock)
                    {
                        iRGBVideo = new Texture2D(iGraphicsDevice, width, height);
                        // Set pixeldata from the ColorImageFrame to a Texture2D
                        iRGBVideo.SetData(color);
                    }
                }
            }
        }

        private void KSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            IsAvailable = e.IsAvailable;
        }

    }
}
