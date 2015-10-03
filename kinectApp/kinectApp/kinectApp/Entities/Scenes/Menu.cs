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
    /*
        Game Main Menu
    */
    public class Menu : Scene
    {
        private IEntity _OptionA;
        private IEntity _OptionB;
        private IEntity _Quit;
        private IEntity _Play;
        private IEntity iLabel;

        public Menu() : base("Menu")
        {
            _Play = new Button("Play", string.Empty, 20, 50, 0);
            _Quit = new Button("Quit", string.Empty, 20, 85, 0);

            iLabel = new Label("Dynamic Dorks :)", string.Empty, 500, 0, 0);

            _OptionA = null;
            _OptionB = null;

            Entities.Add(_Quit);
            Entities.Add(_Play);
            Entities.Add(iLabel);
        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            //Does the user want to quit?
            if (aInputHelper.IsNewPress(Keys.Q) || aInputHelper.IsNewPress(Keys.Escape))
            {
                aSceneManager.ShowOverlay(new ExitGameIntsance());
                return;
            }

            //Else - Let's be silly!
            if (aInputHelper.IsCurPress(Keys.E) &&
                aInputHelper.IsCurPress(Keys.W) &&
                aInputHelper.IsCurPress(Keys.A) &&
                aInputHelper.IsCurPress(Keys.N))
            {
                //aSceneManager.SetScene(new SuperSpecialAwesomeScene());
                Console.WriteLine("Dylan smells!");
                return;
            }
        }
    }
}
