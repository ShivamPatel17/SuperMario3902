using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity.CollisionZones
{
    public class CollisionZone : AbstractEntity
    {
        private bool entered;
        private Rectangle box;
        public override Rectangle CollisionBox { get { return box; } }
        public EntityType LookingFor { get; set; }

        public CollisionZone(Rectangle box, Texture2D texture = null) : base(texture, EntityType.CollisionZone)
        {
            this.box = box;
            entered = false;
            Position = new Vector2(box.Location.X, box.Location.Y + box.Height);
            Collidable = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
            {
                base.Draw(spriteBatch);
            }
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if (box.Right >= entity.CollisionBox.Right && box.Left <= entity.CollisionBox.Left && box.Top <= entity.CollisionBox.Top && box.Bottom >= entity.CollisionBox.Bottom)
            {
                if (!entered && entity.Type == LookingFor)
                {
                    entered = true;
                    OnRaiseEnterEvent(new CollisionZoneEventArgs(entity));
                }
            }
            else
            {
                if (entered && entity.Type == LookingFor)
                {
                    entered = false;
                    OnRaiseLeaveEvent(new CollisionZoneEventArgs(entity));
                }
            }
        }

        protected virtual void OnRaiseEnterEvent(CollisionZoneEventArgs e)
        {
            RaiseEnterEvent?.Invoke(this, e);
        }

        public delegate void EnterEventHandler(object sender, CollisionZoneEventArgs a);
        public event EventHandler<CollisionZoneEventArgs> RaiseEnterEvent;

        protected virtual void OnRaiseLeaveEvent(CollisionZoneEventArgs e)
        {
            RaiseLeaveEvent?.Invoke(this, e);
        }
        public delegate void LeaveEventHandler(object sender, CollisionZoneEventArgs a);
        public event EventHandler<CollisionZoneEventArgs> RaiseLeaveEvent;

        public override IEntity Clone()
        {
            CollisionZone zone = new CollisionZone(box, Texture)
            {
                RaiseLeaveEvent = RaiseLeaveEvent,
                RaiseEnterEvent = RaiseEnterEvent,
                LookingFor = LookingFor
            };

            return zone;
        }

        protected override void Clone(ref IEntity clone)
        {
            //((HiddenCollisionZone)clone).RaiseEnterEvent = RaiseEnterEvent;
            //((HiddenCollisionZone)clone).RaiseLeaveEvent = RaiseLeaveEvent;
        }
    }
}
