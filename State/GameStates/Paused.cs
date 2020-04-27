
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.State.GameStates
{
    class Paused : AbstractGameState
    {
        private bool wasMuted;
        private int selection;

        public Paused(Level context)
        {
            Context = context;
            selection = 1;
        }

        public override void Start()
        {
            Leave(new Playing(Context));
        }

        public override void Enter()
        {
            wasMuted = MediaPlayer.IsMuted;
            MediaPlayer.IsMuted = true;
            Stage.SelectionControls(this);
        }

        public override void Leave(IState state)
        {
            MediaPlayer.IsMuted = wasMuted;
            Context.State = (IGameState)state;
            Context.State.Enter();
        }

        public override void DecrementSelection()
        {
            if(selection > 1)
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
            if (selection == 1)
            {
                Leave(new Playing(Context));
            }
            else if (selection == 2)
            {
                game.Exit();
            }
            else
            {
                Stage.Reset();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            ISet<Layer> layers = Context.GetDrawLayers(camera);
            Layer hud = HUDLayerMaker.Instance.PauseScreen(Stage.player, Context.Clock.Time, camera, selection);
            hud.Order = layers.Count;
            layers.Add(hud);
            foreach (Layer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }
    }
}
