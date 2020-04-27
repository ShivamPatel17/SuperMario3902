using Microsoft.Xna.Framework;
using TechSupportMario.Entity;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioActionStateSprintRight : AbstractMarioActionState
    {        
        public MarioActionStateSprintRight(Mario context) : base(context)
        {
            
        }

        public override void Enter()
        {
            Context.MaxVelocity = 5f;
            Context.ActionStateEnum = Mario.ActionStateType.WalkingRight;
            Context.UpdateTexture();
            if (Context.Velocity.X <= 0f)
            {
                Context.Velocity = new Vector2(.3f, 0);
            }
            else
            {
                Context.Velocity = new Vector2(2f, 0);
            }
            Context.NeedsUpdating = false;
            Context.Acceleration = AbstractEntity.DefaultAcc;
        }

        public override void ActionUp()
        {
            Leave(new MarioActionStateJump(Context, new MarioActionStateMovingRight(Context)));
        }

        public override void ActionDown()
        {
            Leave(new MarioActionStateCrouch(Context));
        }

        public override void ActionLeft()
        {
            Leave(new MarioActionStateDecaying(Context, new MarioActionStateMovingRight(Context)));
        }

        public override void Leave(IState nextState)
        {
            Context.MaxVelocity = 2f; 
            Context.ActionState = (IMarioActionState) nextState;
            Context.CurrentFrame = 0;
            Rectangle rectangle = Context.SourceRectangle;
            rectangle.X = MarioSpriteFactory.RightFacingStart;
            Context.SourceRectangle = rectangle;
            Context.ActionState.Enter();
        }
    }
}
