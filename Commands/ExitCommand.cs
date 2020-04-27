using Microsoft.Xna.Framework;

namespace TechSupportMario
{
    class ExitCommand : ICommand
    {
        private readonly Game Game;
        public ExitCommand(Game game)
        {
            Game = game;
        }

        public void Execute()
        {
            Game.Exit();
        }
    }
}
