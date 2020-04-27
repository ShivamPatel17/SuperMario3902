using System.Collections.Generic;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.BlockState;

namespace TechSupportMario.Entity.Block
{
    public interface IBlock : IEntity
    {
        Queue<ICommand> Items { get; }
        IBlockState BlockState { get; set; }
        void AddCommand(ICommand cmd);
        void CollisionTransition(Mario mario);
        void CollisionTransition();
    }
}
