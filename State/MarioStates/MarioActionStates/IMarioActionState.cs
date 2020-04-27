namespace TechSupportMario.State.MarioStates.MarioActionStates
{
    public interface IMarioActionState : IState
    {
        void ActionLeft();
        void ActionRight();
        void ActionUp();
        void ActionDown();
        void SetPrevious(IMarioActionState previous);
        IMarioActionState Previous { get; set; }
    }
}