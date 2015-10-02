﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

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
        }

        public void Draw(GameTime aGameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(aGameTime);
            }
        }

        public void Unload()
        {
            foreach (var entity in _entities)
            {
                entity.Unload();
            }
        }

        public void Load()
        {
            foreach (var entity in _entities)
            {
                entity.Load();
            }
        }
        #endregion
     }
}
