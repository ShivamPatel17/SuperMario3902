using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Entity.Items
{
    class Key : AbstractItem
    {
        public static int Count = 0;
        public int KeyNum;
        public Key(Texture2D texture) : base(texture, EntityType.Key)
        {
            MaxHeightChange = 33;
            Count++;
            KeyNum = Count;
        }

        public override IEntity Clone()
        {
            Key key = new Key(Texture);
            IEntity clone = key;
            base.Clone(ref clone);
            key = (Key)clone;
            return key;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is Mario)
            {
                Collect = true;
                NeedsUpdating = true;
                Collidable = false;
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
