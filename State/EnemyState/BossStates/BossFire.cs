using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState.BossStates
{
    class BossFire : AbstractBossState
    {
        private AbstractBossState bossState;

        public BossFire(Boss context, AbstractBossState previous) : base(context)
        {
            bossState = previous;
        }

        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            return new BossFire((Boss)enemy, (AbstractBossState)bossState.Clone(bossState.Boss));
        }

        public override void Enter()
        {

            Boss.Velocity = new Vector2(0, 0);
            Boss.NeedsUpdating = true;
        }
        public override void Leave(IState next)
        {
            Boss.EnemyState = bossState;
            Boss.EnemyState.Enter();
        }
    }
}
