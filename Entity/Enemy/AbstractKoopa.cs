using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.Enemy
{
    abstract public class AbstractKoopa : AbstractEnemy
    {
        public bool Held { get; set; }
        public AbstractKoopa(Texture2D texture, EntityType type) : base(texture, type)
        {
            SourceRectangle = new Rectangle(new Point(0, 0), new Point(33, 52));
            Held = false;
        }

        public abstract override IEntity Clone();
        public virtual void EnterShell()
        {
            EnemyState.Leave(new KoopaShellState(this));
        }
        public virtual void ExitShell()
        {
            EnemyState.Leave(new EnemyWalkingLeftState(this));
        }
        public virtual void UpdateTextureAndBox()
        {
            
        }
        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if (entity is Mario)
            {
                if (((Mario)entity).PowerStateEnum == Mario.PowerStateType.Star)
                {
                    TakeDamage();
                    SoundFactory.Instance.Stomp();
                    NeedsUpdating = true;
                    Collidable = false;
                }
                if(direction == CollisionDetection.Direction.up)
                {
                    EnterShell();
                    SoundFactory.Instance.Stomp();
                }
                if(EnemyState is KoopaShellState)
                {
                    if (!Held && !((KoopaShellState)EnemyState).GetDeadly())
                    {
                        if (direction == CollisionDetection.Direction.left)
                        {
                            ((KoopaShellState)EnemyState).Right();
                            SoundFactory.Instance.Stomp();
                        }
                        else if (direction == CollisionDetection.Direction.right)
                        {
                            ((KoopaShellState)EnemyState).Left();
                            SoundFactory.Instance.Stomp();
                        }
                    }
                }
            }
            else if (entity is IBlock)
            {
                if (direction == CollisionDetection.Direction.left || direction == CollisionDetection.Direction.right)
                {
                    ChangeDirection();
                    if(EnemyState is KoopaShellState)
                    {
                        ((IBlock)entity).CollisionTransition();
                    }
                }
                else
                {
                    Vector2 vel = Velocity;
                    vel.Y = 0;
                    Velocity = vel;
                    ((IBlock)entity).CollisionTransition();
                }
            }
            else if (entity is AbstractEnemy)
            {
                if(EnemyState is KoopaShellState)
                {
                    ((AbstractEnemy)entity).TakeDamage();
                    SoundFactory.Instance.Stomp();
                }
                else
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
    }
}
