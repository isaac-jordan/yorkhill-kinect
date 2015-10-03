using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kinectApp.Utilities;

using Microsoft.Xna.Framework.Input;

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

        //Show Pause screen with P
        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            //Show Pause Screen
            if (aInputHelper.IsNewPress(Keys.P))
            {
                aSceneManager.ShowOverlay(new Pause());
            }

            //Show The exit screen
            if (aInputHelper.IsNewPress(Keys.Escape))
            {
                aSceneManager.ShowOverlay(new ExitGameIntsance());
            }
        }
    }
}
