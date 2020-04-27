using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class LeftCommand : ICommand
    {
        private readonly Mario context;
        public LeftCommand(Mario Context)
        {
            context = Context;
        }
        public void Execute()
        {
            context.LeftAction();
        }
    }
}
