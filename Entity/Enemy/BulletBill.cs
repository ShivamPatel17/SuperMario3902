using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.Enemy
{
    class BulletBill : AbstractEnemy
    {
        private readonly int Direction;
        //SpriteEffects effect = SpriteEffects.FlipHorizontally;
        public BulletBill(Texture2D texture, int direction):base(texture, EntityType.BulletBill)
        {
            Rows = 1;
            Columns = 1;
            EnemyState = new NormalState(this);
            EnemyState.Enter();
            if(direction == 1)
            {
                Velocity = new Vector2(2, 0);
                SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if(direction == -1)
            {
                Velocity = new Vector2(-2, 0);
                SpriteEffect = SpriteEffects.None;
            }
            Direction = direction;
        }
        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is IBlock)
            {
                Collect = true;
            }
            else if (entity is Mario)
            {
               if (((Mario)entity).PowerStateEnum == Mario.PowerStateType.Star)
                {
                    TakeDamage();
                    SoundFactory.Instance.Stomp();
                    NeedsUpdating = true;
                    Collidable = false;
                    Stage.player.AddPoints(1000);
                }
            }else if(entity is BulletBill){
                TakeDamage();
                SoundFactory.Instance.Stomp();
                NeedsUpdating = true;
                Collidable = false;
            }
           
        }
        public override IEntity Clone()
        {
            BulletBill bb = new BulletBill(Texture, Direction);
            IEntity clone = bb;
            base.Clone(ref clone);
            bb = (BulletBill)clone;
            return bb;
        }
        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
        }
 
    }
}
