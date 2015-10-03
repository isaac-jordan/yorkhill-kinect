using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kinectApp.Entities
{
    /*
        Handles the changing of Scenes within the game, passing the information to the Entity Manager
    */
    public class SceneManager : IDisposable
    {
        EntityManager iEntityManager;
        ContentManager iContentManager;
        Scene iCurrentScene;

        public SceneManager(ContentManager aContentManager)
        {
            iEntityManager = new EntityManager();
            iContentManager = aContentManager;
        }

        //Clears out the current set of entities of the current scene
        //Then fills the entities for use in the game again.
        public void SetScene(Scene aScene)
        {
            if (iCurrentScene != aScene)
            {
                iCurrentScene = aScene;
            }

            iEntityManager.Unload();
            iEntityManager.Clear();

            foreach (IEntity entity in iCurrentScene.Entities)
            {
                iEntityManager.AddEntity(entity);
            }

            iEntityManager.Load(iContentManager);
        }

        public void UpdateScene(GameTime aGameTime)
        {
            iEntityManager.Update(aGameTime);
        }

        public void DrawScene(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            iEntityManager.Draw(aGameTime, aSpriteBatch);
        }

        public void Dispose()
        {
            iEntityManager.Unload();
            iEntityManager = null;
            iCurrentScene = null;
        }
    }
}
