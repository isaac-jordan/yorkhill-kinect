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
    /*
        Pause Screen of the Game
    */
    public class Pause : Scene
    {
        private IEntity iLabel;

        public Pause() : base("Pause")
        {
            iLabel = new Label("Paused...", string.Empty, 250, 250, 0);

            Entities.Add(iLabel);
        }

        //Pause Screen is dismissed with Space!
        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            if (aInputHelper.IsNewPress(Keys.Space))
            {
                aSceneManager.HideOverlay();
            }
        }
    }
}
