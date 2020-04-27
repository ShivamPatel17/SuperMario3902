using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace TechSupportMario
{
    class KeyboardController : IController
    {
        private Keys[] oldPressedKeys;
        private readonly Dictionary<int, ICommand> commandDict;
        private readonly Dictionary<int, ICommand> releaseDict;

        public KeyboardController()
        {
            commandDict = new Dictionary<int, ICommand>();
            releaseDict = new Dictionary<int, ICommand>();
        }

        public void ClearDictionary()
        {
            commandDict.Clear();
            releaseDict.Clear();
        }

        public void FullClear()
        {
            ClearDictionary();
            oldPressedKeys = new Keys[0];
        }
        
        public void Add(int key, ICommand command)
        {
            commandDict.Add(key, command);
        }

        public void AddRelease(int key, ICommand command)
        {
            releaseDict.Add(key, command);
        }

        public void Update()
        {
            KeyboardState newState = Keyboard.GetState();
            foreach (Keys key in newState.GetPressedKeys())
            {
                if (!oldPressedKeys.Contains(key))
                {
                    if (commandDict.ContainsKey((int)key))
                    {
                        commandDict[(int)key].Execute();
                    }
                }
            }
            if (oldPressedKeys != null)
            {
                foreach (Keys key in oldPressedKeys)
                {
                    if (!newState.GetPressedKeys().Contains(key))
                    {
                        if (releaseDict.ContainsKey((int)key))
                        {
                            releaseDict[(int)key].Execute();
                        }
                    }
                }
            }
            oldPressedKeys = newState.GetPressedKeys();
        }
    }
}
