using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kinectApp.Entities
{
    public interface IScene
    {
        List<IEntity> Entities { get; }
        string Name { get; }

        void Load(EntityManager aManager);
    }



    /*
        Defines a list of entities for the Entity Manager to load in when required.
        Would define parts of game - Menu / Game / HighScores / GameOver etc
    */
    public class Scene
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
    }
}