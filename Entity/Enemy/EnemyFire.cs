using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity.Enemy
{
    public class EnemyFire : AbstractEnemy
    {

        public EnemyFire(Texture2D texture, bool direction) : base(texture, EntityType.EnemyFire)
        {
            if (direction)
            {
                Velocity = new Vector2(5, 0);
            }
            else
            {
                Velocity = new Vector2(-5, 0);
            }
            MaxVelocity = 5.5f;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            Collect = true;
        }

        public override void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(Velocity.X);
            Position += Velocity;
        }

        public override IEntity Clone()
        {
            bool direction = Velocity.X > 0;
            EnemyFire fireball = new EnemyFire(Texture, direction);
            IEntity clone = fireball;
            base.Clone(ref clone);
            return clone;
        }


    }
}
