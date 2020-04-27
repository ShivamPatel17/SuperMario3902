using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class ReleaseUpCommand : ICommand
    {
        private readonly Mario context;

        public ReleaseUpCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.ReleaseUp();
        }
    }
}
