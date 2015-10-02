using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace kinectApp.Entities.Germs
{
    /*
        A Default Germ, uses IEntity
    */
    public interface IGerm : IEntity
    {
        int Id { get; }
        Vector3 Pos { get; }
        bool IsDead { get; }

        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
    }
}
