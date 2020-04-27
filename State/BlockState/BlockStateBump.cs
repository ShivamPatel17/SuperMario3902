using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Block;
using TechSupportMario.State.MarioStates;
using TechSupportMario.SoundItems;

namespace TechSupportMario.State.BlockState
{
    class BlockStateBump : AbstractBlockState
    {

        private const int MaxHeightChange = -18;
        private readonly bool hadItems = false;
        public int Anchor { get; }

        public BlockStateBump(IBlock block, bool hadItems) : base(block)
        {
            SoundFactory.Instance.BrickBump();
            Anchor = (int)block.Position.Y;
            this.hadItems = hadItems;
        }

        public override void Enter()
        {

            Context.Velocity = new Vector2(0, -1.75f);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 nextPosition = Context.Position;
            nextPosition.Y += Context.Velocity.Y;
            Context.Position = nextPosition;
            if (Context.Position.Y <= Anchor + MaxHeightChange)
            {
                Context.Velocity = new Vector2(0, 1.75f);
            }
            else if (Context.Position.Y >= Anchor)
            {
                Context.Position = new Vector2((int)Context.Position.X, Anchor);
                Context.Velocity = new Vector2(0, 0);
                Leave(this);
            }
            else
            {
                Context.Position += Context.Velocity;
            }
        }

        public override void Leave(IState nextState)
        {

            if (Context is QuestionBlock)
            {
                nextState = new BlockStateUsed(Context);
            }
            else
            {
                if(hadItems && Context.Items.Count == 0)
                {
                    nextState = new BlockStateUsed(Context);
                }
                else
                {
                    nextState = new BrickBlockStateNormal(Context);
                }
            }
            Context.BlockState = (IBlockState)nextState;
            Context.BlockState.Enter();
        }

        public override IBlockState Clone(AbstractBlock block)
        {
            return new BlockStateBump(block, hadItems);
        }
    }
}
