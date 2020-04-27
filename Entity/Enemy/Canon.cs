using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity.Enemy
{
    class Canon : AbstractEnemy
    {
        private int lastChange;
        private int Direction;
        public Canon(Texture2D texture, int direction):base(texture, EntityType.Canon)
        {
            Rows = 1;
            Columns = 1;
            Direction = direction;
            Velocity = new Vector2(0, 0);
            NeedsUpdating = true;
        }

        public override IEntity Clone()
        {
            Canon can = new Canon(Texture, Direction);
            IEntity clone = can;
            base.Clone(ref clone);
            can = (Canon)clone;
            return can;
        }
        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
        

        }
        private void Launch(int direction)
        {
            IEntity e = EnemyFactory.Instance.BulletBill(direction);
            if (e != null)
            {
                e.Position = new Vector2(Position.X+(direction*50), Position.Y);
                Stage.level.Add(e);
                ((AbstractEnemy)e).RaiseFireballEvent += Stage.player.HandleFireballEvent;
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (lastChange > Frame * 15)
            {
                Launch(Direction);
                lastChange = 0;
            }
            else
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }
}
