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
        static Random Rand = new Random(DateTime.UtcNow.TimeOfDay.Milliseconds);

        const int HEIGHT = 64;
        const int WIDTH = 64;

        public SmallGerm(string aAssetName, Vector3 aPos) : this(aAssetName, aPos.X, aPos.Y, aPos.Z) { }

        public SmallGerm(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ)
        {
            Id = BaseId++;
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
            int DirX = Rand.Next(0, 12);
            int DirY = Rand.Next(0, 12);

            double AMX = Rand.NextDouble() * 5.5;
            double AMY = Rand.NextDouble() * 5.5;

            //Moving on the X
            if (DirX != 6)
            {
                switch (DirX % 2)
                {
                    case 0:
                    {
                        PosX += (float)AMX;
                        break;
                    }
                    case 1:
                    {
                        PosX -= (float)AMX;
                        break;
                    }
                }
            }

            //Moving on the Y
            if (DirY != 6)
            {
                switch (DirY % 2)
                {
                    case 0:
                    {
                        PosY -= (float)AMY;
                        break;
                    }
                    case 1:
                    {
                        PosY += (float)AMY;
                        break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            int x1 = (int)PosX;
            int y1 = (int)PosY;
            var rec = new Rectangle(x1, y1,HEIGHT, WIDTH);

            aSpriteBatch.Draw(Texture, rec, Color.White);
        }
    }
}
