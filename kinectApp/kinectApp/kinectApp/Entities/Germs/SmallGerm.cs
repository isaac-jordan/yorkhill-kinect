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

        float movementScale = 1.0f;

        public override int Height
        {
            get
            {
                return 16;
            }
        }
        const int BASEHEALTH = 100;

        public override int Width
        {
            get
            {
                return 16;
            }
        }

        public SmallGerm(string aAssetName, Vector3 aPos) : this(aAssetName, aPos.X, aPos.Y, aPos.Z) { }

        public SmallGerm(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ)
        {
            Id = BaseId++;
            Health = BASEHEALTH;
            HasBeenHit = false;
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
            if ((PosX < 0 - Width || PosX > Program.game.depthWidth + Width) || (PosY < 0 - Height || PosY > Program.game.depthHeight + Height) || Health <= 0)
            {
                IsDead = true;
                return;
            }

            int DirX, DirY;
            if (!beenToTopHalfOfScreen)
            {
                DirY = Rand.Next(100) < 80 ? Rand.Next(2,5) * -1 : Rand.Next(2,6); //80% chance to move up
                if (PosY < Program.game.depthHeight / 3) beenToTopHalfOfScreen = true;
            }
            else
            {
                DirY = Rand.Next(100) < 20 ? Rand.Next(2,3) * -1 : Rand.Next(2,4); // 20% chance to move up
            }
            DirX = Rand.Next(0, 5) - 2;

            PosY += (float)DirY * movementScale;
            PosX += (float)DirX * movementScale;
            movementScale += 0.01f;
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            int x1 = (int)PosX;
            int y1 = (int)PosY;
            var rec = new Rectangle(x1, y1, Width, Height);

            aSpriteBatch.Draw(Texture, rec, Color.White);
        }
    }
}
