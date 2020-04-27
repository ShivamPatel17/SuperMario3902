using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;



namespace TechSupportMario
{
    class GamepadController : IController
    {
        private GamePadState pastState;
        private readonly Dictionary<int, ICommand> commandDict;

        public GamepadController()
        {
            commandDict = new Dictionary<int, ICommand>();
        }

        public void ClearDictionary()
        {
            commandDict.Clear();
        }

        public void FullClear()
        {
            commandDict.Clear();
            pastState = new GamePadState();
        }

        public void Add(int button, ICommand command)
        {
            if(!commandDict.ContainsKey(button))
                commandDict.Add(button, command);
        }
        public void Update()
        {
            GamePadState emptyInput = new GamePadState();
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected && currentState != emptyInput)
            {
                var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));
                foreach (var button in buttonList)
                {
                    if (currentState.IsButtonDown(button) &&
                        !pastState.IsButtonDown(button))
                    {
                        if (commandDict.TryGetValue((int)button, out ICommand command))
                            command.Execute();
                    }
                }
            }
            pastState = currentState;
        }
    }
}
