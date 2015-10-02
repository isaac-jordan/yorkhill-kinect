using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace kinectApp.Entities.Germs
{
    /// <summary>
    /// Base Germ object. Must inherit of this and implement the required methods
    /// </summary>
    public abstract class GermBase : IGerm
    {
        private Vector3 _pos;


        /// <summary>
        /// Create new Germ with position 0,0,0
        /// </summary>
        public GermBase()
        {
            _pos = new Vector3(0,0,0);
        }

        /// <summary>
        /// Create a new Germ with the specified a position
        /// </summary>
        /// <param name="aPos">A Position</param>
        public GermBase(Vector3 aPos)
        {
            _pos = aPos;
        }

        /// <summary>
        /// Create a new Germ with a Custom position
        /// </summary>
        /// <param name="aX">X Pos</param>
        /// <param name="aY">Y Pos</param>
        /// <param name="aZ">Z Pos</param>
        public GermBase(float aX, float aY, float aZ)
        {
            _pos = new Vector3(aX, aY, aZ);
        }


        /// <summary>
        /// Gets the ID of the germ
        /// </summary>
        public int Id{ get; private set; }

        /// <summary>
        /// Gets if the Germ is dead.
        /// </summary>
        public bool IsDead
        {
            get; private set; 
        }

        /// <summary>
        /// Gets the Position of the Germ in the Game
        /// </summary>
        public Vector3 Pos
        {
            get { return _pos; }
        }

        /// <summary>
        /// Gets or Sets the X position of the Germ
        /// </summary>
        public float PosX
        {
            get { return Pos.X; }
            set { _pos.X = value; }
        }

        /// <summary>
        /// Gets or Sets the Y position of the Germ
        /// </summary>
        public float PosY
        {
                get { return Pos.Y; }
                set { _pos.Y = value; }
        }

        /// <summary>
        /// Gets or Sets the Z position of the Germ
        /// </summary>
        public float PosZ
        {
            get { return Pos.Z; }
            set { _pos.Z = value; }
        }
    

        public abstract void Draw(GameTime aGameTime);

        public abstract void Load();

        public abstract void Unload();

        public abstract void Update(GameTime aGameTime);
    }
}
