using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates;

namespace TechSupportMario.State.BlockState
{
    class QuestionBlockStateNormal : AbstractBlockState
    {
        public QuestionBlockStateNormal(IBlock context) : base(context)
        {

        }

        public override IBlockState Clone(AbstractBlock block)
        {
            return new QuestionBlockStateNormal(block);
        }

        public override void Collision(Mario mario)
        {
            IBlockState nextState = new BlockStateBump(Context, true);
            Context.BlockState = nextState;
            Leave(nextState);
        }
        public override void Collision()
        {
            //collision by koopa shell
            IBlockState nextState = new BlockStateBump(Context, true);
            Context.BlockState = nextState;
            Leave(nextState);
        }

        public override void Leave(IState next)
        {
            Context.BlockState.Enter();
        }

    }
}
