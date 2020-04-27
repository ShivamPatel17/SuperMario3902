using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class NormalCommand : ICommand
    {
        private readonly Mario mario;
        public NormalCommand(Mario mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {
            mario.StandardPowerState();
        }
    }
}
