using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class NormalState : AbstractEnemyState
    {
        public NormalState(AbstractEnemy context) : base (context)
        {

        }
        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            return new NormalState(enemy);
        }
        public override void Enter()
        {
            Context.Velocity = Vector2.Zero;
        }
        public override void Leave(IState state)
        {
            Context.EnemyState = new EnemyStateDead(Context);
            Context.EnemyState.Enter();

        }
    }
}
