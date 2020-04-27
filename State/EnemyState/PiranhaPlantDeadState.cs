using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class PiranhaPlantDeadState : AbstractEnemyState
    {
        public PiranhaPlantDeadState(AbstractEnemy context) : base(context)
        {

        }

        public override IEnemyState Clone(AbstractEnemy copy)
        {
            return new PiranhaPlantDeadState(copy);
        }

        public override void Enter()
        {
            Context.Velocity = new Microsoft.Xna.Framework.Vector2(0, -2);
            Context.Collidable = false;
        }

        public override void ChangeDirections()
        {
            Context.Velocity = Microsoft.Xna.Framework.Vector2.Zero;
            Context.NeedsUpdating = false;
            Context.Opacity = 0f;
        }
    }
}
