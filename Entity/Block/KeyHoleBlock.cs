using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TechSupportMario.Entity.Block
{
    public class KeyHoleBlock : AbstractBlock
    {
        private bool _used;
        public bool Used 
        {
            get { return _used; }
            set
            {
                if (value)
                {
                    SourceRectangle = new Rectangle(new Point(0, Texture.Height / 2), new Point(Texture.Width, Texture.Height / 2));
                }
                else
                {
                    new Rectangle(new Point(0), new Point(Texture.Width, Texture.Height / 2));
                }
                _used = value;
            }
        }

        public KeyHoleBlock(Texture2D texture) : base(texture, EntityType.KeyHole)
        {
            SourceRectangle = new Rectangle(new Point(0), new Point(Texture.Width, texture.Height / 2));
            Used = false;
        }

        public override IEntity Clone()
        {
            KeyHoleBlock clone = new KeyHoleBlock(Texture);
            IEntity entity = clone;
            base.Clone(ref entity);
            clone.Used = Used;
            clone.SourceRectangle = SourceRectangle;
            return clone;
        }
    }
}
