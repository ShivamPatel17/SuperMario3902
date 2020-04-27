using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.CameraItems;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Background;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.Items;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;
using TechSupportMario.Entity.CollisionZones;
using TechSupportMario.Entity.SpriteMaking;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario
{
    /// <summary>
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera GameCamera { get; }
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
           
            Content.RootDirectory = "Content";
      
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();
            //set up the camera
            GameCamera = new Camera(GraphicsDevice.Viewport)
            {
                Limits = new Rectangle(new Point(0, 0), new Point(6784, 364))
            };
        }

        private void LoadFactories()
        {
            ItemFactory.Instance.LoadFactory(this);
            EnemyFactory.Instance.LoadFactory(this);
            BlockFactory.Instance.LoadFactory(this);
            MarioSpriteFactory.Instance.LoadFactory(this);
            BackgroundFactory.Instance.LoadFactory(this);
            SoundFactory.Instance.LoadFactory(this);
            CollisionZoneFactory.Instance.LoadFactory(this);
            WordAndNumberMaking.Instance.LoadFactory(this);
            HUDLayerMaker.Instance.LoadFactory(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            LoadFactories();
            Stage.InitializeLevel(this);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Stage.Update(gameTime, GameCamera);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Stage.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
