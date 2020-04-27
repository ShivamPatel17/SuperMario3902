using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates.MarioActionStates;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    class AbstractMarioPowerState : IMarioPowerState
    {

        public Mario Context { get; set; }
        public Mario.PowerStateType Previous { get; set; }
        public AbstractMarioPowerState(Mario context)
        {
            Context = context;
        }
        public virtual void Enter()
        {

        }

        public virtual void Leave(IState next)
        {
            Context.ActionState.Enter();
            Context.PowerState.Enter();
            Context.UpdateTexture();
        }

        public virtual void RecievedFireFlower()
        {

        }


        public virtual void SuperMushroom()
        {

        }

        public virtual void StarPower()
        {

        }

        public virtual void TakeDamage()
        {

        }

        public virtual void ThrowFire()
        {
            
        }

        public virtual void Revive()
        {
            if (Context.PowerStateEnum == Mario.PowerStateType.Dead)
            {
                Context.Invincible = true;
            }
            Context.PowerState = new MarioPowerStateNormal(Context);
            Context.PowerStateEnum = Mario.PowerStateType.Normal;
            Context.ActionState = new MarioActionStateIdle(Context, null);
            Context.ActionStateEnum = Mario.ActionStateType.Idle;
            Context.PowerState.Previous = Mario.PowerStateType.Normal;
            Context.Opacity = 1f;
            Context.ActionState.Enter();
            Context.PowerState.Enter();

            Context.Collidable = true;
        }
    }
}
