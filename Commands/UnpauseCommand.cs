
namespace TechSupportMario.Commands
{
    class UnpauseCommand : ICommand
    {
        public UnpauseCommand()
        {

        }
        public void Execute()
        {
            Stage.level.State.Start();
        }
    }
}
