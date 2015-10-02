using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace kinectApp.Entities.Germs
{
    public class SmallGerm : GermBase
    {
        public SmallGerm() : base() { }

        public SmallGerm(Vector3 aPos) : base(aPos) { }

        public SmallGerm(float aX, float aY, float aZ) : base(aX, aY, aZ) { }

        public override void Draw(GameTime aGameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Load()
        {
            //throw new NotImplementedException();
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
