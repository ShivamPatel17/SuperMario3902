namespace TechSupportMario.State
{
    /// <summary>
    /// Filler interface for IEntity
    /// </summary>
    public interface IState
    {
        void Enter();
        void Leave(IState nextState);
    }
}