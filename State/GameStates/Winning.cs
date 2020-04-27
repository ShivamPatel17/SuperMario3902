using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.State.GameStates
{
    class Winning : AbstractGameState
    {
        private int extraPoints;
        private float opacity;
        public Winning(Level context)
        {
            Context = context;
        }
        public override void Enter()
        {
            Stage.ControllersEnd();
            extraPoints = (Stage.level.Clock.Time * 50);
            int height = (int)Stage.mario.Position.Y;
            Stage.player.Score += (3000 - (10 * height));
            if (height <= 70) Stage.player.Lives++;
            SoundItems.SoundFactory.Instance.Won();
            opacity = 0;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            if (extraPoints > 0)
            {
                if (opacity < 1f)
                {
                    opacity += .02f;
                }
                else if (extraPoints >= 100)
                {
                    extraPoints -= 100;
                    Stage.player.Score += 100;
                }
                else if (extraPoints >= 10)
                {
                    extraPoints -= 10;
                    Stage.player.Score += 10;
                }
                else if (extraPoints >= 1)
                {
                    extraPoints -= 1;
                    Stage.player.Score += 1;
                }
                if (Stage.mario.Position.Y >= 352 && Stage.mario.Velocity.Y != 0)
                {
                    Stage.mario.Position = new Vector2(Stage.mario.Position.X, 352);
                    Stage.mario.OnGround = true;
                    Stage.mario.ActionState.ActionDown();
                    Stage.mario.ActionState = new MarioStates.MarioActionStates.MarioActionStateMovingRight(Stage.mario);
                    Stage.mario.ActionState.Enter();
                }
                Stage.mario.Update(gameTime);
            }
            else
            {
                if(opacity >= 0)
                {
                    opacity -= .05f;
                }
                Stage.mario.SourceRectangle = MarioSpriteFactory.Instance.Winning(Stage.mario.PowerStateEnum);
                System.Diagnostics.Debug.WriteLine(Stage.mario.Position);
                if(opacity < 0)
                {
                    Leave(new NextLevelState(Context));
                }
            }
            
            
            
            
            if(Stage.mario.Position.X > 6340)
            {
                Stage.mario.Position = new Vector2(6340, 352);
            }
            camera.LookAt(Stage.mario.Position);
        }

        public override void Leave(IState nextState)
        {
            Context.State = (IGameState)nextState;
            Context.State.Enter();
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            ISet<Layer> layers = Context.GetDrawLayers(camera);
            Layer hud = HUDLayerMaker.Instance.WinScreen(Stage.player, Context.Clock.Time, camera, extraPoints);
            Layer blackBackground = HUDLayerMaker.Instance.BlackBackground(opacity, camera);
            blackBackground.Order = layers.Count;
            layers.Add(blackBackground);
            hud.Order = layers.Count;
            //hud.Sprites.Add(Stage.mario);
            Layer marioLayer = new Layer(camera)
            {
                Order = hud.Order + 1
            };
            marioLayer.Sprites.Add(Stage.mario);
            layers.Add(hud);
            layers.Add(marioLayer);
            foreach (Layer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }
    }
}
