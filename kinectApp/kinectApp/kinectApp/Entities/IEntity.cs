using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kinectApp.Entities
{
    /*
        Based Game entity interface.
    */
    public interface IEntity
    {
        void Draw(GameTime aGameTime);
        void Update(GameTime aGameTime);

        void Load();
        void Unload();
    }
}
