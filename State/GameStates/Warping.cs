
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.Controller;
using TechSupportMario.HeadsUpDisplay;
using TechSupportMario.SoundItems;

namespace TechSupportMario.State.GameStates
{
    class Warping : AbstractGameState
    {
        private int iterations = 50;
        private readonly int next;
        private Vector2 velocity;

        /// <summary>
        /// direction = 1 is down, 2 is right, 3 is up, 4 is left (the direction mario should move to look like he is going through the pipe.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="next"></param>
        /// <param name="direction"></param>
        public Warping(Level level, int next, int direction)
        {
            Context = level;
            this.next = next;
            switch (direction)
            {
                case 1:
                    velocity = new Vector2(0, 2);
                    break;
                case 2:
                    velocity = new Vector2(2, 0);
                    break;
                case 3:
                    velocity = new Vector2(0, -2);
                    break;
                case 4:
                    velocity = new Vector2(-2, 0);
                    break;
                default:
                    velocity = Vector2.Zero;
                    break;
            }
        }

        public override void Enter()
        {
            SoundFactory.Instance.Warp();
            Stage.mario.Order = 1f;
            foreach(IController controller in Stage.controllers)
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
            if (iterations == 0)
            {
                SoundFactory.Instance.CameraChange();
                Context.GoToNewArea(next, camera);
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
            Stage.mario.ActionState = new MarioStates.MarioActionStates.MarioActionStateIdle(Stage.mario, null);
            Collisions.CollisionDetection.SetOnGround(Stage.mario, Context.GetTree());
            Context.State = (IGameState)nextState;
            Context.State.Enter();
        }
    }
}
