using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class FireMarioCommand : ICommand
    {
        private readonly Mario context;

        public FireMarioCommand(Mario mario)
        {
            this.context = mario;
        }

        public void Execute()
        {
            context.ActionFireFlower();
        }
    }
}
