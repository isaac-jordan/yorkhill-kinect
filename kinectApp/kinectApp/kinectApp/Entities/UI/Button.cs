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

        public Button(string aText, string aAssetName, float aX, float aY, float aZ) : base(aAssetName, aX, aY, aZ)
        {
            _Text = aText;
        }

        public override void Load(ContentManager aContentManager)
        {
            Texture = aContentManager.Load<Texture2D>("UI.Button");
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
    }
}
