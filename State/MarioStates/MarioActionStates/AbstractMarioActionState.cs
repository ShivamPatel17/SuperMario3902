using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    class AbstractMarioActionState : IMarioActionState
    {
        
        public Mario Context { get; set; }

        public IMarioActionState Previous { get; set; }

        public AbstractMarioActionState(Mario context)
        {
            Context = context;
        }

        public virtual void ActionDown()
        {
            
        }

        public virtual void ActionLeft()
        {
            
        }

        public virtual void ActionRight()
        {
            
        }

        public virtual void ActionUp()
        {
            
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Leave(IState next)
        {
            
        }
        public virtual void SetPrevious(IMarioActionState newPrevious)
        {
            Previous = newPrevious;
        }
    }
}
