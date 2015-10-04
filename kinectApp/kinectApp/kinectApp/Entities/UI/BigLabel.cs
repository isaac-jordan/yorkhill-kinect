using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace kinectApp.Entities.UI
{
    public class BigLabel : BaseEntity
    {
        private string iText;
        private SpriteFont iFont;

        public override int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BigLabel(string aMessage, string aAssetName, float aX, float aY, float aZ) : base(string.Empty, aX, aY, aZ)
        {
            iText = aMessage;
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.DrawString(iFont, iText, new Vector2(Pos.X, Pos.Y), Color.White);
        }

        public override void Load(ContentManager aContentManager)
        {
            iFont = aContentManager.Load<SpriteFont>("UI.LabelText.Big");
        }

        public override void Unload()
        {
            iFont = null;
        }

        //No need for an update of a Label
        public override void Update(GameTime aGameTime)
        {
            return;
        }
    }
}
