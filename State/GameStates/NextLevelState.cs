
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.State.GameStates
{
    class NextLevelState : AbstractGameState
    {
        private const int FrameTime = 32;
        private int lastChange = 0;
        private int circle = 0;
        private int selection = 1;
        public NextLevelState(Level context)
        {
            Context = context;
        }

        public override void Enter()
        {

        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            if(lastChange > FrameTime && circle < 11)
            {
                circle++;
                lastChange = 0;
                if (circle == 11)
                {
                    Stage.SelectionControls(this);
                }
            }
            else
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public override void DecrementSelection()
        {
            if (selection > 1)
            {
                selection--;
            }
        }

        public override void IncrementSelection()
        {
            if(selection < 3)
            {
                selection++;
            }
        }

        public override void Selection(Game1 game)
        {
            if(selection == 1)
            {
                Stage.NextLevel();
            }
            else if (selection == 2)
            {
                game.Exit();
            }
            else
            {
                Stage.FullReset();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            ISet<Layer> layers = Context.GetDrawLayers(camera);
            Layer blackCircle = HUDLayerMaker.Instance.BlackBackground(1f, camera, circle, Stage.mario.CollisionBox.Center);
            blackCircle.Order = layers.Count;
            layers.Add(blackCircle);
            if(circle > 10)
            {
                Layer selectionHud = HUDLayerMaker.Instance.WinSelectionScreen(camera, selection);
                selectionHud.Order = layers.Count;
                layers.Add(selectionHud);
            }
            foreach(Layer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }
    }
}
