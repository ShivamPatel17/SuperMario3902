using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TechSupportMario.State.EnemyState.BossStates;

namespace TechSupportMario.Entity.Enemy
{
    class Boss : AbstractEnemy
    {
        public int Lives { get; set; }
        private const int fireTime = 120;
        private int lastFire = 0;
        public int Hit { get; set; }
        private Random random;
        public Boss(Texture2D texture): base(texture, EntityType.Boss)
        {
            Lives = 3;
            random = new Random();
            EnemyState = new BossLeft(this);
            EnemyState.Enter();
            SourceRectangle = new Rectangle(new Point(0, 0), new Point(32, 64));
            Hit = 0;
        }

        public override IEntity Clone()
        {
            Boss boss = new Boss(Texture);
            IEntity clone = boss;
            base.Clone(ref clone);
            boss = (Boss)clone;
            return boss;
        }

        public override void Update(GameTime gameTime)
        {
            lastFire += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(Hit > 0)
            {
                Hit -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (EnemyState is BossFire)
            {
                
                if(lastFire > fireTime)
                {
                    EnemyState.Leave(null);
                }
            }
            else if(EnemyState is BossHit)
            {
                if (Hit <= 0)
                {
                    EnemyState.Leave(null);
                    Hit = 0;
                }
            }
            else
            {
                if(lastFire * random.NextDouble() > 1000f)
                {
                    ((AbstractBossState)EnemyState).Fire();
                    lastFire = 0;
                }
                else
                {
                    base.Update(gameTime);
                }
            }
        }
    }
}
