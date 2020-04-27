using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class ReleaseLeftCommand : ICommand
    {

        private readonly Mario context;

        public ReleaseLeftCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.ReleaseLeft();
        }
    }
}
