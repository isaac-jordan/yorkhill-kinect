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
        public SmallGerm() : base() { }

        public SmallGerm(string aAssetName) : base(aAssetName) { }

        public SmallGerm(string aAssetName, Vector3 aPos) : base(aAssetName, aPos) { }

        public SmallGerm(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ) { }

        public override void Load(ContentManager aContentManager)
        {
            base.Load(aContentManager);
        }

        public override void Unload()
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime aGameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
