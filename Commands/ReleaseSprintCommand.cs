using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class ReleaseSprintCommand : ICommand
    {
        private readonly Mario context;

        public ReleaseSprintCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.ReleaseSprint();
        }
    }
}
