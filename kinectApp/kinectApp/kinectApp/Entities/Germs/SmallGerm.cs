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
            int DirX, DirY;
            if (beenToTopHalfOfScreen)
            {
                DirY = Rand.Next(100) < 80 ? Rand.Next(3) * -1 : Rand.Next(2);
            }
            else
            {
                DirY = Rand.Next(100) < 20 ? Rand.Next(3) * -1 : Rand.Next(2);
            }
            DirX = Rand.Next(0, 5) - 2;

            PosY += DirY;
            PosX += DirX;

            
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            int x1 = (int)PosX;
            int y1 = (int)PosY;
            var rec = new Rectangle(x1, y1,WIDTH, HEIGHT);

            aSpriteBatch.Draw(Texture, rec, Color.White);
        }
    }
}
