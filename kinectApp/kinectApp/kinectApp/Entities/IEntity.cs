using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities
{
    /*
        Based Game entity interface.
    */
    public interface IEntity
    {
        Vector3 Pos { get; }

        void Draw(GameTime aGameTime);
        void Update(GameTime aGameTime);

        void Load(SpriteBatch aSpriteBatch);
        void Unload();

        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
    }
}
