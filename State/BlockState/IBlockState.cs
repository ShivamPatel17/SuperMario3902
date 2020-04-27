using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.MarioStates;

namespace TechSupportMario.State.BlockState
{
    public interface IBlockState : IState
    {
        void Collision(Mario mario);
        void Collision();
        void ShowItem();
        void Update(GameTime gameTime);

        IBlockState Clone(AbstractBlock block);
    }
}
