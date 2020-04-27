using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.Enemy
{
    public class RedKoopa : AbstractKoopa//eventually the green and red koopa will have different behavior in how they move in the world
    {
        

        public RedKoopa(Texture2D texture) : base(texture, EntityType.RedKoop)
        {
            
        }

        public override IEntity Clone()
        {
            RedKoopa redKoopa = new RedKoopa(Texture);
            IEntity clone = redKoopa;
            base.Clone(ref clone);
            redKoopa = (RedKoopa)clone;
            return redKoopa;
        }
        public override void UpdateTextureAndBox()
        {
            if (EnemyState is KoopaShellState)
            {
                Texture = EnemyFactory.Instance.SwapToRedShell();
                SourceRectangle = new Rectangle(new Point(0, 0), new Point(32, 32));
            }
            else
            {
                Texture = EnemyFactory.Instance.SwapToRedKoopa();
                SourceRectangle = new Rectangle(new Point(0, 0), new Point(33, 52));
            }
        }

    }
}
