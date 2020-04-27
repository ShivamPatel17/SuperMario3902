using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class ReleaseDownCommand : ICommand
    {
        private readonly Mario context;

        public ReleaseDownCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.ReleaseDown();
        }
    }
}
