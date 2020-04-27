
using TechSupportMario.State.GameStates;

namespace TechSupportMario.Commands
{
    class MadeSelection : ICommand
    {
        private readonly IGameState state;
        private readonly Game1 game;
        public MadeSelection(Game1 game, IGameState state)
        {
            this.state = state;
            this.game = game;
        }

        public void Execute()
        {
            state.Selection(game);
        }
    }
}
