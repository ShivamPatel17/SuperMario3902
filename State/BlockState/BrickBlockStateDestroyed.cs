using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Block;
using TechSupportMario.SoundItems;


namespace TechSupportMario.State.BlockState
{
    class BrickBlockStateDestroyed : AbstractBlockState
    {
        public BrickBlockStateDestroyed(IBlock block) : base(block)
        {

        }

        public override void Enter()
        {
            Context.Opacity = 0f;
            Context.Collidable = false;
        }

        public override IBlockState Clone(AbstractBlock block)
        {
            return new BrickBlockStateDestroyed(block);
        }
    }
}
