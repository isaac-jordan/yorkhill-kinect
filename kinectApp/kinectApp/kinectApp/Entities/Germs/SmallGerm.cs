using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities.Germs
{
    public class SmallGerm : GermBase
    {
        static int BaseId = 2000;

        const int HEIGHT = 64;
        const int WIDTH = 64;

        public SmallGerm(string aAssetName, Vector3 aPos) : this(aAssetName, aPos.X, aPos.Y, aPos.Z) { }

        public SmallGerm(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ)
        {
            Id = BaseId++;
            Height = HEIGHT;
            Width = WIDTH;
        }


        public override void Load(ContentManager aContentManager)
        {
            base.Load(aContentManager);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime aGameTime)
        {
            this.PosX += 2.0f;
            this.PosY += 5.0f;
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            int x1 = int.Parse(PosX.ToString());
            int y1 = int.Parse(PosY.ToString());
            var rec = new Rectangle(x1, y1, Height, Width);

            aSpriteBatch.Draw(Texture, rec, Color.White);
        }
    }
}
