using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kinectApp.Entities.Germs
{
    public class GermFactory
    {
        public static IEntity CreateSmallGerm()
        {
            return new SmallGerm("SmallGerm", new Microsoft.Xna.Framework.Vector3(0,2,3));
        }

   }
}
