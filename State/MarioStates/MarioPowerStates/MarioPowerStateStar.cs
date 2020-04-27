using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    class MarioPowerStateStar : AbstractMarioPowerState
    {
        public IMarioPowerState previousState;
        
        public MarioPowerStateStar(Mario context) : base(context)
        {
            previousState = Context.PowerState;
        }

        public override void Enter()
        {
            Context.PowerStateEnum = Mario.PowerStateType.Star;
            Context.NeedsUpdating = true;
            
        }

        public override void Leave(IState next)
        {
            Context.CurrentTexture = 0;
            Context.Texture = MarioSpriteFactory.Instance.Star(0);
            Context.PowerState = previousState;
            Context.PowerState.Enter();
        }

        public void Leave()
        {
            Context.CurrentTexture = 0;
            Context.Texture = MarioSpriteFactory.Instance.Star(0);
            Context.PowerState = previousState;
            Context.PowerState.Enter();
        }

        public override void RecievedFireFlower()
        {
            previousState = new MarioPowerStateFire(Context);
            Previous = Mario.PowerStateType.Fire;
            Context.UpdateTexture();
        }


        public override void SuperMushroom()
        {
            if(!(previousState is MarioPowerStateFire))
            {
                previousState = new MarioPowerStateSuper(Context);
                Previous = Mario.PowerStateType.Super;
                Context.UpdateTexture();
            }
            
        }

        public override void Revive()
        {
            if(!(previousState is MarioPowerStateFire || previousState is MarioPowerStateSuper))
            {
                previousState = new MarioPowerStateNormal(Context);
                Previous = Mario.PowerStateType.Normal;
                Context.UpdateTexture();
            }
        }
    }
}
