using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities
{
    /*
        Based Game entity interface.
    */
    public interface IEntity
    {
        Vector3 Pos { get; }
        Texture2D Texture { get; }
        string AssetName { get; }

        int Height { get; }
        int Width { get; }

        void Update(GameTime aGameTime);
        void Draw(SpriteBatch aSpriteBatch);

        void Load(ContentManager aContentManager);
        void Unload();

        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
    }
}
