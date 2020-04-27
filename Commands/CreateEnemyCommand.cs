using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Enemy;
using static TechSupportMario.Entity.AbstractEntity;

namespace TechSupportMario.Commands
{
    class CreateEnemyCommand : ICommand
    {
        public EntityType Create { get; }
        private readonly EntityWarpBlock block;

        public CreateEnemyCommand(EntityType create, EntityWarpBlock block)
        {
            Create = create;
            this.block = block;
        }

        public void Execute()
        {
            Stage.AddEntity(EnemyFactory.Instance.NewEnemy(Create, block));
        }

    }
}
