using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TechSupportMario.Entity.Background
{
    class BackgroundFactory
    {
        private static BackgroundFactory instance;
        private Texture2D CloudSpriteSheet;
        private Texture2D BigCloudSpriteSheet;
        private Texture2D BushSpriteSheet;
        private Texture2D MountainSpriteSheet;
        private Texture2D MountainRangeSpriteSheet;
        private Texture2D Underground1;
        private Queue<Texture2D> Underground;
        private Texture2D CastlePillars;
        private Texture2D CastleWindows;
        private Texture2D CastleWalls;

        private BackgroundFactory()
        {
        }
        public void LoadFactory(Game game)
        {
            CloudSpriteSheet = game.Content.Load<Texture2D>("Background/Cloud");
            BigCloudSpriteSheet = game.Content.Load<Texture2D>("Background/Cloud_Big");
            BushSpriteSheet = game.Content.Load<Texture2D>("Background/bush");
            MountainSpriteSheet = game.Content.Load<Texture2D>("Background/mount_lightgreen_front");
            MountainRangeSpriteSheet = game.Content.Load<Texture2D>("Background/mountain_range");
            Underground1 = game.Content.Load<Texture2D>("Background/undergroundbackground1");
            Underground = new Queue<Texture2D>();
            Texture2D temp = game.Content.Load<Texture2D>("Background/undergroundbackground2");
            Underground.Enqueue(temp);
            temp = game.Content.Load<Texture2D>("Background/undergroundbackground3");
            Underground.Enqueue(temp);
            CastlePillars = game.Content.Load<Texture2D>("Background/pillar_background");
            CastleWindows = game.Content.Load<Texture2D>("Background/castle_background");
            CastleWalls = game.Content.Load<Texture2D>("Background/background_window");
        }

        public static BackgroundFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BackgroundFactory();
                }
                return instance;

            }
        }
        public ISprite Cloud()
        {
            return new Sprite(CloudSpriteSheet);
        }
        public ISprite BigCloud()
        {
            return new Sprite(BigCloudSpriteSheet);
        }
        public ISprite Bush()
        {
            return new Sprite(BushSpriteSheet);
        }
        public ISprite Mountain()
        {
            return new Sprite(MountainSpriteSheet);
        }
        public ISprite MountainRange()
        {
            return new Sprite(MountainRangeSpriteSheet);
        }

        public ISprite CastleWindowsSprite()
        {
            return new Sprite(CastleWindows);
        }

        public ISprite CastlePillarSprite()
        {
            return new Sprite(CastlePillars);
        }
        public ISprite CastleNormal()
        {
            return new Sprite(CastleWalls);
        }
        public ISprite UndergroundPreset()
        {
            return new Changing_background(Underground1, Underground);
        }
    }
}
