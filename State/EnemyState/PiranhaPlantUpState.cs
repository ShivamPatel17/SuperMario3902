using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class PiranhaPlantUpState : AbstractEnemyState
    {
        public PiranhaPlantUpState(AbstractEnemy context):base(context)
        {
            
        }

        public override IEnemyState Clone(AbstractEnemy copy)
        {
            return new PiranhaPlantUpState(copy);
        }

        public override void Enter()
        {
            Context.Velocity = new Microsoft.Xna.Framework.Vector2(0, -1f);
            Context.Collidable = true;
        }

        public override void TakeDamage()
        {
            Leave(new PiranhaPlantDeadState(Context));
        }
    }
}
