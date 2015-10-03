using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kinect_mouse_mapper.MouseManipulator;
using Microsoft.Kinect;
using System.Windows.Forms;

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

                                _joints[0] = handRight;
                                _joints[1] = thumbRight;
                                _joints[2] = handLeft;
                                _joints[3] = thumbLeft;

                                VirtualMouse.Move((int)((_joints[1].Position.X) * 10)< 1 ? (int)((_joints[1].Position.X) * 10):0, (int)((_joints[1].Position.X) * 10) < 1 ? (int)(-(_joints[1].Position.Y) * 10) : 0);
                                Console.WriteLine(string.Format("\r{0},{1}", Cursor.Position.X, Cursor.Position.Y));
                            }
                        }
                    }
                }
            }


        }


    }
}
