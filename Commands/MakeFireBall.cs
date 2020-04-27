using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class MakeFireBall : ICommand
    {
        private readonly Mario mario;

        public MakeFireBall(Mario mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {
            mario.ActionFireBall();
        }
    }
}
