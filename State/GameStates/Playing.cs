using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.State.GameStates
{
    public class Playing : AbstractGameState
    {

        public Playing(Level context)
        {
            Context = context;
        }
        public override void Enter()
        {
            Stage.ControllersGamePlay();
        }
        public override void End()
        {
            if(Stage.player.Lives > 0)
            {
                Leave(new Winning(Context));
            }
            else
            {
                Leave(new GameOverState(Context));
            }
        }

        public override void Pause()
        {
            Context.State = new Paused(Context);
            Context.State.Enter();
        }

        public override void Warp(int NextArea, int dir)
        {
            Leave(new Warping(Context, NextArea, dir));
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            Context.Update(gameTime, camera);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            ISet<Layer> layers = Context.GetDrawLayers(camera);
            Layer hud = HUDLayerMaker.Instance.NormalHud(Stage.player, Context.Clock.Time, camera);
            hud.Order = layers.Count;
            layers.Add(hud);
            foreach(Layer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }

        public override void Leave(IState nextState)
        {
            Context.State = (AbstractGameState)nextState;
            Context.State.Enter();
        }
    }
}
