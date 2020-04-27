using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.State.GameStates
{
    class MarioDeadState : AbstractGameState
    {
        private int iterations = 180;
        private Vector2 velocity;

        /// <summary>
        /// direction = 1 is down, 2 is right, 3 is up, 4 is left (the direction mario should move to look like he is going through the pipe.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="next"></param>
        /// <param name="direction"></param>
        public MarioDeadState(Level level)
        {
            Context = level;
            velocity = new Vector2(0, 0);
        }

        public override void Enter()
        {
            Stage.mario.Order = 0f;
            SoundItems.SoundFactory.Instance.Dead();
            MediaPlayer.Pause();
            foreach (IController controller in Stage.controllers)
            {
                controller.ClearDictionary();
            }
        }

        public override void Pause()
        {
            Context.State = new Paused(Context);
            Context.State.Enter();
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            Stage.mario.Position += velocity;
            iterations--;
            if(iterations == 120)
            {
                velocity = new Vector2(0, -3);
            }
            if (iterations == 70)
            {
                velocity = new Vector2(0, 3);
            }
            else if (iterations == 0)
            {
                Leave(new Playing(Context));
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            ISet<Layer> layers = Context.GetDrawLayers(camera);
            Layer hud = HUDLayerMaker.Instance.NormalHud(Stage.player, Context.Clock.Time, camera);
            hud.Order = layers.Count;
            layers.Add(hud);
            foreach (Layer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }

        public override void Leave(IState nextState)
        {
            Stage.ControllersGamePlay();
            Stage.mario.Order = 0f;
            Stage.Reset();
            MediaPlayer.Resume();
            Collisions.CollisionDetection.SetOnGround(Stage.mario, Context.GetTree());
            Context.State = (IGameState)nextState;
            Context.State.Enter();
        }
    }
}
