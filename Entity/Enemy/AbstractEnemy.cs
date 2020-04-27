using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.EnemyState;
using TechSupportMario.SoundItems;
using System;
using System.Diagnostics;
using TechSupportMario.State.EnemyState.BossStates;

namespace TechSupportMario.Entity.Enemy
{
    /// <summary>
    /// common ground for all enemy types
    /// </summary>
    public abstract class AbstractEnemy : AbstractEntity
    {
        private int currentYFrame;
        private int lastFrameTime;
        // This is for the enemy state
        public IEnemyState EnemyState { get; set; }
        public AbstractEnemy DeadEnemy { get; set; }

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
                if(box.X > Position.X)
                {
                    box.X--;
                    box.Width++;
                }
                return box;
            }
        }

        protected AbstractEnemy(Texture2D texture, EntityType type) : base(texture, type)
        {
            currentYFrame = 0;
            lastFrameTime = 0;
            Rows = 1;
            Columns = 2;

            if (type == EntityType.Boo)
                EnemyState = new BooMovingState(this);
            else if (type != EntityType.PiranhaPlant)
                EnemyState = new EnemyWalkingLeftState(this);
            else if (type == EntityType.Canon)
                EnemyState = new NormalState(this);
            else if (type == EntityType.PiranhaPlant)
                EnemyState = new PiranhaPlantUpState(this);
            EnemyState.Enter();
        }

        public void ChangeDirection()
        {
            EnemyState.ChangeDirections();
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void TakeDamage()
        {
            EnemyState.TakeDamage();
        }

        public void SetSourcePosition(Point p)
        {
            Rectangle source = SourceRectangle;
            source.Location = p;
            SourceRectangle = source;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if (entity is Mario)
            {
                if (direction == CollisionDetection.Direction.up || ((Mario)entity).PowerStateEnum == Mario.PowerStateType.Star)
                {
                    
                    SoundFactory.Instance.Stomp();
                    NeedsUpdating = true;
                    Collidable = false;
                    TakeDamage();
                }
            }
            else if (entity is IBlock)
            {
                if (direction == CollisionDetection.Direction.left || direction == CollisionDetection.Direction.right)
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
            else if (entity is AbstractEnemy)
            {
                if (direction == CollisionDetection.Direction.left && Velocity.X < 0)
                {
                    ChangeDirection();
                }
                else if (direction == CollisionDetection.Direction.right && Velocity.X > 0)
                {
                    ChangeDirection();
                }
            }
            else if (entity is Fireball)
            {
                TakeDamage();
                SoundFactory.Instance.Stomp();
                NeedsUpdating = true;
                Collidable = false;
                OnRaiseFireballEvent(null);
            }
        }

        public virtual void Flip()
        {
            if (EnemyState is EnemyWalkingLeftState || EnemyState is BossLeft)
            {
                currentYFrame = 1;
            }
            else
            {
                currentYFrame = 0;
            }
            Rectangle source = SourceRectangle;
            source.Y = currentYFrame * SourceRectangle.Size.Y;
            SourceRectangle = source;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (DeadEnemy == null)
            {
                base.Draw(spriteBatch);
            }
            else
            {
                DeadEnemy.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(EnemyState is KoopaShellState)
            {
                UpdateKoopaShell(gameTime);
            }
            else if (!(EnemyState is EnemyStateDead))
            {
                base.Update(gameTime);
                if (OnGround)
                {
                    Vector2 vel = new Vector2(Velocity.X, 0);
                    Velocity = vel;
                }
                lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (lastFrameTime >= Frame)
                {
                    lastFrameTime = 0;
                    Rectangle source = SourceRectangle;

                    if (source.X != 0)
                    {
                        source.X = 0;
                    }
                    else
                    {
                        source.X += SourceRectangle.Width;
                    }
                    SourceRectangle = source;
                }
            }
            else
            {
                EnemyState.Update(gameTime);
            }
        }
        private void UpdateKoopaShell(GameTime gameTime)
        {
            base.Update(gameTime);
            if (OnGround)
            {
                Vector2 vel = new Vector2(Velocity.X, 0);
                Velocity = vel;
            }
            if (Velocity.X != 0 && !((AbstractKoopa)this).Held)
            {
                lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (lastFrameTime >= Frame)
                {
                    lastFrameTime = 0;
                    Rectangle source = SourceRectangle;

                    if (source.X != 0)
                    {
                        source.X = 0;
                    }
                    else
                    {
                        source.X += SourceRectangle.Width;
                    }
                    SourceRectangle = source;
                }
            }
            EnemyState.Update(gameTime);
            if (((KoopaShellState)EnemyState).GetSwap())
            {
                ((AbstractKoopa)this).ExitShell();
            }
        }
        protected override void Clone(ref IEntity entity) 
        {
            base.Clone(ref entity);
            if (EnemyState != null)
            {
                ((AbstractEnemy)entity).EnemyState = EnemyState.Clone((AbstractEnemy)entity);
                ((AbstractEnemy)entity).EnemyState.Enter();
            }
            if( DeadEnemy != null)
            {
                ((AbstractEnemy)entity).DeadEnemy = (AbstractEnemy)DeadEnemy.Clone();
            }
        }

        //events
        protected virtual void OnRaiseFireballEvent(EventArgs e)
        {
            RaiseFireballEvent?.Invoke(this, e);
        }
        public delegate void FireballEventHandler(object sender, EventArgs e);
        public event EventHandler<EventArgs> RaiseFireballEvent;
    }
}
