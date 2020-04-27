using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    class MarioPowerStateFire : AbstractMarioPowerState
    {
        public MarioPowerStateFire(Mario mario) : base(mario) { }
        public override void Enter()
        {
            Context.PowerStateEnum = Mario.PowerStateType.Fire;
            Context.UpdateTexture();
        }

        public override void Leave(IState nextState)
        {
            ((IMarioPowerState)nextState).Previous = Mario.PowerStateType.Fire;
            Context.PowerState = (IMarioPowerState) nextState;
            Context.PowerState.Enter();
            if(!(nextState is MarioPowerStateStar))
                Context.Invincible = true;
        }

        public override void StarPower()
        {
            Leave(new MarioPowerStateStar(Context));

        }
        public override void TakeDamage()
        {
            Context.Invincible = true;
            Leave(new MarioPowerStateSuper(Context));
        }

        public override void ThrowFire()
        {
            SoundFactory.Instance.ShootFire();
            Rectangle source = Context.SourceRectangle;
            MarioSpriteFactory.GetFireThrow(ref source);
            Context.SourceRectangle = source;
            Stage.AddEntity(MarioSpriteFactory.Instance.MakeFireball(Context.Position, Context.SourceRectangle.X));
            Context.ShotFireBall = true;
        }
    }
}
