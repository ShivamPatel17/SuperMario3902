using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.Enemy
{
    public class GreenKoopa : AbstractKoopa
    {


        public GreenKoopa(Texture2D texture) : base(texture, EntityType.GreenKoopa)
        {

        }

        public override IEntity Clone()
        {
            GreenKoopa greenKoopa = new GreenKoopa(Texture);
            IEntity clone = greenKoopa;
            base.Clone(ref clone);
            greenKoopa = (GreenKoopa)clone;
            return greenKoopa;
        }

        public override void UpdateTextureAndBox()
        {
            if (EnemyState is KoopaShellState)
            {
                Texture = EnemyFactory.Instance.SwapToGreenShell();
                SourceRectangle = new Rectangle(new Point(0, 0), new Point(32, 32));
            }
            else
            {
                Texture = EnemyFactory.Instance.SwapToGreenKoopa();
                SourceRectangle = new Rectangle(new Point(0, 0), new Point(33, 52));
            }
        }
    }
}
