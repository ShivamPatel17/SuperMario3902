
namespace TechSupportMario.Commands
{
    class RestartCommand : ICommand
    {
        public RestartCommand()
        {

        }
        public void Execute()
        {  
            Stage.FullReset();
        }
    }
}
