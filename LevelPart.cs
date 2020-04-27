using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario
{
    public class LevelPart
    {

        public QuadTree Entities { get; set; }
        public SortedSet<Layer> Layers { get; set; }
        public ISet<AbstractEnemy> Enemies { get; set; }
        public Dictionary<int, Vector2> ExitPositions { get; set; }
        public int Song { get; set; }
        public LevelPart()
        {
            Layers = new SortedSet<Layer>();
            Enemies = new HashSet<AbstractEnemy>();
            ExitPositions = new Dictionary<int, Vector2>();
        }
    }
}
