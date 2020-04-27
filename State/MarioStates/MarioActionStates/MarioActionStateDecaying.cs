using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioActionStateDecaying : AbstractMarioActionState
    {
        public MarioActionStateDecaying(Mario context, AbstractMarioActionState previous) : base(context)
        {
            Previous = previous;
        }

        public override void Enter()
        {
            Context.ActionStateEnum = Mario.ActionStateType.Decaying;
            //check if mario should have a Y velocity
            if (Context.OnGround)
            {
                Context.Velocity = new Vector2(Context.Velocity.X, 0);
            }
        }

        public override void ActionUp()
        {
            Leave(new MarioActionStateJump(Context, new MarioActionStateIdle(Context, null)));
        }

        public override void ActionDown()
        {
            Leave(new MarioActionStateCrouch(Context));
        }

        public override void ActionRight()
        {
            if (!(Previous is MarioActionStateMovingRight))
            {
                Context.Velocity = new Vector2(0, Context.Velocity.Y);
            }
            Leave(new MarioActionStateMovingRight(Context));
        }

        public override void ActionLeft()
        {
            if (!(Previous is MarioActionStateMovingLeft))
            {
                Context.Velocity = new Vector2(0, Context.Velocity.Y);
            }
            Leave(new MarioActionStateMovingLeft(Context));
        }

        public override void Leave(IState next)
        {
            Context.ActionState = (AbstractMarioActionState)next;
            Context.ActionState.Enter();
        }
    }
}
