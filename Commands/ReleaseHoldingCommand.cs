using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Commands
{
    class ReleaseHoldingCommand : ICommand
    {
        private readonly Mario context;

        public ReleaseHoldingCommand(Mario context)
        {
            this.context = context;
        }

        public void Execute()
        {
            context.ReleaseHoldingCommand();
        }
    }
}
