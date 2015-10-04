using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kinect_mouse_mapper.MouseManipulator;
using Microsoft.Kinect;
using System.Windows.Forms;
using System.Drawing;

namespace kinect_mouse_mapper
{
    class Program
    {
        static KinectSensor sensor;
        static MultiSourceFrameReader _multiReader;
        static Body[] _bodies;
        static Joint[] _joints;
        static void Main(string[] args)
        {
            sensor = KinectSensor.GetDefault();

            sensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            sensor.Open();

            _multiReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);

            _multiReader.MultiSourceFrameArrived += OnMultipleFramesArrivedHandler;

            VirtualMouse.MoveTo(900, 39);
            VirtualMouse.LeftClick();
            Console.ReadKey();
        }

        private static void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            Console.WriteLine(string.Format("KINECT STATUS CHANGE: {0}", (e.IsAvailable) ? "Available" : "Not Available"));
        }

        private static void OnMultipleFramesArrivedHandler(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // Retrieve multisource frame reference
            MultiSourceFrameReference multiRef = e.FrameReference;

            MultiSourceFrame multiFrame = multiRef.AcquireFrame();
            if (multiFrame == null) return;

            // Retrieve data stream frame references
            BodyFrameReference bodyRef = multiFrame.BodyFrameReference;

            using (BodyFrame bodyFrame = bodyRef.AcquireFrame())
            {
                if (bodyFrame == null) return;

                if (bodyFrame != null)
                {
                    _bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];

                    bodyFrame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                // Find the joints
                                Joint handRight = body.Joints[JointType.HandRight];
                                Joint thumbRight = body.Joints[JointType.ThumbRight];

                                Joint handLeft = body.Joints[JointType.HandLeft];
                                Joint thumbLeft = body.Joints[JointType.ThumbLeft];

                                if (_joints == null || _joints.Length != 4)
                                {
                                    _joints = new Joint[4];
                                }

                                
                                Joint j = handRight;

                                CameraSpacePoint skeletonPoint = j.Position;
                                ColorSpacePoint colorPoint = sensor.CoordinateMapper.MapCameraPointToColorSpace(skeletonPoint);
                                // 2D coordinates in pixels

                                // Skeleton-to-Color mapping

                                Point point = new Point();
                                point.X = (int)colorPoint.X;
                                point.Y = (int)colorPoint.Y;
                               
                                VirtualMouse.MoveTo(point.X, point.Y);
                                //Console.WriteLine(string.Format("\r{0},{1}", Cursor.Position.X, Cursor.Position.Y));
                                Console.WriteLine(body.HandRightState);

                                long lasttime = (long)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
        
                                if (body.HandRightState != HandState.NotTracked && (((long)DateTime.UtcNow.TimeOfDay.TotalMilliseconds - lasttime) >= 1000))
                                {
                                    if (body.HandRightState == HandState.Closed)
                                    {
                                        lasttime = (long)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
                                        VirtualMouse.LeftClick();
                                    }
                                    if (body.HandRightState == HandState.Lasso)
                                    {
                                        lasttime = (long)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
                                        VirtualMouse.RightClick();
                                    }
                                }
                            }
                        }
                    }
                }
            }


        }


    }
}
