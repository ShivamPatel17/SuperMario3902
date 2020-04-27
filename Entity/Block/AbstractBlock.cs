using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.BlockState;

namespace TechSupportMario.Entity.Block
{
    public abstract class AbstractBlock : AbstractEntity, IBlock
    {
        public Queue<ICommand> Items { get;}
        public IBlockState BlockState { get; set; }
        protected AbstractBlock(Texture2D texture, EntityType type) : base(texture, type)
        {
            Items = new Queue<ICommand>();
            Collidable = true;
        }

        public virtual void CollisionTransition(Mario mario)
        {

        }

        public virtual void CollisionTransition()
        {

        }
        public virtual void AddCommand(ICommand cmd) {
            Items.Enqueue(cmd);
        }

        protected override void Clone(ref IEntity copy)
        {
            base.Clone(ref copy);
            foreach(ICommand command in Items)
            {
                ((AbstractBlock)copy).Items.Enqueue(command);
            }
            ((AbstractBlock)copy).BlockState = BlockState?.Clone((AbstractBlock)copy);
            ((AbstractBlock)copy).BlockState?.Enter();
        }
    }
}
