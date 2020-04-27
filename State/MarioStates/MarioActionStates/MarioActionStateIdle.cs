using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioActionStateIdle : AbstractMarioActionState
    {
        public MarioActionStateIdle(Mario context, AbstractMarioActionState previous) : base(context)
        {
            this.Previous = previous;
        }

        public override void Enter()
        {
            //check if mario needs to decay
            if(Context.Velocity.X != 0)
            {
                Leave(new MarioActionStateDecaying(Context, this));
                return;
            }
            Context.ActionStateEnum = Mario.ActionStateType.Idle;
            Context.Velocity = new Vector2(0f, 0f);
            Context.UpdateTexture();
            Context.NeedsUpdating = true;
        }

        public override void ActionUp()
        {
            Leave(new MarioActionStateJump(Context, this));
        }

        public override void ActionDown()
        {
            Leave(new MarioActionStateCrouch(Context));
        }

        public override void ActionLeft()
        {
            if (Context.SourceRectangle.X == 0)
            {
                Leave(new MarioActionStateMovingLeft(Context));
            }
            else
            {
                Context.ChangeFacing(0);
                Leave(new MarioActionStateMovingLeft(Context));
            }
        }

        public override void ActionRight()
        {
            if (Context.SourceRectangle.X != 0)
            {
                Leave(new MarioActionStateMovingRight(Context));
            }
            else
            {
                Context.ChangeFacing(1);
                Leave(new MarioActionStateMovingRight(Context));
            }
        }

        public override void Leave(IState nextState)
        {
            Context.ActionState = (IMarioActionState)nextState;
            Context.ActionState.Enter();
        }
    }
}
