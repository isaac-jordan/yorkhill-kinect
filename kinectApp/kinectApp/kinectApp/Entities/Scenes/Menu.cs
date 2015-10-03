﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kinectApp.Entities.UI;

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
            _Quit = new Button("Quit", string.Empty, 2, 2, 2);
        }
    }
}