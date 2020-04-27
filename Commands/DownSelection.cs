using TechSupportMario.State.GameStates;

namespace TechSupportMario.Commands
{
    class DownSelection : ICommand
    {
        private readonly IGameState gameState;

        public DownSelection(IGameState gameState)
        {
            this.gameState = gameState;
        }

        public void Execute()
        {
            gameState.IncrementSelection();
        }
    }
}
