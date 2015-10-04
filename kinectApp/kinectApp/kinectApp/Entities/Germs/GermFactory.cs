using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace kinectApp.Entities.Germs
{
    public class GermFactory
    {
        static Random Rand = new Random(DateTime.Now.Millisecond + 13);

        /// <summary>
        /// Creates a Small germ at a random location
        /// </summary>
        /// <returns></returns>
        public static IEntity CreateSmallGerm()
        {
            float x = (float)(Rand.NextDouble() * Program.game.screenWidth);
            float y = Program.game.screenHeight;

            return new SmallGerm("SmallGerm", new Vector3(x, y, 0));
        }

        /// <summary>
        /// Creates a Small germ at the specified location
        /// </summary>
        /// <param name="aX">X co-ord</param>
        /// <param name="aY">Y co-ord</param>
        /// <param name="aZ">Z co-ord</param>
        /// <returns></returns>
        public static IEntity CreateSmallGerm(float aX, float aY, float aZ)
        {
            return new SmallGerm("SmallGerm", new Vector3(aX, aY, aZ));
        }


        /// <summary>
        /// Createsa a Big Germ at a random location
        /// </summary>
        /// <returns></returns>
        public static IEntity CreateBigGerm()
        {
            float x = (float)(Rand.NextDouble() * Program.game.screenWidth);
            float y = Program.game.screenHeight;

            return new BigGerm("BigGerm", new Vector3(x, y, 0));
        }

        /// <summary>
        /// Creates a new BigGerm at the specified position
        /// </summary>
        /// <param name="aX">X co-ord</param>
        /// <param name="aY">Y co-ord</param>
        /// <param name="aZ">Z co-ord</param>
        /// <returns></returns>
        public static IEntity CreateBigGerm(float aX, float aY, float aZ)
        {
            return new BigGerm("BigGerm", new Vector3(aX, aY, aZ));
        }
    }
}
