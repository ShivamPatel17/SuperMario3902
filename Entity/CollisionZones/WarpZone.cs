using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TechSupportMario.Entity.Block.BlockEventArgs;

namespace TechSupportMario.Entity.CollisionZones
{
    public class WarpZone : CollisionZone
    {

        public int NextArea { get; set; }

        public WarpZone(Rectangle box, Texture2D texture = null) : base(box, texture)
        {

        }

        public override void Interact()
        {
            OnRaiseWarpEvent(new WarpEventArgs(NextArea, -1));
        }

        protected virtual void OnRaiseWarpEvent(WarpEventArgs e)
        {
            RaiseWarpEvent?.Invoke(this, e);
        }

        public delegate void WarpEventHandler(object sender, WarpEventArgs a);
        public event EventHandler<WarpEventArgs> RaiseWarpEvent;
    }
}
