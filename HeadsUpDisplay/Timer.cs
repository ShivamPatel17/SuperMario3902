using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.HeadsUpDisplay
{
    public class Timer
    {
        public int Time { get; set; }
        //used to calculate time elapsed in the game
        private int timer = 0;

        public Timer()
        {
            Time = 400;
        }

        public void Update(GameTime gameTime)
        {
            timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > 1000)
            {
                timer %= 1000;
                Time -= 1;
            }
            if (Time == 100)
            {
                SoundItems.SoundFactory.Instance.TimeOut();
            }
            if(Time < 0)
            {
                Time = 0;
            }
        }
    }
}
