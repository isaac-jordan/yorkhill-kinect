using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

using kinectApp.Utilities;
using kinectApp.Entities.UI;

namespace kinectApp.Entities.Scenes
{
    public class KinectDisconnect : Scene
    {
        private IEntity iLabel;

        public KinectDisconnect() : base("kinect-disconnect")
        {
            iLabel = new BigLabel("Searching for Kinect...", string.Empty, 225, 250, 0);

            Entities.Add(iLabel);
        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            return;
        }
    }


    public class KinectLost : Scene
    {
        private IEntity iLabel;

        public KinectLost() : base("kinect-disconnect")
        {
            iLabel = new BigLabel("Searching for player...", string.Empty, 225, 250, 0);

            Entities.Add(iLabel);
        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            return;
        }
    }
}
