using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class MarioActionStateDead : AbstractMarioActionState
    {
        public MarioActionStateDead(Mario mario) : base(mario)
        {

        }

        public override void Enter()
        {
            Context.ActionStateEnum = Mario.ActionStateType.Dead;
            Context.Velocity = new Microsoft.Xna.Framework.Vector2(0, 0);
            Context.NeedsUpdating = true;//set to false once we start moving him on death
            Context.Collidable = false;

        }

    }
}
