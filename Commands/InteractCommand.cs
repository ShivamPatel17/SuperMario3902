
namespace TechSupportMario.Commands
{
    class InteractCommand : ICommand
    {
        public InteractCommand() { }

        public void Execute()
        {
            Stage.level.Interact();
        }
    }
}
