using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class EnemyStateDead : AbstractEnemyState
    {
        public EnemyStateDead(AbstractEnemy context) : base(context)
        {

        }

        public override IEnemyState Clone(AbstractEnemy enemy)
        {
            IEnemyState state = new EnemyStateDead(enemy);
            return state;
        }

        public override void Enter()
        {
            Context.Opacity = 0.75f;

            Context.Velocity = new Vector2(0,0);

            Vector2 position = Context.Position - new Vector2(0, Context.Texture.Height / 2.0f);
            Context.DeadEnemy = (AbstractEnemy)EnemyFactory.Instance.Explosion(position);
            Rectangle rectangle = Context.DeadEnemy.SourceRectangle;
            rectangle.Width /= 5;
            Context.DeadEnemy.SourceRectangle = rectangle;
            Context.Collidable = false;
            Context.NeedsUpdating = true;
        }

        public override void Update(GameTime gameTime)
        {
            Context.DeadEnemy.Update(gameTime);
            if (Context.DeadEnemy.Collect)
            {
                Context.Collect = true;
            }
        }
    }
}
