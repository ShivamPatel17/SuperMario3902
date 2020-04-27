
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity.CollisionZones
{
    public class LimitedHiddenZone : CollisionZone
    {
        protected bool used;

        public LimitedHiddenZone(Rectangle rectangle, Texture2D texture = null): base(rectangle, texture)
        {
            used = false;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if (!used)
            {
                base.CollisionResponse(entity, direction);
            }
        }

        protected override void OnRaiseLeaveEvent(CollisionZoneEventArgs e)
        {
            used = true;
            base.OnRaiseLeaveEvent(e);
        }

        protected override void OnRaiseEnterEvent(CollisionZoneEventArgs e)
        {
            e.Position = Position;
            base.OnRaiseEnterEvent(e);
        }

        public override IEntity Clone()
        {
            LimitedHiddenZone zone = new LimitedHiddenZone(SourceRectangle, Texture)
            {
                used = used
            };
            IEntity clone = zone;
            base.Clone(ref clone);
            zone = (LimitedHiddenZone)clone;
            //zone.RaiseEnterEvent += Stage.HandleCheckpoint;
            return zone;
        }
    }
}
