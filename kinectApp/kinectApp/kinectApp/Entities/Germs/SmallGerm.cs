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
        static Random Rand = new Random(234230);


        public SmallGerm() : base() { Id = Rand.Next(0, 200); }

        public SmallGerm(string aAssetName) : base(aAssetName) { Id = Rand.Next(0, 200);  }

        public SmallGerm(string aAssetName, Vector3 aPos) : base(aAssetName, aPos) { Id = Rand.Next(0, 200); }

        public SmallGerm(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ) { Id = Rand.Next(0, 200); }

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
            //throw new NotImplementedException();
        }
    }
}
