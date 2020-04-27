using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.State.MarioStates;

namespace TechSupportMario.State.EnemyState
{
    class EnemyWalkingLeftState : AbstractEnemyState
    {
        public EnemyWalkingLeftState(AbstractEnemy context) : base(context)
        {

        }

        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            return new EnemyWalkingLeftState(enemy);
        }

        public override void Enter()
        {

            Context.Velocity = new Vector2(-1.5f, 0);
            Context.NeedsUpdating = false;
        }
        public override void Leave(IState next)
        {
            Context.EnemyState = (AbstractEnemyState)next;
            Context.EnemyState.Enter();
        }

        public override void ChangeDirections()
        {
            Context.Flip();
            Leave(new EnemyWalkingRightState(Context));
        }
    }
}
