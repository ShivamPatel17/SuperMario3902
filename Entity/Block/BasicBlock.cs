using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.Block
{
    public class BasicBlock : AbstractBlock
    {

        public BasicBlock(Texture2D texture, EntityType type) : base(texture, type)
        {

        }

        public override IEntity Clone()
        {
            BasicBlock block = new BasicBlock(Texture, Type);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (BasicBlock)clone;
            return block;
        }
    }
}
