using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.CameraItems;
using TechSupportMario.LevelLoader;

namespace TechSupportMario.Commands
{
    class ResetCommand : ICommand
    {
        public ResetCommand()
        {
            
        }

        public void Execute()
        {
            Stage.Reset();
        }
    }
}
