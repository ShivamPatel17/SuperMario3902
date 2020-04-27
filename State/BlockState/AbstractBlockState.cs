using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates;

namespace TechSupportMario.State.BlockState
{
    abstract class AbstractBlockState : IBlockState
    {
        public IBlock Context { get; set; }
        public AbstractBlockState(IBlock context)
        {
            Context = context;
        }

        public virtual void Collision(Mario mario)
        {
        }
        public virtual void Collision()
        {

        }

        public virtual void Leave(IState next)
        {
        }

        public virtual void ShowItem()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Enter()
        {
            
        }

        public abstract IBlockState Clone(AbstractBlock block);
    }
}
