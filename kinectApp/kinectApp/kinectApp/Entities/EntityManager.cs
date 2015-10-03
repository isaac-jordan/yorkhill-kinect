using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace kinectApp.Entities
{
    public class EntityManager
    {
        List<IEntity> _entities;

        public EntityManager()
        {
            _entities = new List<IEntity>();
        }

        /// <summary>
        /// Adds a new Entity to the Game
        /// </summary>
        /// <param name="aEntity"></param>
        public void AddEntity(IEntity aEntity)
        {
            if (!_entities.Contains(aEntity))
            {
                _entities.Add(aEntity);
            }
        }

        /// <summary>
        /// Removes the specified entity from the Game
        /// </summary>
        /// <param name="aEntity"></param>
        public void RemoveEntity(IEntity aEntity)
        {
            if (_entities.Contains(aEntity))
            {
                _entities.Remove(aEntity);
            }
        }

        /// <summary>
        /// Returns if the manager has the specified entity
        /// </summary>
        /// <param name="aEntity"></param>
        /// <returns></returns>
        public bool HasEntity(IEntity aEntity)
        {
            return _entities.Contains(aEntity);
        }


        #region Game Logic
        public void Update(GameTime aGameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(aGameTime);
            }

            if (_entities.Count < 5)
            {
                _entities.Add(Entities.Germs.GermFactory.CreateSmallGerm());
            }
        }

        public void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(aSpriteBatch);
            }
        }

        public void Unload()
        {
            #if DEBUG
                Console.WriteLine("<Disposing Entities> : {0}", _entities.Count);
            #endif

            foreach (var entity in _entities)
            {
                entity.Unload();
            }
        }

        public void Load(ContentManager aContentManager)
        {
            #if DEBUG
                Console.WriteLine("<Loading Entities> : {0}", _entities.Count);
            #endif

            foreach (var entity in _entities)
            {
                entity.Load(aContentManager);
            }
        }
        #endregion
     }
}
