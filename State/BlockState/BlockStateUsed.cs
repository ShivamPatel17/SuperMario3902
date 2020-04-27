using Microsoft.Xna.Framework;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Block;

namespace TechSupportMario.State.BlockState
{
    class BlockStateUsed :AbstractBlockState
    {

        public BlockStateUsed(IBlock block) : base(block)
        {

        }

        public override void Enter()
        {
            if(Context is QuestionBlock)
            {
                Rectangle source = Context.SourceRectangle;
                source.X = Context.Texture.Width - Context.SourceRectangle.Width;
                Context.SourceRectangle = source;
            }
            else
            {
                Context.Texture = BlockFactory.Instance.UsedTexture();
            }
        }

        public override IBlockState Clone(AbstractBlock block)
        {
            return new BlockStateUsed(block);
        }
    }
}
