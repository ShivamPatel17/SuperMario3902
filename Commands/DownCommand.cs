using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class DownCommand : ICommand
    {
        private readonly Mario context;

        public DownCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.DownAction();
        }
    }
}
