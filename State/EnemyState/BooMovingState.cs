using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class BooMovingState : AbstractEnemyState
    {
        public BooMovingState(AbstractEnemy context) : base(context)
        {

        }
        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            return new BooMovingState(enemy);
        }
        public override void Enter()
        {
            Context.Velocity = new Vector2(-0.7f, 0);
            //Context.Collidable = false;
        }
        public override void Leave(IState state)
        {
            Context.EnemyState = new EnemyStateDead(Context);
            Context.EnemyState.Enter();

        }

    }
}
