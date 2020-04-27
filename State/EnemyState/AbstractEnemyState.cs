using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    abstract class AbstractEnemyState : IEnemyState
    {
        public AbstractEnemy Context { get; set; }

        public AbstractEnemyState(AbstractEnemy context)
        {
            Context = context;
        }
        public virtual void Enter()
        {
        }

        public virtual void Leave(IState next)
        {
        }

        public virtual void TakeDamage()
        {
            Leave(new EnemyStateDead(Context));
        }

        public virtual void ChangeDirections()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public abstract IEnemyState Clone(AbstractEnemy enemy);
    }
}
