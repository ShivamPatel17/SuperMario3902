
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;

namespace TechSupportMario.Entity.CollisionZones
{
    public class LockedDoor : WarpZone
    {
        public int LocksLeft { get; set; }
        public List<KeyHoleBlock> KeyHoles { get; }
        public LockedDoor(Rectangle box, int locks, Texture2D texture = null) : base(box, texture)
        {
            LocksLeft = locks;
            KeyHoles = new List<KeyHoleBlock>();
        }

        public override void Interact()
        {
            if(LocksLeft == 0)
            {
                base.Interact();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            base.CollisionResponse(entity, direction);
            if(entity is MarioEntity.Mario)
            {
                while(LocksLeft > 0 && Stage.player.Keys > 0)
                {
                    LocksLeft--;
                    Stage.player.Keys--;
                    KeyHoles[LocksLeft].Used = true;
                    
                }
            }
        }

    }
}
