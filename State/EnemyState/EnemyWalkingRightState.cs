using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class EnemyWalkingRightState : AbstractEnemyState
    {
        public EnemyWalkingRightState(AbstractEnemy context) : base(context)
        {

        }

        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            return new EnemyWalkingRightState(enemy);
        }

        public override void Enter()
        {
            Context.Velocity = new Microsoft.Xna.Framework.Vector2(1.5f, 0);
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
            Leave((IState)new EnemyWalkingLeftState(Context));
        }
    }
}
