using Microsoft.Xna.Framework;
using System;

namespace TechSupportMario.Entity.CollisionZones
{
    public class CollisionZoneEventArgs : EventArgs
    {
        public IEntity Entity { get; set; }
        public Vector2 Position { get; set; }
        public CollisionZoneEventArgs(IEntity entity)
        {
            Entity = entity;
        }

    }
}
