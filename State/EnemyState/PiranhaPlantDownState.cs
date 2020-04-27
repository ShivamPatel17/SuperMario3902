using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class PiranhaPlantDownState : AbstractEnemyState
    {
        public PiranhaPlantDownState(AbstractEnemy context) : base(context)
        {

        }
        public override IEnemyState Clone(AbstractEnemy copy)
        {
            return new PiranhaPlantDownState(copy);
        }

        public override void Enter()
        {
            Context.Velocity = new Microsoft.Xna.Framework.Vector2(0, 1f);
        }

        public override void ChangeDirections()
        {
            Leave(new PiranhaPlantUpState(Context));
        }

        public override void TakeDamage()
        {
            Leave(new PiranhaPlantUpState(Context));
        }

        public override void Leave(IState next)
        {
            Context.EnemyState = (AbstractEnemyState)next;
            Context.EnemyState.Enter();
        }
    }
}
