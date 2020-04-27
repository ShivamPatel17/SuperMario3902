using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.MarioEntity
{
    class MarioSpriteFactory
    {
        //pixel positions in sprite sheet
        public const int RightFacingStart = 300;//x position
        private const int NormalMarioStart = 0;
        private const int SuperMarioStart = 230;
        private const int FireMarioStart = 540;
        //starting positions for running
        private const int RunningNormal = 40;
        private const int RunningSuper = 286;
        private const int RunningFire = 596;
        //starting positions for holding a shell
        private const int ShellNormal = 80;
        private const int ShellSuper = 342;
        private const int ShellFire = 652;
        //throwing fire ball y
        private const int FireBallThrow = 648;
        //walking starting positions relative to the start x
        private const int NormalFrame2 = 28;
        private const int NormalFrame3 = 56;
        private const int SuperFrame2 = 32;
        private const int SuperFrame3 = 64;
        //crouch starting positions y
        private const int NormalMarioCrouch = 120;
        private const int SuperMarioCrouch = 398;
        private const int FireMarioCrouch = 709;
        //jump starting positions y
        private const int NormalMarioJump = 148;
        private const int SuperMarioJump = 428;
        private const int FireMarioJump = 738;
        private const int FallingStart = 32; //x position
        //dead starting position y
        private const int DeadMarioStart = 857;

        //crouch heights and widths
        private const int CrouchHeightNormal = 28;
        private const int CrouchHeightSuper = 30;
        private const int CrouchWidthNormal = 30;
        private const int CrouchWidthSuper = 32;
        //idle heights and widths
        private const int IdleHeightNormal = 40;
        private const int IdleWidthNormal = 28;
        private const int IdleHeightSuper = 56;
        private const int IdleWidthSuper = 30;
        //jump height and widths
        private const int JumpWidth = 32;
        private const int JumpHeightSuper = 62;
        private const int JumpHeightNormal = 44;
        private const int FallingHeightNormal = 40;
        private const int FallingHeightSuper = 58;
        //walking second state dim
        private const int WalkingSuperWidth = 32;
        //running third state dim
        private const int RunningNormalWidth = 30;
        private const int RunningNormalWidth3 = 32;
        private const int RunningNormalHeight = 40;
        private const int RunningNormalHeight3 = 38;
        private const int RunningSuperWidth = 36;
        private const int RunningSuperHeight = 56;
        private const int RunningSuperHeight3 = 54;
        //shell dims
        private const int ShellNormalWidth = 30;
        private const int ShellNormalHeight = 40;
        private const int ShellNormalHeight3 = 38;
        private const int ShellSuperWidth = 32;
        private const int ShellSuperHeight = 56;
        private const int ShellSuperHeight3 = 54;
        //dead dims
        private const int DeadHeight = 41;
        private const int DeadWidth = 34;
        //win peace sign positions and widths
        private const int winYPos = 897;
        private const int winWidth = 32;
        private const int normalWinHeight = 42;
        private const int superWinHeight = 56;

        //textures for star mario
        private readonly Texture2D[] textures;
        //texture for fireballs
        private Texture2D fireball;

        //instance for this factory
        private static MarioSpriteFactory _instance;

        private MarioSpriteFactory()
        {
            textures = new Texture2D[4];
        }

        public static MarioSpriteFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MarioSpriteFactory();
                }
                return _instance;
            }
        }

        public void LoadFactory(Game game)
        {
            textures[0] = game.Content.Load<Texture2D>("mario/mario_sprites");
            textures[1] = game.Content.Load<Texture2D>("mario/super2");
            textures[2] = game.Content.Load<Texture2D>("mario/super3");
            textures[3] = game.Content.Load<Texture2D>("mario/super4");
            fireball = game.Content.Load<Texture2D>("mario/fireball");
        }

        public static void SetSourceRectangle(ref Rectangle source, Mario.PowerStateType power, Mario.ActionStateType action)
        {
            Point location = new Point();
            Point size = new Point();
            if(source.X >= RightFacingStart)
            {
                location.X = RightFacingStart;
            }
            else
            {
                location.X = 0;
            }
            //set the location to the beginning of the 'set' of sprites for each power state
            switch (power)
            {
                case Mario.PowerStateType.Normal:
                    location.Y = NormalMarioStart;
                    size.X = IdleWidthNormal;
                    size.Y = IdleHeightNormal;
                    break;
                case Mario.PowerStateType.Super:
                    location.Y = SuperMarioStart;
                    size.X = IdleWidthSuper;
                    size.Y = IdleHeightSuper;
                    break;
                case Mario.PowerStateType.Fire:
                    location.Y = FireMarioStart;
                    size.X = IdleWidthSuper;
                    size.Y = IdleHeightSuper;
                    break;
                case Mario.PowerStateType.Dead:
                    location.Y = DeadMarioStart;
                    location.X = 0;
                    size.X = DeadWidth;
                    size.Y = DeadHeight;
                    break;
            }

            switch (action)
            {
                case Mario.ActionStateType.WalkingLeft:
                    if (location.Y > SuperMarioStart)
                    {
                        size.X = IdleWidthSuper;
                    }
                    break;
                case Mario.ActionStateType.WalkingRight:
                    location.X = RightFacingStart;
                    if (location.Y > SuperMarioStart)
                    {
                        size.X = IdleWidthSuper;
                    }
                    break;
                case Mario.ActionStateType.Jumping:
                    switch (power)
                    {
                        case Mario.PowerStateType.Normal:
                            location.Y = NormalMarioJump;
                            size.Y = JumpHeightNormal;
                            break;
                        case Mario.PowerStateType.Super:
                            location.Y = SuperMarioJump;
                            size.Y = JumpHeightSuper;
                            break;
                        case Mario.PowerStateType.Fire:
                            location.Y = FireMarioJump;
                            size.Y = JumpHeightSuper;
                            break;
                    }
                    size.X = JumpWidth;
                    break;
                case Mario.ActionStateType.Crouching:
                    switch (power)
                    {
                        case Mario.PowerStateType.Normal:
                            location.Y = NormalMarioCrouch;
                            size.Y = CrouchHeightNormal;
                            size.X = CrouchWidthNormal;
                            break;
                        case Mario.PowerStateType.Super:
                            location.Y = SuperMarioCrouch;
                            size.Y = CrouchHeightSuper;
                            size.X = CrouchWidthSuper;
                            break;
                        case Mario.PowerStateType.Fire:
                            location.Y = FireMarioCrouch;
                            size.Y = CrouchHeightSuper;
                            size.X = CrouchWidthSuper;
                            break;
                    }
                    break;
                case Mario.ActionStateType.HoldingShell:
                    switch (power)
                    {
                        case Mario.PowerStateType.Normal:
                            location.Y = ShellNormal;
                            size.Y = ShellNormalHeight;
                            size.X = ShellNormalWidth;
                            break;
                        case Mario.PowerStateType.Super:
                            location.Y = ShellSuper;
                            size.Y = ShellSuperHeight;
                            size.X = ShellNormalWidth;
                            break;
                        case Mario.PowerStateType.Fire:
                            location.Y = ShellFire;
                            size.Y = ShellSuperHeight;
                            size.X = ShellSuperWidth;
                            break;
                    }
                    break;
            }

            source.Location = location;
            source.Size = size;
        }

        public static void GetFireThrow(ref Rectangle source)
        {
            source.Width = IdleWidthSuper;
            source.Height = IdleHeightSuper;
            source.Y = FireBallThrow;
            if (source.X >= RightFacingStart)
            {
                source.X = RightFacingStart;
            }
            else
            {
                source.X = 0;
            }
        }

        public static void Walking(ref Rectangle source, int frame, Mario.PowerStateType power, Mario.ActionStateType walkingDir)
        {
                if(frame == 0)
                {
                    if (power != Mario.PowerStateType.Normal)
                    {
                        source.Width = WalkingSuperWidth;
                        source.X = 0;
                        source.Y = SuperMarioStart;
                        if (power == Mario.PowerStateType.Fire)
                        {
                            source.Y = FireMarioStart;
                        }
                    }
                    else
                    {
                        source.Width = IdleWidthNormal;
                        source.X = 0;
                        source.Y = NormalMarioStart;
                    }
                }
                else if (frame == 1)
                {
                    if (power != Mario.PowerStateType.Normal)
                    {
                        source.Width = WalkingSuperWidth;
                        source.X = SuperFrame2;
                        source.Y = SuperMarioStart;
                        if (power == Mario.PowerStateType.Fire)
                        {
                            source.Y = FireMarioStart;
                        }
                    }
                    else
                    {
                        source.Width = IdleWidthNormal;
                        source.X = NormalFrame2;
                        source.Y = NormalMarioStart;
                    }
                }
                else
                {
                    if (power != Mario.PowerStateType.Normal)
                    {
                        source.Width = WalkingSuperWidth;
                        source.X = SuperFrame3;
                        source.Y = SuperMarioStart;
                        if (power == Mario.PowerStateType.Fire)
                        {
                            source.Y = FireMarioStart;
                        }
                    }
                    else
                    {
                        source.Width = IdleWidthNormal;
                        source.X = NormalFrame3;
                        source.Y = NormalMarioStart;
                    }
                }
                if (walkingDir == Mario.ActionStateType.WalkingRight)
                {
                    source.X += RightFacingStart;
                }
            
        }

        public static void Running(ref Rectangle source, int frame, Mario.PowerStateType power, Mario.ActionStateType walkingDir)
        {
            if (frame == 0)
            {
                if (power != Mario.PowerStateType.Normal)
                {
                    source.Width = RunningSuperWidth;
                    source.X = 0;
                    source.Height = RunningSuperHeight;
                    if (power == Mario.PowerStateType.Fire)
                    {
                        source.Y = RunningFire;
                    }
                    else
                    {
                        source.Y = RunningSuper;
                    }
                }
                else
                {
                    source.Width = RunningNormalWidth;
                    source.X = 0;
                    source.Y = RunningNormal;
                    source.Height = RunningNormalHeight;
                }
            }
            else if (frame == 1)
            {
                if (power != Mario.PowerStateType.Normal)
                {
                    source.Width = RunningNormalWidth;
                    source.X = RunningSuperWidth;
                    source.Y = RunningSuper;
                    source.Height = RunningSuperHeight;
                    if (power == Mario.PowerStateType.Fire)
                    {
                        source.Y = RunningFire;
                    }
                }
                else
                {
                    source.Width = RunningNormalWidth;
                    source.X = RunningNormalWidth;
                    source.Y = RunningNormal;
                    source.Height = RunningNormalHeight;
                }
            }
            else
            {
                if (power != Mario.PowerStateType.Normal)
                {
                    source.Width = WalkingSuperWidth;
                    source.X = 2 * RunningSuperWidth;
                    source.Y = RunningSuper;
                    source.Height = RunningSuperHeight3;
                    if (power == Mario.PowerStateType.Fire)
                    {
                        source.Y = FireMarioStart;
                    }
                }
                else
                {
                    source.Width = RunningNormalWidth3;
                    source.X = RunningNormalWidth * 2;
                    source.Y = RunningNormal;
                    source.Height = RunningNormalHeight3;
                }
            }
            if (walkingDir == Mario.ActionStateType.WalkingRight)
            {
                source.X += RightFacingStart;
            }
        }

        public static void ShellWalking(ref Rectangle source, int frame, Mario.PowerStateType power, Mario.ActionStateType walkingDir)
        {
            if (frame == 0)
            {
                if (power != Mario.PowerStateType.Normal)
                {
                    source.Width = ShellSuperWidth;
                    source.X = 0;
                    source.Height = ShellSuperHeight;
                    if (power == Mario.PowerStateType.Fire)
                    {
                        source.Y = ShellFire;
                    }
                    else
                    {
                        source.Y = ShellSuper;
                    }
                }
                else
                {
                    source.Width = ShellNormalWidth;
                    source.X = 0;
                    source.Y = ShellNormal;
                    source.Height = ShellNormalHeight;
                }
            }
            else if (frame == 1)
            {
                if (power != Mario.PowerStateType.Normal)
                {
                    source.Width = ShellNormalWidth;
                    source.X = ShellNormalWidth;
                    source.Y = ShellSuper;
                    source.Height = RunningSuperHeight;
                    if (power == Mario.PowerStateType.Fire)
                    {
                        source.Y = ShellFire;
                    }
                }
                else
                {
                    source.Width = ShellNormalWidth;
                    source.X = ShellNormalWidth;
                    source.Y = ShellNormal;
                    source.Height = ShellNormalHeight;
                }
            }
            else
            {
                if (power != Mario.PowerStateType.Normal)
                {
                    source.Width = ShellSuperWidth;
                    source.X = 2 * ShellSuperWidth;
                    source.Y = ShellSuper;
                    source.Height = ShellSuperHeight3;
                    if (power == Mario.PowerStateType.Fire)
                    {
                        source.Y = ShellFire;
                    }
                }
                else
                {
                    source.Width = ShellNormalWidth;
                    source.X = ShellNormalWidth * 2;
                    source.Y = ShellNormal;
                    source.Height = ShellNormalHeight3;
                }
            }
            if (walkingDir == Mario.ActionStateType.WalkingRight)
            {
                source.X += RightFacingStart;
            }

        }
        /// <summary>
        /// To easily get the right animation/texture for falling mario with a single call.
        /// You need to get set mario's source rectangle equal to the rectangle you pass in.
        /// To do this:
        /// Rectangle source = SourceRectangle;
        /// MarioSpriteFactory.FallingJump(ref source);
        /// SourceRectangle = source;
        /// </summary>
        /// <param name="source"></param>
        public static Rectangle FallingJump(Rectangle source, Mario.ActionStateType action)
        {
            if(source.X >= RightFacingStart)
            {
                source.X = FallingStart + RightFacingStart;
            }
            else
            {
                source.X = FallingStart;
            }

            if (source.Y < SuperMarioStart)
            {
                source.Y = NormalMarioJump;
                source.Height = FallingHeightNormal;
            }
            else if (source.Y < FireMarioStart)
            {
                source.Y = SuperMarioJump;
                source.Height = FallingHeightSuper;
            }
            else
            {
                source.Y = FireMarioJump;
                source.Height = FallingHeightSuper;
            }
            return source;
        }

        public Rectangle Winning(Mario.PowerStateType power)
        {
            Rectangle source = new Rectangle(new Point(0, winYPos), new Point(winWidth, normalWinHeight));
            if(power != Mario.PowerStateType.Normal)
            {
                source.Height = superWinHeight;
            }
            return source;
        }

        public Texture2D Star(int currentText)
        {
            return textures[currentText];
        }

        public Fireball MakeFireball(Vector2 position, int dir)
        {
            bool facing = dir >= RightFacingStart;
            
            Vector2 pos;
            if (facing)
            {
                pos = new Vector2(position.X + IdleWidthSuper + 2, position.Y - 30);
            }
            else
            {
                pos = new Vector2(position.X - fireball.Width - 1, position.Y - 30);
            }
            Fireball fb = new Fireball(fireball, facing)
            {
                Position = pos
            };
            Rectangle source = fb.SourceRectangle;
            source.Width /= 2;
            fb.SourceRectangle = source;
            return fb;
        }
    }
}
