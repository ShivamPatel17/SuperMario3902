using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioActionStateCrouch : AbstractMarioActionState
    {

        public MarioActionStateCrouch(Mario mario) : base(mario) { }

        public override void Enter()
        {
            Context.ActionStateEnum = Mario.ActionStateType.Crouching;
            Context.Velocity = new Vector2(0, .1f);
            Context.UpdateTexture();
            Context.NeedsUpdating = true;
        }

        public override void ActionUp()
        {
            Leave(new MarioActionStateIdle(Context, this));
        }

        public override void ActionLeft()
        {
            Context.ChangeFacing(0);
        }

        public override void ActionRight()
        {
            Context.ChangeFacing(1);
        }

        public override void Leave(IState next)
        {
            Context.ReleaseDirectionLock();
            Context.ActionState = (AbstractMarioActionState) next;
            Context.ActionState.Enter();//not much to do here right now
        }
    }
}
