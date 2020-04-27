using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    class MarioPowerStateSuper : AbstractMarioPowerState
    {
        public MarioPowerStateSuper(Mario context) : base(context)
        {

        }

        public override void Enter()
        {
            Context.PowerStateEnum = Mario.PowerStateType.Super;
            Context.UpdateTexture();
            //SoundFactory.Instance.PowerEat();
        }

        public override void Leave(IState nextState)
        {
            ((IMarioPowerState)nextState).Previous = Mario.PowerStateType.Super;
            Context.PowerState = (IMarioPowerState)nextState;
            Context.PowerState.Enter();
            Context.Opacity = 1f;
            if (!(nextState is MarioPowerStateStar))
                Context.Invincible = true;
        }

        public override void TakeDamage()
        {
            Context.Invincible = true;
            Leave(new MarioPowerStateNormal(Context));
        }

        public override void StarPower()
        {
            Leave(new MarioPowerStateStar(Context));
        }

        public override void RecievedFireFlower()
        {
            Leave(new MarioPowerStateFire(Context));
        }
    }
}
