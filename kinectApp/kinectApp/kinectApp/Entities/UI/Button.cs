using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities.UI
{
    public class Button : BaseEnitiy
    {
        private string _Text;
        private SpriteFont _Font;

        const int HEIGHT = 32;
        const int WIDTH = 128;

        public Button(string aText, string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ)
        {
            _Text = aText;
        }

        public override void Load(ContentManager aContentManager)
        {
            Texture = aContentManager.Load<Texture2D>("UI.Button");
            _Font = aContentManager.Load<SpriteFont>("SpriteFont1");
        }

        public override void Unload()
        {
            if (Texture != null)
            {
                Texture.Dispose();
            }
            Texture = null;
        }

        //We have a button. We don't care about updates (YET!)
        public override void Update(GameTime aGameTime)
        {
            return;
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            var x = int.Parse(PosX.ToString());
            var y = int.Parse(PosY.ToString());

            var rect = new Rectangle(x, y, WIDTH, HEIGHT);

            aSpriteBatch.Draw(Texture, rect, Color.White);

            //Format the text position so it's roughly in the center of the button
            var textPos = new Vector2(PosX + (128.0f / 4.0f), PosY);
            aSpriteBatch.DrawString(_Font, _Text, textPos, Color.White);
        }
    }
}
