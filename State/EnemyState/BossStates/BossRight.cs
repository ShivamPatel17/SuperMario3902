using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState.BossStates
{
    class BossRight : AbstractBossState
    {
        public BossRight(Boss context) : base(context)
        {

        }

        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            return new BossRight((Boss)enemy);
        }

        public override void Fire()
        {
            Leave(new BossFire(Boss, this));
            Stage.level.Add(EnemyFactory.Instance.MakeFireball(Boss.Position, 32));
        }

        public override void Enter()
        {

            Boss.Velocity = new Vector2(2f, 0);
            Boss.NeedsUpdating = false;
        }
        public override void Leave(IState next)
        {
            Boss.EnemyState = (AbstractEnemyState)next;
            Boss.EnemyState.Enter();
        }

        public override void ChangeDirections()
        {
            Boss.Flip();
            Leave(new BossLeft(Boss));
        }
    }
}
