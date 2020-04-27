using Microsoft.Xna.Framework;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    public interface IEnemyState : IState
    {
        void TakeDamage();
        void ChangeDirections();
        void Update(GameTime gameTime);

        IEnemyState Clone(AbstractEnemy enemy);
    }
}
