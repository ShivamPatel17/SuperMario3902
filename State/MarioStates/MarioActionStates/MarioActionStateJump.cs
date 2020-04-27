using Microsoft.Xna.Framework;
using TechSupportMario.Entity;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioActionStateJump : AbstractMarioActionState
    {
        public MarioActionStateJump(Mario context, IMarioActionState state) : base(context)
        {
            Previous = state;
            
        }

        public override void Enter()
        {
            Context.ActionStateEnum = Mario.ActionStateType.Jumping;
            Context.UpdateTexture();
            if (!Context.Falling)
            {
                Vector2 vel = Context.Velocity;
                vel.Y = -6.4f;
                Context.Velocity = vel;
            }
            if (!(Previous is MarioActionStateMovingLeft || Previous is MarioActionStateMovingRight))
            {
                Context.Acceleration = 0;
            }
            Context.NeedsUpdating = false;
        }

        public override void ActionDown()
        {
            if (Context.OnGround)
            {
                Leave(Previous);
            }
            else
            {
                Vector2 vel = Context.Velocity;
                vel.Y = 0f;
                Context.NeedsUpdating = true;
                Context.Velocity = vel;
                Context.SourceRectangle = MarioSpriteFactory.FallingJump(Context.SourceRectangle, Context.ActionStateEnum);
            }
        }

        public override void ActionLeft()
        {
            
            if (Previous is MarioActionStateIdle || Previous is MarioActionStateDecaying)
            {
                Context.ChangeFacing(0);
                Previous = new MarioActionStateMovingLeft(Context);
                Vector2 vel = Context.Velocity;
                vel.X = -.3f;
                Context.Velocity = vel;
                Context.Acceleration = AbstractEntity.DefaultAcc;
            }
            else
            {
                Previous = new MarioActionStateDecaying(Context, this);
                Context.Acceleration = 0f;
            }
            
        }

        public override void ActionRight()
        {

            if (Previous is MarioActionStateIdle || Previous is MarioActionStateDecaying)
            {
                Context.ChangeFacing(1);
                Previous = new MarioActionStateMovingRight(Context);
                Vector2 vel = Context.Velocity;
                vel.X = .3f;
                Context.Velocity = vel;
                Context.Acceleration = AbstractEntity.DefaultAcc;
            }
            else
            {
                Previous = new MarioActionStateDecaying(Context, this);
                Context.Acceleration = 0f;
            }
        }

        public override void Leave(IState next)
        {
            Context.Falling = false;
            Context.Acceleration = AbstractEntity.DefaultAcc;
            Context.ActionState = Previous;
            Context.ActionState.Enter();
        }
    }
}
