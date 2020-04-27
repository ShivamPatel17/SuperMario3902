using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Items;

namespace TechSupportMario.Commands
{
    class FireFlowerItemCommand : ICommand
    {
        private readonly IBlock Block;
        public FireFlowerItemCommand(IBlock block)
        {
            Block = block;
        }
        public void Execute()
        {
            ItemFactory.Instance.FireMushroomItem().BounceFromBlock(Block.Position);
        }
    }
}
