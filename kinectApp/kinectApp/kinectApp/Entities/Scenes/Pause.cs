using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kinectApp.Utilities;

namespace kinectApp.Entities.Scenes
{
    /*
        Pause Screen of the Game
    */
    public class Pause : Scene
    {
        public Pause() : base("Pause")
        {

        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            throw new NotImplementedException();
        }
    }
}
