using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Items;

namespace TechSupportMario.Commands
{
    class OneUpMushroomItemCommand : ICommand
    {
        private readonly IBlock Block;
        public OneUpMushroomItemCommand(IBlock block)
        {
            Block = block;
        }
        public void Execute()
        {
            ItemFactory.Instance.OneUpMushroomItem(Block.Position.X + (Block.SourceRectangle.Width/2.0f)).BounceFromBlock(Block.Position);
        }
    }
}
