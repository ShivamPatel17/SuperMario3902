using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class DamageCommand : ICommand
    {
        private readonly Mario mario;

        public DamageCommand(Mario mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {
            mario.TakeDamage();
        }
    }
}
