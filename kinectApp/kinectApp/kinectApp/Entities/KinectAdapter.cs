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
        private MultiSourceFrameReader iFrameReader;
        private Texture2D iRGBVideo;
        private GraphicsDevice iGraphicsDevice;

        private byte[] iColourImageBuffer;
        private Body[] _bodies;
        private List<Point> hands = new List<Point>();
        private Action<bool> iUpdateTitle;
        private readonly static object iVideoLock = new object();
        private readonly static object iJointLock = new object();

        public const int kWidth = 1920;
        public const int kHeight = 1080;

        private List<Task> iProcessingTasks;

        public KinectAdapter(GraphicsDevice aGraphicsDevice, Action<bool> aUpdateTitle)
        {
            iSensor = KinectSensor.GetDefault();
            iGraphicsDevice = aGraphicsDevice;
            iUpdateTitle = aUpdateTitle;


            iProcessingTasks = new List<Task>(500);
            iColourImageBuffer = new byte[kWidth * kHeight * 4];
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
                lock (iVideoLock)
                {
                    return iRGBVideo;
                }
            }
        }

        /// <summary>
        /// Gets the current found joints from the last frame.
        /// </summary>
        public List<Point> KinectJoints
        {
            get
            {
                lock (iJointLock)
                {
                    return hands;
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
                iFrameReader = iSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);

                iFrameReader.MultiSourceFrameArrived += KFrameReader_MultiSourceFrameArrived;
            }
        }

        private void KFrameReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var T = Task.Factory.StartNew(() =>
            {
                // Retrieve multisource frame reference
                MultiSourceFrameReference multiRef = e.FrameReference;
                MultiSourceFrame multiFrame;
                try
                {
                    multiFrame = multiRef.AcquireFrame();
                }
                catch (System.InvalidOperationException)
                {
                    return;
                }


                if (multiFrame == null) return;

                // Retrieve data stream frame references
                ColorFrameReference colorRef = multiFrame.ColorFrameReference;
                BodyFrameReference bodyRef = multiFrame.BodyFrameReference;

                Task.Factory.StartNew(() => ProcessRGBVideo(colorRef));
                Task.Factory.StartNew(() => ProcessJoints(bodyRef));

            }).ContinueWith((aTask) => iProcessingTasks.Remove(aTask));

            iProcessingTasks.Add(T);
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
        private void ProcessRGBVideo(ColorFrameReference aReference)
        {
            using (ColorFrame colorImageFrame = aReference.AcquireFrame())
            {
                if (colorImageFrame != null)
                {
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

                    var video = new Texture2D(iGraphicsDevice, kWidth, kHeight);

                    //TODO Stop stupid AccessViolation
                    video.SetData(color);


                    lock (iVideoLock)
                    {
                        iRGBVideo = video;
                    }
                }
            }
        }

        private void ProcessJoints(BodyFrameReference aReference)
        {
            using (BodyFrame bodyFrame = aReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    _bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];

                    bodyFrame.GetAndRefreshBodyData(_bodies);

                    hands.Clear();

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                JointType[] JointsToGet = { JointType.HandRight, JointType.HandLeft, JointType.WristRight, JointType.WristLeft };

                                //Console.WriteLine("I can see a joint at:" + handRight.Position.X + ", " + handRight.Position.Y + ", " + handRight.Position.Z);

                                foreach (JointType jt in JointsToGet)
                                {
                                    hands.Add(MapJointToPoint(body.Joints[jt]));
                                }
                            }
                        }
                    }
                }
            }
        }

        private Point MapJointToPoint(Joint joint)
        {
            CameraSpacePoint skeletonPoint = joint.Position;
            ColorSpacePoint colorPoint = iSensor.CoordinateMapper.MapCameraPointToColorSpace(skeletonPoint);
            // 2D coordinates in pixels

            // Skeleton-to-Color mapping

            Point point = new Point();
            point.X = (int)colorPoint.X;
            point.Y = (int)colorPoint.Y;
            return point;
        }

        //Process a change in the availability of Kinect
        private void KSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            IsAvailable = e.IsAvailable;
            iRGBVideo = null;

            iUpdateTitle(IsAvailable);
        }

        //Frees resources used by the adapter
        public void Dispose()
        {
            //Wait for all the processing to be done on the video frames
            //Sometimes we'd be disposing while we're trying to re-construct the RGBVideo
            var Tasks = iProcessingTasks.ToArray();
            Task.WaitAll(Tasks);

            iProcessingTasks.Clear();

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
