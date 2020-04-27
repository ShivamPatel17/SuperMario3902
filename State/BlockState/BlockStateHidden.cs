using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates;

namespace TechSupportMario.State.BlockState
{
    class BlockStateHidden : AbstractBlockState
    {
        //hidden block state is for when the block is hidden not when it is transitioning out of hidden
        public BlockStateHidden(IBlock context) : base(context)
        {
            
        }

        public override void Enter()
        {
            Context.Opacity = 0.0f;
        }

        public override void Collision(Mario mario)
        {
            //if(mario.PowerState is )
            IBlockState nextState;
            if (Context is BrickBlock)
            {
                nextState = new BrickBlockStateNormal(Context);
            }
            else
            {
                nextState = new QuestionBlockStateNormal(Context);
            }
            Context.BlockState = nextState;
            Leave(nextState);
        }

        public override void Leave(IState next)
        {
            Context.Opacity = 1f;
            Context.BlockState.Enter();
        }

        public override IBlockState Clone(AbstractBlock block)
        {
            return new BlockStateHidden(block);
        }
    }
}
