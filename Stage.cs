using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.CameraItems;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Block.BlockEventArgs;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.LevelLoader;
using TechSupportMario.SoundItems;
using TechSupportMario.Entity.CollisionZones;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TechSupportMario.Controller;
using TechSupportMario.State.GameStates;

namespace TechSupportMario
{
    static class Stage
    {
        public static Player player;
        public static Mario mario;
        public static Level level;
        public static Sound sound;
        public static List<IController> controllers;
        private static Camera camera;
        private static Game1 theGame;
        private static int currentLevel;
        
        

        public static void InitializeLevel(Game1 game)
        {
            currentLevel = 1;
            sound = SoundFactory.Instance.FirstLevel();
            //create mario/player
            player = new Player
            {
                PlayerMario = new Mario(game.Content.Load<Texture2D>("mario/mario_sprites"))
            };
            mario = player.PlayerMario;
            mario.RaiseCollisionEvent += player.HandleMarioCollision;
            mario.RaiseInstantDeathEvent += player.HandleInstantDeathEvent;
            KeyboardController keyboard = new KeyboardController();
            GamepadController gamepad = new GamepadController();
            Loader.LoadLevel(game.GameCamera, "LevelLoader/World1-1.txt");

            controllers = new List<IController>
            {
                keyboard,
                gamepad
            };
            
            theGame = game;
            camera = game.GameCamera;
            level.SetUpLevel();
            Rectangle limits = level.GetTree().Bounds;
            limits.Height -= 16;
            camera.Limits = limits;
            level.RaiseLevelEvent += mario.HandleLevelEvent;
            level.State = new Playing(level);
            level.State.Enter();
        }

        public static void Reset()
        {
            level.ResetLevel();
            mario.Reset();
            camera.LookAt(mario.Position);
            player.Reset();
            sound.Reset();
            level.State.Start();
        }

        public static void AddLayer(Layer layer)
        {
            level.AddLayer(layer);
        }

        public static void AddEntity(IEntity entity)
        {
            level.Add(entity);
        }

        public static void Update(GameTime gameTime, Camera camera)
        {
            level.State.Update(gameTime, camera);
            foreach(IController controller in controllers)
            {
                controller.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            level.State.Draw(spriteBatch, camera);
        }

        public static void HandleWarp(object sender, WarpEventArgs eventArgs)
        {
            mario.ReleaseDirectionLock();
            level.State.Warp(eventArgs.NextArea, eventArgs.Direction);
        }

        public static void HandleCheckpoint(object sender, CollisionZoneEventArgs eventArgs)
        {
            level.SaveLevelState((IEntity)sender, eventArgs);
            player.SaveState();
        }

        public static void HandleGameEnd(object sender, CollisionZoneEventArgs eventArgs)
        {
            level.State.End();
        }

        /// <summary>
        /// this resets the game completely, like quiting the processes and restarting it.
        /// </summary>
        public static void FullReset()
        {
            currentLevel = 1;
            sound = SoundFactory.Instance.FirstLevel();
            //create mario/player
            player = new Player
            {
                PlayerMario = new Mario(theGame.Content.Load<Texture2D>("mario/mario_sprites"))
            };
            mario = player.PlayerMario;
            //mario.RaiseCollisionEvent += player.HandleMarioCollision;
            //mario.RaiseInstantDeathEvent += player.HandleInstantDeathEvent;
            KeyboardController keyboard = new KeyboardController();
            GamepadController gamepad = new GamepadController();
            Loader.LoadLevel(camera, "LevelLoader/World1-1.txt");
            level.SetUpLevel();
            level.RaiseLevelEvent += mario.HandleLevelEvent;
            level.State = new Playing(level);
            level.State.Enter();
            Rectangle limits = level.GetTree().Bounds;
            limits.Height -= 16;
            camera.Limits = limits;
        }

        public static void FullClear()
        {
            for(int i = 0; i < controllers.Count; i++)
            {
                controllers[i].FullClear();
            }

            sound = SoundFactory.Instance.FirstLevel();
            //create mario/player
            //mario.RaiseCollisionEvent += player.HandleMarioCollision;
           // mario.RaiseInstantDeathEvent += player.HandleInstantDeathEvent;
            KeyboardController keyboard = new KeyboardController();
            GamepadController gamepad = new GamepadController();
            Loader.LoadLevel(camera, "LevelLoader/World1-2.txt");
            level.SetUpLevel();
            level.RaiseLevelEvent += mario.HandleLevelEvent;
            level.State = new Playing(level);
            level.State.Enter();
        }

        public static void NextLevel()
        {
            if (currentLevel == 1)
            {
                currentLevel = 2;
                //create mario/player
                for (int i = 0; i < controllers.Count; i++)
                {
                    controllers[i].FullClear();
                }
                //mario.RaiseCollisionEvent += player.HandleMarioCollision;
                //mario.RaiseInstantDeathEvent += player.HandleInstantDeathEvent;
                KeyboardController keyboard = new KeyboardController();
                GamepadController gamepad = new GamepadController();

                Loader.LoadLevel(camera, "LevelLoader/World1-2.txt");
                level.SetUpLevel();
                level.RaiseLevelEvent += mario.HandleLevelEvent;
                level.State = new Playing(level);
                level.State.Enter();
                camera.Limits = level.GetTree().Bounds;
            }
            else
            {
                FullReset();
            }
            
        }

        public static void ControllersGamePlay()
        {
            ControllerMaker.InitializeControllersForGameplay(theGame, (KeyboardController)controllers[0], (GamepadController)controllers[1]);
        }
        public static void SelectionControls(IGameState state)
        {
            ControllerMaker.PauseControllers(theGame, (KeyboardController)controllers[0], (GamepadController)controllers[1], state);
        }
        public static void ControllersEnd()
        {
            ControllerMaker.GameEndControllers(theGame, (KeyboardController)controllers[0], (GamepadController)controllers[1]);
        }
    }
}
