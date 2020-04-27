using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class StarCommand : ICommand
    {
        private readonly Mario mario;
        public StarCommand(Mario mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {
            mario.StarAction();
        }
    }
}
