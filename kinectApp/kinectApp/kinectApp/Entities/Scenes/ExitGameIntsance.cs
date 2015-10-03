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
            iMessage = new Label("Bye Bye?", string.Empty, 50, 100, 0);

            iYesbtn = new Button("Yes", string.Empty, 50, 135, 0);
            iNobtn = new Button("Back", string.Empty, 190, 135, 0);

            Entities.Add(iMessage);
            Entities.Add(iYesbtn);
            Entities.Add(iNobtn);
        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            //Close/Quit :)

            if(aInputHelper.IsNewPress(Keys.Q) || aInputHelper.IsNewPress(Keys.Enter) || aInputHelper.IsNewPress(Keys.Y))
            {
                Game1.ForceClose();
                return;
            }

            //We don't want to close quit - return to previous screen
            if (aInputHelper.IsNewPress(Keys.N) || aInputHelper.IsNewPress(Keys.Back))
            {
                aSceneManager.HideOverlay();
                return;
            }
        }
    }
}
