using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class SuperCommand : ICommand
    {
        private readonly Mario mario;

        public SuperCommand(Mario mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {
            mario.SuperMushroomAction(true);
        }
    }
}
