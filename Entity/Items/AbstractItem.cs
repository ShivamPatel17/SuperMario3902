using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;

namespace TechSupportMario.Entity.Items
{
    abstract class AbstractItem : AbstractEntity, IItem
    {
        public int Anchor { get; set; }
        public bool FromBlock{get; set;}

        public int Direction{get; set;}
        public int MaxHeightChange{get; set;}

        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = SourceRectangle;
                box.Location = new Point((int)Position.X, (int)Position.Y - SourceRectangle.Height);
                if (box.Bottom < Position.Y)
                {
                    box.Height++;
                }
                if (box.Right < Position.X + box.Width)
                {
                    box.Width++;
                }
                if (box.X > Position.X)
                {
                    box.X--;
                    box.Width++;
                }
                return box;
            }
        }

        public AbstractItem(Texture2D texture, EntityType type) : base(texture, type)
        {
            SourceRectangle = texture.Bounds;
            Opacity = 1f;
            Acceleration = 0f;
        }

        public virtual void  ChangeDirection()
        {
            Direction *= -1;
            Vector2 temp = Velocity;
            temp.X *= -1;
            Velocity = temp;
        }

        public virtual void BounceFromBlock(Vector2 position)
        {
            FromBlock = true;
            Anchor = (int)position.Y;
            Position = new Vector2(position.X+2, position.Y - Texture.Height);
            Velocity = new Vector2(0, -0.25f);
            Collidable = false;
            Stage.AddEntity(this);
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if (entity is Mario)
            {
                if (this is OneUpMushroom)
                {
                    SoundFactory.Instance.UpEat();
                }
                else if (this is SuperMushroom)
                {
                    SoundFactory.Instance.PowerEat();
                }
                else if (this is Coin)
                {
                    SoundFactory.Instance.Coin();
                }
                Collect = true;         
            }
            else
            {
                if (direction == CollisionDetection.Direction.right || direction == CollisionDetection.Direction.left)
                {
                    ChangeDirection();
                }
                else
                {
                    Vector2 vel = Velocity;
                    vel.Y = 0;
                    Velocity = vel;
                }
            }
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public abstract override IEntity Clone();

        protected override void Clone(ref IEntity clone)
        {
            base.Clone(ref clone);
            ((AbstractItem)clone).Anchor = Anchor;
            ((AbstractItem)clone).FromBlock = FromBlock;
            ((AbstractItem)clone).Direction = Direction;
            ((AbstractItem)clone).MaxHeightChange = MaxHeightChange;
        }

    }
}
