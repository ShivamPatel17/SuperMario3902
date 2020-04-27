
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Items;

namespace TechSupportMario.Commands
{
    class StarItemCommand : ICommand
    {
        private readonly IBlock block;
        public StarItemCommand(IBlock block)
        {
            this.block = block;
        }
        public void Execute()
        {
            ItemFactory.Instance.StarItem(block.Position.X + (block.SourceRectangle.Width/2.0f)).BounceFromBlock(block.Position);
        }
    }
}
