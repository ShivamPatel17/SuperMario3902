using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.HeadsUpDisplay;
using TechSupportMario.LevelLoader;

namespace TechSupportMario.Commands
{
    class PauseCommand : ICommand
    {
        public PauseCommand()
        {

        }
        public void Execute()
        {
            Stage.level.State.Pause();
        }
    }
}
