using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class UpCommand : ICommand
    {
        private readonly Mario context;

        public UpCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.UpAction();
        }
    }
}
