using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Entity.Items
{
    class Star : AbstractItem
    {
        private bool changedDir;
        public Star(Texture2D texture, int direction) : base(texture, EntityType.Star)
        {
            Direction = direction;
            MaxHeightChange = 33;
            changedDir = false;
        }

        public void GiveUpVelocity()
        {
                Vector2 vel = Velocity;
                vel.Y = -2.9f;
                Velocity = vel;
                
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is Mario)
            {
                Collect = true;
            }
            else if (entity is IBlock)
            {
                if (direction == CollisionDetection.Direction.down || direction == CollisionDetection.Direction.downnov)
                {
                    GiveUpVelocity();
                }
                else if (direction == CollisionDetection.Direction.right || direction == CollisionDetection.Direction.left)
                {  
                    ChangeDirection();
                }
            }
        }

        public override void ChangeDirection()
        {
            if (!changedDir) 
            {
                base.ChangeDirection();
                changedDir = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Collidable)
            {
                base.Update(gameTime);
            }
            changedDir = false;

            if (FromBlock)
            {
                Vector2 nextPosition = Position;
                nextPosition.Y += Velocity.Y;
                Position = nextPosition;
                if (Position.Y <= Anchor - MaxHeightChange)
                {
                    Velocity = new Vector2(0, 0.25f);
                    Anchor -= 32;
                }
                else if (Position.Y >= Anchor)
                {
                    Position = new Vector2((int)Position.X, Anchor);
                    Collidable = true;
                    Velocity = new Vector2(0.8f*Direction,0);
                    FromBlock = false;
                }
                else
                {
                    Position += Velocity;
                }
            }
            if (Collidable){
                if (OnGround){
                   Vector2 vel = Velocity;
                   vel.Y = -1.5f;//temp number, can be changed later.
                   Velocity = vel;
                }
                Position += Velocity;
            }
        }

        public override IEntity Clone()
        {
            Star star = new Star(Texture, Direction);
            IEntity clone = star;
            base.Clone(ref clone);
            star = (Star)clone;
            star.changedDir = changedDir;
            return star;
        }
    }
}
