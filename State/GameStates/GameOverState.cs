using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.State.GameStates
{
    class GameOverState : AbstractGameState
    {

        private int selection = 1;
        public GameOverState(Level context)
        {
            Context = context;
        }
        public override void Enter()
        {
            Stage.ControllersEnd();
            SoundItems.SoundFactory.Instance.GameOver();
            MediaPlayer.Pause();
            Stage.SelectionControls(this);
        }
        public override void Restart()
        {
            Leave(new Playing(Context));
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
            if (selection < 2)
            {
                selection++;
            }
        }

        public override void Selection(Game1 game)
        {
            if (selection == 1)
            {
                game.Exit();
            }
            else if (selection == 2)
            {
                
                Stage.FullReset();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            ISet<Layer> layers = Context.GetDrawLayers(camera);
            Layer hud = HUDLayerMaker.Instance.GameOverScreen(Stage.player, Context.Clock.Time, camera, selection);
            hud.Order = layers.Count;
            layers.Add(hud);
            foreach (Layer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }

        public override void Leave(IState nextState)
        {
            MediaPlayer.Resume();
            Context.State = (IGameState)nextState;
            Context.State.Enter();
        }
    }
}
