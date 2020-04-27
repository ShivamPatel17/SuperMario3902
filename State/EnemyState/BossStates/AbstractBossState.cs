using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState.BossStates
{
    abstract class AbstractBossState : AbstractEnemyState
    {
        public Boss Boss { get; set; }

        protected AbstractBossState(Boss boss) : base(null)
        {
            Boss = boss;
        }

        public virtual void Fire()
        {

        }

        public override void TakeDamage()
        {
            Boss.Lives--;
            if(Boss.Lives == 0)
            {
                Stage.level.State.End();
            }
            else
            {
                Leave(new BossHit(Boss, this));
            }
        }
    }
}
