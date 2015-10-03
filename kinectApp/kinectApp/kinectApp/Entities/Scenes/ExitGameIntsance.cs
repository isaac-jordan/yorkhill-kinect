using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

using kinectApp.Entities.UI;
using kinectApp.Utilities;

namespace kinectApp.Entities.Scenes
{
    public class ExitGameIntsance : Scene
    {
        private IEntity iMessage;
        private IEntity iYesbtn;
        private IEntity iNobtn;

        public ExitGameIntsance() : base("ExitGameInstance")
        {
            iMessage = new Label("Bye Bye?", string.Empty, 50, 50, 0);

            Entities.Add(iMessage);
        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            //Close/Quit :)

            if(aInputHelper.IsNewPress(Keys.Y) || aInputHelper.IsNewPress(Keys.Enter))
            {
                Game1.ForceClose();
            }
        }
    }
}
