using Microsoft.Xna.Framework;
using System;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity
{
    public interface IEntity : ISprite
    {
        Rectangle SourceRectangle { get; set; }//this is the size and position of the sprite in the spirit sheet
        int Rows { get; set; }
        int Columns { get; set; }
        Vector2 Velocity { get; set; }
        bool Collect { get; set; }
        /// <summary>
        /// If OnGround is true that means there is some type of block directly below the entity. If this is true
        /// then the entity shouldn't have a downward acceleration from gravity applied to the entity.
        /// </summary>
        bool OnGround { get; set; }
        void CollisionResponse(IEntity entity, CollisionDetection.Direction direction);
        AbstractEntity.EntityType Type { get; }
        int TrueWidth();
        int TrueHeight();
        IEntity Clone();
        void Interact();
    }
}
