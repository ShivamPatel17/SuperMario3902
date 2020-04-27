using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.Enemy
{
    class PiranhaPlant : AbstractEnemy
    {
        private readonly IBlock cameFrom;
        private Vector2 anchor;
        private int lastUpdate;

        public PiranhaPlant(Texture2D texture, IBlock parent): base(texture, EntityType.PiranhaPlant)
        {
            cameFrom = parent;
            Position = new Vector2(parent.Position.X + parent.TrueWidth() / 4, parent.Position.Y);
            anchor = Position;
            Order = 1f;
            Collidable = true;
            lastUpdate = 0;
            if(parent is EntityWarpBlock warp)
            {
                warp.Shot = this;
            }
            SourceRectangle = new Rectangle(new Point(0, 0), new Point(texture.Width / 2, texture.Height));
        }

        public override IEntity Clone()
        {
            PiranhaPlant plant = new PiranhaPlant(Texture, cameFrom);
            IEntity clone = plant;
            base.Clone(ref clone);
            plant = (PiranhaPlant)clone;
            return plant;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity.Type == EntityType.Fireball || (entity is Mario && ((Mario)entity).PowerStateEnum == Mario.PowerStateType.Star))
            {
                Collect = true;
                cameFrom.NeedsUpdating = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (EnemyState is PiranhaPlantDownState)
            {
                if (Position.Y >= anchor.Y)
                {
                    Velocity = Vector2.Zero;
                    Collidable = false;
                }
            }
            else
            {
                if(Position.Y <= anchor.Y - SourceRectangle.Height)
                {
                    EnemyState = new PiranhaPlantDownState(this);
                    EnemyState.Enter();
                }
            }
            Position += Velocity;
            if (!Velocity.Equals(Vector2.Zero))//then it isn't hidden
            {
                if(Frame <= lastUpdate)
                {
                    if (SourceRectangle.X != 0)
                    {
                        SourceRectangle = new Rectangle(new Point(0, 0), SourceRectangle.Size);
                    }
                    else
                    {
                        SourceRectangle = new Rectangle(new Point(Texture.Width / 2, 0), SourceRectangle.Size);
                    }
                    lastUpdate = 0;
                }
                else
                {
                    lastUpdate += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else
            {
                if(SourceRectangle.X != 0)
                {
                    SourceRectangle = new Rectangle(new Point(0, 0), SourceRectangle.Size);
                }
            }
        }
    }
}
