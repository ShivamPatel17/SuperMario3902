using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Entity.Items
{
    class FireFlower : AbstractItem
    {
        public FireFlower(Texture2D texture) : base(texture, EntityType.FireFlower)
        {
            MaxHeightChange = 33;
        }

        public override IEntity Clone()
        {
            FireFlower flower = new FireFlower(Texture);
            IEntity clone = flower;
            base.Clone(ref clone);
            flower = (FireFlower)clone;
            return flower;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is Mario)
            {
                Collect = true;
                NeedsUpdating = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (FromBlock)
            {
                Vector2 nextPosition = Position;
                nextPosition.Y += Velocity.Y;
                Position = nextPosition;
                if (Position.Y <= Anchor - MaxHeightChange)
                {
                    Velocity = new Vector2(0, 1.75f);
                    Anchor -= 32;
                }
                else if (Position.Y >= Anchor)
                {
                    Position = new Vector2((int)Position.X, Anchor);
                    Velocity = new Vector2(0, 0);
                    Collidable = true;
                }
                else
                {
                    Position += Velocity;
                }
            }
        }
    }
}
