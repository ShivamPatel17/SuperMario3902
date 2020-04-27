using Microsoft.Xna.Framework.Input;
using TechSupportMario.Commands;
using TechSupportMario.State.GameStates;

namespace TechSupportMario.Controller
{
    static class ControllerMaker
    {

        public static void InitializeControllersForGameplay(Game1 game, KeyboardController keyboard, GamepadController gamepad)
        {
            keyboard.ClearDictionary();
            gamepad.ClearDictionary();
            //Background sound
            ICommand mutecommand = new MuteCommand(Stage.sound);
            keyboard.Add((int)Keys.M, mutecommand);
            //Add the Reset command to the r
            ICommand resetcommand = new ResetCommand();
            keyboard.Add((int)Keys.R, resetcommand);
            gamepad.Add((int)Buttons.Back, resetcommand);
            //Add the Exit Command to the controllers
            ICommand exitcommand = new ExitCommand(game);
            keyboard.Add((int)Keys.Q, exitcommand);
            gamepad.Add((int)Buttons.Start, exitcommand);
            //add the left command
            ICommand leftcommand = new LeftCommand(Stage.mario);
            keyboard.Add((int)Keys.A, leftcommand);
            keyboard.Add((int)Keys.Left, leftcommand);
            gamepad.Add((int)Buttons.DPadLeft, leftcommand);
            //add the right commmand
            ICommand rightcommand = new RightCommand(Stage.mario);
            keyboard.Add((int)Keys.D, rightcommand);
            keyboard.Add((int)Keys.Right, rightcommand);
            gamepad.Add((int)Buttons.DPadRight, rightcommand);
            //Add the srpint command
            ICommand sprintcommand = new SprintCommand(Stage.mario);
            keyboard.Add((int)Keys.LeftShift, sprintcommand);
            keyboard.Add((int)Keys.RightShift, sprintcommand);
            //add the down command
            ICommand downcommand = new DownCommand(Stage.mario);
            keyboard.Add((int)Keys.S, downcommand);
            keyboard.Add((int)Keys.Down, downcommand);
            gamepad.Add((int)Buttons.DPadDown, downcommand);
            //add the up command
            ICommand upcommand = new UpCommand(Stage.mario);
            keyboard.Add((int)Keys.W, upcommand);
            keyboard.Add((int)Keys.Up, upcommand);
            gamepad.Add((int)Buttons.DPadUp, upcommand);
            //add fireflower command
            ICommand firecommand = new FireMarioCommand(Stage.mario);
            keyboard.Add((int)Keys.I, firecommand);
            //add super command
            ICommand supercommand = new SuperCommand(Stage.mario);
            keyboard.Add((int)Keys.U, supercommand);
            //add normal mario command
            ICommand normalcommand = new NormalCommand(Stage.mario);
            keyboard.Add((int)Keys.Y, normalcommand);
            //add take damage command
            ICommand damagecommand = new DamageCommand(Stage.mario);
            keyboard.Add((int)Keys.O, damagecommand);
            //add star power command
            ICommand starcommand = new StarCommand(Stage.mario);
            keyboard.Add((int)Keys.L, starcommand);
            //add the fireball command with space
            ICommand fireballcommand = new MakeFireBall(Stage.mario);
            keyboard.Add((int)Keys.Space, fireballcommand);
            //add the Pause hud command with N
            ICommand pausehudcommand = new PauseCommand();
            keyboard.Add((int)Keys.P, pausehudcommand);
            ICommand interact = new InteractCommand();
            keyboard.Add((int)Keys.V, interact);
            gamepad.Add((int)Buttons.RightTrigger, interact);
            //add hold command E, no current mapping on gamepad
            ICommand holdcommand = new HoldingCommand(Stage.mario);
            keyboard.Add((int)Keys.E, holdcommand);
            

            /*
             * Add release commands. These are mainly for mario movement.
             */
            ICommand rightR = new ReleaseRightCommand(Stage.mario);
            keyboard.AddRelease((int)Keys.D, rightR);
            keyboard.AddRelease((int)Keys.Right, rightR);
           

            ICommand leftR = new ReleaseLeftCommand(Stage.mario);
            keyboard.AddRelease((int)Keys.A, leftR);
            keyboard.AddRelease((int)Keys.Left, leftR);

            ICommand downR = new ReleaseDownCommand(Stage.mario);
            keyboard.AddRelease((int)Keys.S, downR);
            keyboard.AddRelease((int)Keys.Down, downR);


            ICommand upR = new ReleaseUpCommand(Stage.mario);
            keyboard.AddRelease((int)Keys.W, upR);
            keyboard.AddRelease((int)Keys.Up, upR);

            ICommand sprintR = new ReleaseSprintCommand(Stage.mario);
            keyboard.AddRelease((int)Keys.LeftShift, sprintR);
            keyboard.AddRelease((int)Keys.RightShift, sprintR);

            ICommand holdR = new ReleaseHoldingCommand(Stage.mario);
            keyboard.AddRelease((int)Keys.E, holdR);
        }
        public static void GameEndControllers(Game1 game, KeyboardController keyboard, GamepadController gamepad)
        {
            keyboard.ClearDictionary();
            gamepad.ClearDictionary();



            ICommand resetcommand = new RestartCommand();
            keyboard.Add((int)Keys.R, resetcommand);
            gamepad.Add((int)Buttons.Back, resetcommand);
            //Add the Exit Command to the controllers
            ICommand exitcommand = new ExitCommand(game);
            keyboard.Add((int)Keys.Q, exitcommand);
            gamepad.Add((int)Buttons.Start, exitcommand);
        }
        public static void PauseControllers(Game1 game, KeyboardController keyboard, GamepadController gamepad, IGameState state)
        {
            keyboard.ClearDictionary();
            gamepad.ClearDictionary();


            ICommand unpause = new UnpauseCommand();
            keyboard.Add((int)Keys.P, unpause);
            gamepad.Add((int)Buttons.X, unpause);
            //Add the Exit Command to the controllers
            ICommand exitcommand = new ExitCommand(game);
            keyboard.Add((int)Keys.Q, exitcommand);
            gamepad.Add((int)Buttons.Start, exitcommand);
            ICommand resetcommand = new RestartCommand();
            keyboard.Add((int)Keys.R, resetcommand);
            gamepad.Add((int)Buttons.Back, resetcommand);
            AddSelection(game, keyboard, gamepad, state);
        }

        private static void AddSelection(Game1 game, KeyboardController keyboard, GamepadController gamepad, IGameState state)
        {
            ICommand upSelection = new UpSelection(state);
            keyboard.Add((int)Keys.Up, upSelection);
            gamepad.Add((int)Buttons.DPadUp, upSelection);

            ICommand downSelection = new DownSelection(state);
            keyboard.Add((int)Keys.Down, downSelection);
            gamepad.Add((int)Buttons.DPadDown, downSelection);

            ICommand select = new MadeSelection(game, state);
            keyboard.Add((int)Keys.Enter, select);
            gamepad.Add((int)Buttons.A, select);
        }


    }
}
