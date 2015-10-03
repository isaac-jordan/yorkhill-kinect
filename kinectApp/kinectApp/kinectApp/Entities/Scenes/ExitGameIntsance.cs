using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }
    }
}
