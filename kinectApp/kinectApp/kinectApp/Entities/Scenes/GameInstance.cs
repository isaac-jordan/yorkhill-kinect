using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kinectApp.Utilities;

namespace kinectApp.Entities.Scenes
{
    /*
        Actual running of the Game
    */
    public class GameInstance : Scene
    {
        public GameInstance() : base("Game")
        {

        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            throw new NotImplementedException();
        }
    }
}
