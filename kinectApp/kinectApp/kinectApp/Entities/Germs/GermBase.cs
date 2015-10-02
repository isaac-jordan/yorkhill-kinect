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

        public float PosY
        {
                get { return Pos.Y; }
                set { _pos.Y = value; }
        }

        public float PosZ
        {
            get { return Pos.Z; }
            set { _pos.Z = value; }
        }

        public void Draw(GameTime aGameTime)
        {
            throw new NotImplementedException();
        }

        public virtual void Load()
        {
            throw new NotImplementedException();
        }

        public virtual void Unload()
        {
            throw new NotImplementedException();
        }

        public virtual void Update(GameTime aGameTime)
        {
            throw new NotImplementedException();
        }
    }
}
