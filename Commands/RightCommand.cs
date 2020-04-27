using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class RightCommand : ICommand
    {
        private readonly Mario context;
        public RightCommand(Mario Context)
        {
            context = Context;
        }
        public void Execute()
        {
            context.RightAction();
        }
    }
}
