using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;

namespace TechSupportMario.Entity.MarioEntity
{
    class Fireball : AbstractEntity
    {
        private int lastChange;
        /// <summary>
        /// If direction is true then the fire ball with go right, if false it will go left;
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="direction"></param>
        public Fireball(Texture2D texture, bool direction) : base(texture, EntityType.Fireball)
        {
            if (direction)
            {
                Velocity = new Vector2(5, 0);
            }
            else
            {
                Velocity = new Vector2(-5, 0);
            }
            lastChange = 0;
            MaxVelocity = 5.5f;
        }

        public void GiveUpVelocity()
        {
            Vector2 vel = Velocity;
            vel.Y = -3;//temp number, can be changed later.
            Velocity = vel;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is IBlock)
            {
                if(direction != CollisionDetection.Direction.down)
                {
                    Collect = true;
                }
                else
                {
                    GiveUpVelocity();
                }
            }
            else
            {
                Collect = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(Velocity.X);
            base.Update(gameTime);
            if(lastChange >= Frame)
            {
                lastChange = 0;
                if(SourceRectangle.X != 0)
                {
                    Rectangle source = SourceRectangle;
                    source.X = 0;
                    SourceRectangle = source;
                }
                else
                {
                    Rectangle source = SourceRectangle;
                    source.X = Texture.Width /2;
                    SourceRectangle = source;
                }
            }
            else
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public override IEntity Clone()
        {
            bool direction = Velocity.X > 0;
            Fireball fireball = new Fireball(Texture, direction);
            IEntity clone = fireball;
            base.Clone(ref clone);
            return clone;
        }
    }
}
