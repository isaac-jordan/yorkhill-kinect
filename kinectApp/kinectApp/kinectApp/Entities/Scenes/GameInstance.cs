using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kinectApp.Utilities;

using Microsoft.Xna.Framework.Input;

using kinectApp.Entities.Germs;

namespace kinectApp.Entities.Scenes
{
    /*
        Actual running of the Game
    */
    public class GameInstance : Scene
    {
        List<IEntity> iGerms;


        public GameInstance() : base("Game")
        {
            iGerms = new List<IEntity>();

            iGerms.Add(GermFactory.CreateSmallGerm(450,550,0));
            iGerms.Add(GermFactory.CreateSmallGerm(755,230,0));
            iGerms.Add(GermFactory.CreateBigGerm(260,345,0));

            for (int i = 0; i < 25; i++)
            {
                iGerms.Add(GermFactory.CreateSmallGerm());
            }

            for (int i = 0; i < 10; i++)
            {
                iGerms.Add(GermFactory.CreateBigGerm());
            }


            Entities.AddRange(iGerms);
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
