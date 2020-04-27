using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class SprintCommand : ICommand
    {
        private readonly Mario context;
        public SprintCommand(Mario Context)
        {
            context = Context;
        }
        public void Execute()
        {
            context.GoSprint();
        }
    }
}
