using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Menu() : base("Menu")
        {
            _Quit = new Button("Quit", string.Empty, 20, 50, 0);
            _Play = new Button("Play", string.Empty, 20, 80, 0);

            _OptionA = null;
            _OptionB = null;

            Entities.Add(_Quit);
            Entities.Add(_Play);
        }

        public override void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager)
        {
            throw new NotImplementedException();
        }
    }
}
