using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kinectApp.Utilities;

namespace kinectApp.Entities
{
    public interface IScene
    {
        List<IEntity> Entities { get; }
        string Name { get; }

        void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager);
    }



    /*
        Defines a list of entities for the Entity Manager to load in when required.
        Would define parts of game - Menu / Game / HighScores / GameOver etc
    */
    public abstract class Scene : IScene
    {
        private string _name;
        private List<IEntity> _entites;

        public Scene(string aName)
        {
            _name = aName;
            _entites = new List<IEntity>();
        }

        public List<IEntity> Entities { get { return _entites; } }
        public string Name { get { return _name; } }

        public abstract void HandleKeys(InputHelper aInputHelper, ISceneManager aSceneManager);
    }
}