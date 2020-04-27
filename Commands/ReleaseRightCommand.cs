using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class ReleaseRightCommand : ICommand
    {
        private readonly Mario context;

        public ReleaseRightCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.ReleaseRight();
        }
    }
}
