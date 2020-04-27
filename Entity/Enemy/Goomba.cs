using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.Enemy
{
    public class Goomba : AbstractEnemy
    {
        public Goomba(Texture2D texture) : base(texture, EntityType.Goomba)
        {
            SourceRectangle = new Rectangle(new Point(0, 0), new Point(32, 32));
        }

        public override IEntity Clone()
        {
            Goomba goomba = new Goomba(Texture);
            IEntity clone = goomba;
            base.Clone(ref clone);
            goomba = (Goomba)clone;
            return goomba;
        }
    }
}
