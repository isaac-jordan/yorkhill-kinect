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
        Scene iCurrentOverlay;

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

                iEntityManager.Unload();
                iEntityManager.Clear();

                foreach (IEntity entity in iCurrentScene.Entities)
                {
                    iEntityManager.AddEntity(entity);
                }

                iEntityManager.Load(iContentManager);
            }
        }

        //Shows an Overlay onto the current game
        //It's technically not an overlay, we just preserve state of the current scene
        //So users can return back where they want to do.
        //I wonder - do people actually read my comments?
        public void ShowOverlay(Scene aOverlay)
        {
            iCurrentOverlay = aOverlay;

            iEntityManager.Clear();

            foreach (IEntity entity in iCurrentOverlay.Entities)
            {
                iEntityManager.AddEntity(entity);
            }

            iEntityManager.Load(iContentManager);
        }

        //Hides the current overlay so that the game can return to normal :)
        public void HideOverlay()
        {
            if (iCurrentOverlay != null)
            {
                iEntityManager.Unload();
                iEntityManager.Clear();

                foreach (var entity in iCurrentScene.Entities)
                {
                    iEntityManager.AddEntity(entity);
                }

                iCurrentOverlay = null;
            }
        }

        public void UpdateScene(GameTime aGameTime)
        {
            iEntityManager.Update(aGameTime);
        }

        public void DrawScene(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            iEntityManager.Draw(aGameTime, aSpriteBatch);
        }

        public SceneDescription GetDescription()
        {
            if (iCurrentOverlay != null)
            {
                return SceneDescription.Overlay;
            }
            else
            {
                if (iCurrentScene is Scenes.Menu)
                {
                    return SceneDescription.Menu;
                }
                else if (iCurrentScene is Scenes.GameInstance)
                {
                    return SceneDescription.Game;
                }
                else
                {
                    return SceneDescription.Unknown;
                }
            }
        }

        public void Dispose()
        {
            iEntityManager.Unload();
            iEntityManager = null;
            iCurrentScene = null;
        }
    }


    public enum SceneType
    {
        Normal = 1,
        Overlay = 2,
    };

    public enum SceneDescription
    {
        Menu = 1,
        Game = 2,
        Pause = 3,
        Unknown = 4,
        Overlay = 5,
    };
}
