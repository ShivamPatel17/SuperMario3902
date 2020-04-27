using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace TechSupportMario.SoundItems
{
    class SoundFactory
    {
        private static SoundFactory instance;

        private Song level1;
        private Song underground;
        private Song won;
        private Song ghost;

        private SoundEffect brickBump;
        private SoundEffect brickDes;
        private SoundEffect powerShow;
        private SoundEffect powerEat;
        private SoundEffect upEat;
        private SoundEffect shootFire;
        private SoundEffect stomp;
        private SoundEffect coin;
        private SoundEffect dead;
        private SoundEffect jumpN;
        private SoundEffect warp;
        private SoundEffect cameraChange;
        private SoundEffect timeOut;
        private SoundEffect gameOver;
        private SoundEffect pipe;
        
        private SoundFactory()
        {
        }

        public void LoadFactory(Game game)
        {
            level1 = game.Content.Load<Song>("Sound/normal_overworld_song");
            underground = game.Content.Load<Song>("Sound/normal_underworld_song");
            brickBump = game.Content.Load<SoundEffect>("Sound/Effect/smb_bump");
            brickDes = game.Content.Load<SoundEffect>("sound/Effect/smb_breakblock");
            powerShow = game.Content.Load<SoundEffect>("sound/Effect/smb_powerup_appears");
            powerEat = game.Content.Load<SoundEffect>("sound/Effect/smb_powerup");
            upEat = game.Content.Load<SoundEffect>("sound/Effect/smb_1-up");
            shootFire = game.Content.Load<SoundEffect>("sound/Effect/smb_fireball");
            stomp = game.Content.Load<SoundEffect>("sound/Effect/shell-hit");
            coin = game.Content.Load<SoundEffect>("sound/Effect/coin");
            dead = game.Content.Load<SoundEffect>("sound/Effect/player-died");
            jumpN = game.Content.Load<SoundEffect>("sound/Effect/player-jump");
            warp = game.Content.Load<SoundEffect>("sound/Effect/warp");
            cameraChange = game.Content.Load<SoundEffect>("sound/Effect/camera-change");
            timeOut = game.Content.Load<SoundEffect>("sound/Effect/smb_warning");
            gameOver = game.Content.Load<SoundEffect>("sound/Effect/smb_gameover");
            pipe = game.Content.Load<SoundEffect>("sound/Effect/smb_pipe");
            won = game.Content.Load<Song>("sound/Effect/level-win");
            ghost = game.Content.Load<Song>("sound/ghost_house");
        }

        public static SoundFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundFactory();
                }
                return instance;

            }
        }

        public void PlaySong(int song)
        {
            switch (song)
            {
                case 1:
                    FirstLevel();
                    break;
                case 2:
                    GhostSong();
                    break;
                case 3:
                    Underground();
                    break;
            }
        }

        public Sound FirstLevel()
        {
            return new Sound(level1, true);
        }

        public Sound Underground()
        {
            return new Sound(underground, true);
        }

        public SoundEvent BrickBump()
        {
            return new SoundEvent(brickBump);
        }

        public SoundEvent BrickDes()
        {
            return new SoundEvent(brickDes);
        }

        public SoundEvent PowerShow()
        {
            return new SoundEvent(powerShow);
        }
        public SoundEvent PowerEat()
        {
            return new SoundEvent(powerEat);
        }
        public SoundEvent UpEat()
        {
            return new SoundEvent(upEat);
        }
        public SoundEvent ShootFire()
        {
            return new SoundEvent(shootFire);
        }
        public SoundEvent Stomp()
        {
            return new SoundEvent(stomp);
        }
        public SoundEvent Dead()
        {
            return new SoundEvent(dead);
        }
        public SoundEvent Coin()
        {
            return new SoundEvent(coin);
        }
        public SoundEvent JumpN()
        {
            return new SoundEvent(jumpN);
        }
        public SoundEvent Warp()
        {
            return new SoundEvent(warp);
        }
        public SoundEvent CameraChange()
        {
            return new SoundEvent(cameraChange);
        }
        public SoundEvent TimeOut()
        {
            return new SoundEvent(timeOut);
        }
        public SoundEvent GameOver()
        {
            return new SoundEvent(gameOver);
        }
        public SoundEvent Pipe()
        {
            return new SoundEvent(pipe);
        }
        public Sound Won()
        {
            return new Sound(won, false);
        }
        public Sound GhostSong()
        {
            return new Sound(ghost, true);
        }
    }
}
