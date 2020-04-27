using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    class MarioPowerStateNormal : AbstractMarioPowerState
    {
        public MarioPowerStateNormal(Mario context) : base(context)
        {

        }

        public override void Enter()
        {
            Context.PowerStateEnum = Mario.PowerStateType.Normal;
            Context.UpdateTexture();
        }

        public override void Leave(IState nextState)
        {
            ((IMarioPowerState)nextState).Previous = Mario.PowerStateType.Normal;
            Context.PowerState = (IMarioPowerState) nextState;
            Context.PowerState.Enter();
            //for cheat code
            Context.Opacity = 1f;
            if (!(nextState is MarioPowerStateStar))
                Context.Invincible = true;
        }

        public override void SuperMushroom()
        {
            Leave(new MarioPowerStateSuper(Context));
        }

        public override void StarPower()
        {
            Leave(new MarioPowerStateStar(Context));
        }

        public override void TakeDamage()
        {
            Leave(new MarioPowerStateDead(Context));
        }

        public override void RecievedFireFlower()
        {
            Leave(new MarioPowerStateFire(Context));
        }
    }
}
