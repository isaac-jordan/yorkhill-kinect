using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities.Germs
{
    public class BigGerm : GermBase
    {
        static int BaseId = 20223;
        static Random Rand = new Random((int)DateTime.Now.Ticks);

        const int HEIGHT = 64;
        const int WIDTH = 64;

        public BigGerm(string aAssetName, Vector3 aPos) : this(aAssetName, aPos.X, aPos.Y, aPos.Z) { }

        public BigGerm(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ)
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
            int DirX = Rand.Next(0, 10);
            int DirY = Rand.Next(0, 10);

            double AMX = Rand.NextDouble() * 2.5;
            double AMY = Rand.NextDouble() * 2.5;

            //Moving on the X
            if (DirX != 5)
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
            if (DirY != 5)
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
            int x1 = int.Parse(PosX.ToString());
            int y1 = int.Parse(PosY.ToString());
            var rec = new Rectangle(x1, y1, HEIGHT, WIDTH);

            aSpriteBatch.Draw(Texture, rec, Color.White);
        }
    }
}
