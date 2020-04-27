using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioHoldingShellState : AbstractMarioActionState
    {
        private AbstractKoopa _koopa;
        public MarioHoldingShellState(Mario context, AbstractKoopa koopa, IMarioActionState state) : base(context)
        {
            Previous = state;
            koopa.Held = true;
            _koopa = koopa;
            koopa.Collidable = false;
        }
        public override void Enter()
        {
            Context.ActionStateEnum = Mario.ActionStateType.HoldingShell;
            Context.UpdateTexture();
            Context.Velocity = new Vector2(0, 0);
            Context.NeedsUpdating = true;
        }
        public override void ActionUp()
        {

        }

        public override void ActionDown()
        {

        }

        public override void ActionLeft()
        {
        }

        public override void ActionRight()
        {
        }

        public void Throw()
        {
            Leave(Previous);
            _koopa.Held = false;
            _koopa.Velocity = new Vector2(0, -5f);
        }
        public override void Leave(IState next)
        {
            Context.ActionState = Previous;
            Context.ActionState.Enter();
        }

        public void UpdateState()
        {
            _koopa.Velocity = Context.Velocity;
        }

    }
}
