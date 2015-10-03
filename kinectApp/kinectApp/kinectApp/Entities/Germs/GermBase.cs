using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using kinectApp.Entities;

namespace kinectApp.Entities.Germs
{
    /// <summary>
    /// Base Germ object. Must inherit of this and implement the required methods
    /// </summary>
    public abstract class GermBase : BaseEnitiy, IGerm
    {
        protected Texture2D _texture;

        /// <summary>
        /// Create new Germ with position 0,0,0
        /// </summary>
        public GermBase() : base() { }

        /// <summary>
        /// Create a new Germ with a asset name.
        /// </summary>
        /// <param name="aAssetName"></param>
        public GermBase(string aAssetName) : base(aAssetName) { }

        /// <summary>
        /// Create a new Germ with the specified a position
        /// </summary>
        /// <param name="aPos">A Position</param>
        public GermBase(string aAssetName, Vector3 aPos) : base(aAssetName, aPos) { }
        /// <summary>
        /// Create a new Germ with a Custom position
        /// </summary>
        /// <param name="aX">X Pos</param>
        /// <param name="aY">Y Pos</param>
        /// <param name="aZ">Z Pos</param>
        public GermBase(string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ) { }

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

        public override void Load(ContentManager aContentManager)
        {
            if (!string.IsNullOrEmpty(AssetName))
            {
                Texture = aContentManager.Load<Texture2D>(AssetName);
            }
        }

        public abstract override void Unload();

        public abstract override void Update(GameTime aGameTime);
    }
}
