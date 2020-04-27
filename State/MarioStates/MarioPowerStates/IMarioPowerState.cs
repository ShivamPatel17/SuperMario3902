using Microsoft.Xna.Framework;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioPowerStates
{
    public interface IMarioPowerState : IState
    {
        Mario.PowerStateType Previous { get; set; }
        void TakeDamage();
        void RecievedFireFlower();
        void SuperMushroom();
        void StarPower();
        void Revive();
        void ThrowFire();
    }
}