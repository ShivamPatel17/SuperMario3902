using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates.MarioActionStates;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    class MarioPowerStateDead : AbstractMarioPowerState
    {

        public MarioPowerStateDead(Mario context) : base(context)
        {

        }

        public override void Enter()
        {
            Context.Opacity = .9f;
            Context.ActionState = new MarioActionStateDead(Context);
            Context.ActionState.Enter();
            Context.PowerStateEnum = Mario.PowerStateType.Dead;
            Previous = Mario.PowerStateType.Dead;
            Context.UpdateTexture();
            Context.Collidable = false;
            Context.Velocity = new Microsoft.Xna.Framework.Vector2();
        }

        public override void Leave(IState nextState)
        {
            Context.PowerState = (IMarioPowerState) nextState;
            Context.Opacity = 1f;
            Context.ActionState = new MarioActionStateIdle(Context, null);
            Context.PowerState.Enter();
            Context.ActionState.Enter();
            Context.PowerState.Previous = Context.PowerStateEnum;
            Context.Collidable = true;
        }

        //later sprints probably shouldn't allow this to happen
        public override void SuperMushroom()
        {
            Leave(new MarioPowerStateSuper(Context));
        }

        public override void RecievedFireFlower()
        {
            Leave(new MarioPowerStateFire(Context));
        }
    }
}
