using Microsoft.Xna.Framework.Graphics;
using System;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block.BlockEventArgs;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Entity.Block
{
    class WarpEntranceBlock : AbstractBlock
    {
        private readonly CollisionDetection.Direction facingDirection;
        public int Warp { get; set; }
        public WarpEntranceBlock(Texture2D texture, CollisionDetection.Direction dir) : base(texture, EntityType.WarpPipe)
        {
            facingDirection = dir;
            Order = .7f;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is Mario mario)
            {
                if(facingDirection == direction)
                {
                    if(facingDirection == CollisionDetection.Direction.up)
                    {
                        if(mario.ActionStateEnum == Mario.ActionStateType.Crouching)
                        {
                            SoundItems.SoundFactory.Instance.Pipe();
                            OnRaiseWarpEvent(new WarpEventArgs(Warp, 1));

                        }
                    }
                    else
                    {
                        int dir;
                        if(facingDirection == CollisionDetection.Direction.right)
                        {
                            dir = 4;
                        }
                        else if (facingDirection == CollisionDetection.Direction.left)
                        {
                            dir = 2;
                        }
                        else
                        {
                            dir = 3;
                        }
                        OnRaiseWarpEvent(new WarpEventArgs(Warp, dir));
                    }
                }
            }
        }

        protected virtual void OnRaiseWarpEvent(WarpEventArgs e)
        {
            RaiseWarpEvent?.Invoke(this, e);
        }

        public override IEntity Clone()
        {
            WarpEntranceBlock block = new WarpEntranceBlock(Texture, facingDirection);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (WarpEntranceBlock)clone;
            block.RaiseWarpEvent = RaiseWarpEvent;
            block.Warp = Warp;
            return block;
        }

        public delegate void WarpEventHandler(object sender, WarpEventArgs a);
        public event EventHandler<WarpEventArgs> RaiseWarpEvent;
    }
}
