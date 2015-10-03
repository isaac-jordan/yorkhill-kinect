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

        public SmallGerm(Vector3 aPos) : base(aPos) { }

        public SmallGerm(float aX, float aY, float aZ) : base(aX, aY, aZ) { }

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
