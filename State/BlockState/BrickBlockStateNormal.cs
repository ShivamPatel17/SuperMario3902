using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates.MarioPowerStates;

namespace TechSupportMario.State.BlockState
{
    class BrickBlockStateNormal : AbstractBlockState
    {
        public BrickBlockStateNormal(IBlock block) : base(block)
        {

        }

        public override void Enter()
        {
            Context.Velocity = new Vector2();
        }

        public override void Collision(Mario mario)
        {
            IBlockState nextState;
            if (Context.Items?.Count > 0)
            {
                //release item and bump
                Context.Items.Peek().Execute();
                Context.Items.Dequeue();
                nextState = new BlockStateBump(Context, true);
            }
            else
            {
                
                //check mario's power state if normal bump else destroy
                if (mario.PowerState is MarioPowerStateNormal)
                {
                    
                    nextState = new BlockStateBump(Context, false);
                }
                else
                {
                    nextState = new BrickBlockStateDestroyed(Context);
                }
            }
            
            Leave(nextState);
        }

        public override void Collision()
        {
            IBlockState nextState;
            if (Context.Items?.Count > 0)
            {
                //release item and bump
                Context.Items.Peek().Execute();
                Context.Items.Dequeue();
                nextState = new BlockStateBump(Context, true);
            }
            else
            {

                nextState = new BlockStateBump(Context, false);
            }

            Leave(nextState);
        }

        public override void Leave(IState nextState)
        {
            Context.BlockState = (IBlockState) nextState;
            Context.BlockState.Enter();
        }

        public override IBlockState Clone(AbstractBlock block)
        {
            return new BrickBlockStateNormal(block);
        }
    }
}
