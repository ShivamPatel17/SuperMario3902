using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity
{
    public abstract class AbstractEntity : IEntity
    {
        public float MaxVelocity { get; set; } = 2f;
        private Vector2 startingPosition;
        public const int Frame = 120;
        protected const float gravity = 0.13f;
        public const float  DefaultAcc = .08f;
        public float Acceleration { get; set; }
        public Vector2 Velocity { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private Vector2 _position;
        public Vector2 Position { get { return _position; } set { if (startingPosition == null) startingPosition = value; _position = value; } }
        public Rectangle SourceRectangle { get; set; }
        public float Opacity { get; set; }
        public float Order { get; set; }
        public Texture2D Texture { get; set; }
        public bool OnGround { get; set; }
        public bool NeedsUpdating { get; set; }
        public EntityType Type { get; }
        public SpriteEffects SpriteEffect { get; set; }
        public enum EntityType
        {
            Pipe,
            PipeTop,
            PipeMiddle,
            PipeBottom,
            WarpPipe,
            WarpPipeEntity,
            Basic,
            Question,
            Brick,
            Mario,
            Goomba,
            GreenKoopa,
            RedKoop,
            PiranhaPlant,
            DeadEnemy,
            CollisionZone,
            Coin,
            OneUp,
            Super,
            Star,
            FireFlower,
            Fireball,
            Flag, 
            Boo,
            Button,
            Toggle,
            Disappearing,
            Canon,
            BulletBill,
            Key,
            KeyHole,
            Boss,
            EnemyFire
        }
        public virtual Rectangle CollisionBox
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
                if (box.Left > Position.X)
                {
                    box.X--;
                    box.Width++;
                }
                return box;
            }
        }
        public bool Collidable { get; set; }
        public bool Collect { get; set; }

        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y - SourceRectangle.Height), SourceRectangle, (Color.White * Opacity), 0f, Vector2.Zero, 1f, SpriteEffect, Order);
        }

        public virtual void Update(GameTime gameTime)
        {
            Vector2 _velocity = Velocity;
            //gravity
            if (!OnGround)
            {
                _velocity.Y += gravity;
                _velocity.Y = MathHelper.Clamp(_velocity.Y, -9f, 9f);
            }
            _velocity.X += _velocity.X * Acceleration;
            _velocity.X = MathHelper.Clamp(_velocity.X, -MaxVelocity, MaxVelocity);
            Velocity = _velocity;
            Position += Velocity;
        }
        public virtual int TrueWidth()
        {
            return SourceRectangle.Width;
        }

        public virtual int TrueHeight()
        {
            return SourceRectangle.Height;
        }

        protected AbstractEntity(Texture2D texture, EntityType type)
        {
            Texture = texture;
            Collidable = true;
            Collect = false;
            Opacity = 1f;
            SpriteEffect = SpriteEffects.None;
            if(texture != null)
            {
                SourceRectangle = texture.Bounds;
            }
            else
            {
                SourceRectangle = new Rectangle();
            }
            OnGround = true;
            Acceleration = DefaultAcc;
            Type = type;
            Order = .5f;
        }

        public virtual void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            //defualt is to do nothing.
        }

        public virtual int GetTrueWidth()
        {
            return SourceRectangle.Width;
        }

        public virtual int GetTrueHeight()
        {
            return SourceRectangle.Height;
        }

        public abstract IEntity Clone();
        protected virtual void Clone(ref IEntity clone)
        {
            clone.Order = Order;
            clone.Position = new Vector2(Position.X, Position.Y);
            clone.OnGround = OnGround;
            clone.NeedsUpdating = NeedsUpdating;
            clone.Velocity = new Vector2(Velocity.X, Velocity.Y);
            clone.SourceRectangle = new Rectangle(SourceRectangle.Location, SourceRectangle.Size);
            clone.Collidable = Collidable;
        }

        public override bool Equals(object obj)
        {
            if(obj is AbstractEntity entity)
            {
                if (entity.Position.Equals(Position))
                {
                    if (entity.CollisionBox.Equals(CollisionBox))
                    {
                        if (entity.Velocity.Equals(Velocity))
                        {
                            if (entity.OnGround == OnGround)
                            {
                                if(entity.Type == Type)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public virtual void Interact() { }

        public override int GetHashCode()
        {
            return (int)((int)Type * startingPosition.X - 2 * (int)Type * startingPosition.Y);
        }
    }
}
