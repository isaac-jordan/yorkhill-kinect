using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using kinectApp.Entities;

namespace kinectApp.Entities.Germs
{
    /// <summary>
    /// Base Germ object. Must inherit of this and implement the required methods
    /// </summary>
    public abstract class GermBase : BaseEntity, IGerm
    {
        protected Texture2D _texture;
        protected bool beenToTopHalfOfScreen = false;

        private int iHealth = 0;
        protected DateTime iHitTime = new DateTime();

        protected const int WAITTIME = 2500;
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
        public int Id{ get; set; }

        /// <summary>
        /// Health of the entity
        /// </summary>
        public int Health
        {
            get
            {
                return iHealth;
            }
            set
            {
                var now = DateTime.Now;

                if ((DateTime.Now - iHitTime).Milliseconds > WAITTIME)
                {
                    iHealth = value;
                    iHitTime = now;
                    HasBeenHit = true;
                }
            }
        }

        /// <summary>
        /// Gets if the enemy has been hit recently.
        /// </summary>
        public bool HasBeenHit
        {
            get; set;
        }

        /// <summary>
        /// Gets if the Germ is dead.
        /// </summary>
        public bool IsDead
        {
            get; protected set; 
        }

        public override void Load(ContentManager aContentManager)
        {
            if (!string.IsNullOrEmpty(AssetName))
            {
                Texture = aContentManager.Load<Texture2D>(AssetName);
            }
        }

        public override void Unload()
        {
            if (Texture != null)
            {
                Texture.Dispose();
            }
            Texture = null;
        }

        public abstract override void Update(GameTime aGameTime);
        public abstract override void Draw(SpriteBatch aSpriteBatch);
    }
}
