using TechSupportMario.State.GameStates;

namespace TechSupportMario.Commands
{
    class UpSelection : ICommand
    {
        private readonly IGameState gameState;

        public UpSelection(IGameState gameState)
        {
            this.gameState = gameState;
        }

        public void Execute()
        {
            gameState.DecrementSelection();
        }
    }
}
