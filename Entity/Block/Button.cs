using Microsoft.Xna.Framework.Graphics;
using System;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity.Block
{
    class Button : AbstractBlock
    {
        public bool Set { get; set; }
        public Button(Texture2D texture) : base(texture, EntityType.Button)
        {
            Set = false;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(direction == CollisionDetection.Direction.up)
            {
                OnRaiseWarpEvent(new EventArgs());
            }
        }

        protected virtual void OnRaiseWarpEvent(EventArgs e)
        {
            RaisePressEvent?.Invoke(this, e);
        }

        public delegate void PressEventHandler(object sender, EventArgs a);
        public event EventHandler<EventArgs> RaisePressEvent;

        public override IEntity Clone()
        {
            Button block = new Button(Texture);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (Button)clone;
            block.RaisePressEvent = RaisePressEvent;
            return block;
        }
    }
}
