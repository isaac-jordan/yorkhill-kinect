using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities
{
    public abstract class BaseEnitiy : IEntity
    {
        private Vector3 _pos;

        #region Constructors
        public BaseEnitiy() : this(string.Empty, 0, 0, 0) { }

        public BaseEnitiy(string aAssetName) : this(aAssetName, 0, 0, 0) { }

        public BaseEnitiy(string aAssetName, Vector3 aPos) : this(aAssetName, aPos.X, aPos.Y, aPos.Z) { }

        public BaseEnitiy(string aAssetName, float aX, float aY, float aZ)
        {
            _pos = new Vector3(aX, aY, aZ);
            AssetName = aAssetName;
        }
        #endregion

        /// <summary>
        /// Gets or Sets the texture of the entity
        /// </summary>
        public Texture2D Texture
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the asset name of the entity
        /// </summary>
        public string AssetName
        {
            get; private set;
        }

        #region Position Code
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

        #endregion

        public abstract void Draw(SpriteBatch aSpriteBatch);
        public abstract void Update(GameTime aGameTime);

        public abstract void Load(ContentManager aContentManager);
        public abstract void Unload();
    }
}
