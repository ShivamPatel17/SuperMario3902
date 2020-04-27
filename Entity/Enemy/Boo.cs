using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.Enemy
{
    public class Boo : AbstractEnemy
    {
        int currentYFrame;
        private double xdiff, ydiff;
        public Boo(Texture2D texture):base(texture, EntityType.Boo)
        {
            Velocity = new Vector2(1, 1);
            Columns = 2;
            SourceRectangle = new Rectangle(new Point(0, 0), new Point(32, 32));
        }
        public override IEntity Clone()
        {
            Boo boo = new Boo(Texture);
            IEntity clone = boo;
            base.Clone(ref clone);
            boo = (Boo)clone;
            return boo;
        }


        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity  is Mario  && ((Mario)entity).PowerStateEnum == Mario.PowerStateType.Star)
            {
                Collect = true;
                NeedsUpdating = false;
            }
               
        }

        public override void Update(GameTime gameTime)
        {
            xdiff = MathHelper.Clamp((Stage.mario.Position.X - Position.X), -1, 1);
            ydiff = MathHelper.Clamp((Stage.mario.Position.Y - Position.Y), -.4f, .3f);

            Velocity = new Vector2((float)xdiff, (float)ydiff);
            if (xdiff < 0)
            {
                Flip(0);
            }
            else
            {
                Flip(1);
            }
            if((xdiff > 0 && Stage.mario.SourceRectangle.X >= 300) || (xdiff < 0 && Stage.mario.SourceRectangle.X <= 300))
            {
                Position += Velocity;
            }
            
        }
        public void Flip(int i)
        {
            currentYFrame = i;
            Rectangle source = SourceRectangle;
            source.Y = currentYFrame * SourceRectangle.Size.Y;
            SourceRectangle = source;
        }
    }
}
