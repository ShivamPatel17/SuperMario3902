using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Commands;
using TechSupportMario.Entity.CollisionZones;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.Entity.Block
{
    class EntityWarpBlock : AbstractBlock
    {
        public ICommand Command { get; set; }
        public IEntity Shot { get; set; }
        public bool Fire { get; set; }
        private const int  timeToFire = 3000;
        private int lastFire = 2500;

        public EntityWarpBlock(Texture2D texture) : base(texture, EntityType.WarpPipeEntity)
        {
            NeedsUpdating = true;
            Fire = true;
        }

        public void HandleEnterEvent(object sender, CollisionZoneEventArgs e)
        {
            Fire = false;
        }

        public void HandleLeaveEvent(object sender, CollisionZoneEventArgs e)
        {
            Fire = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Fire)
            {
                lastFire += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (lastFire >= timeToFire)
                {
                    if (Shot == null)
                    {
                        Command?.Execute();
                    }
                    else
                    {
                        ((PiranhaPlant)Shot).EnemyState.ChangeDirections();
                    }
                    lastFire = 0;
                }
            }
            else
            {
                lastFire = 0;
            }
        }

        public override IEntity Clone()
        {
            EntityWarpBlock block = new EntityWarpBlock(Texture);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (EntityWarpBlock)clone;
            block.Fire = Fire;
            block.lastFire = lastFire;
            if(Command is CreateEnemyCommand)
            {
                block.Command = new CreateEnemyCommand( ((CreateEnemyCommand)Command).Create, block);
            }
            else
            {
                block.Command = Command;
            }
            block.Shot = Shot?.Clone();
            return block;
        }
    }
}
