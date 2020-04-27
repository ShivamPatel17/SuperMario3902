
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.CollisionZones
{
    public class CollisionZoneFactory
    {
        private static CollisionZoneFactory _instance;

        private Texture2D flag;
        private Texture2D checkpoint;
        private Texture2D castle;
        private Texture2D smallDoor;
        private Texture2D lockedDoor;

        private CollisionZoneFactory() { }

        // Still doesn't fully undestand this
        // Basically if _instance is null then assign value after ??
        public static CollisionZoneFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollisionZoneFactory();
                }
                return _instance;
            }
        }

        public void LoadFactory(Game game)
        {
            flag = game.Content.Load<Texture2D>("CollisionZones/flag");
            castle = game.Content.Load<Texture2D>("CollisionZones/Castle");
            checkpoint = game.Content.Load<Texture2D>("CollisionZones/checkpoint");
            smallDoor = game.Content.Load<Texture2D>("CollisionZones/smalldoor");
            lockedDoor = game.Content.Load<Texture2D>("CollisionZones/reddoor");
        }

        public LimitedHiddenZone Flag(Point position)
        {
            return new LimitedHiddenZone(new Rectangle(new Point(position.X, position.Y - flag.Bounds.Height), flag.Bounds.Size), flag);
        }

        public LimitedHiddenZone Castle(Point position)
        {
            return new LimitedHiddenZone(new Rectangle(position, castle.Bounds.Size), castle);
        }

        public Checkpoint CheckpointZone(Point position, Point size)
        {
            return new Checkpoint(new Rectangle(position, size), checkpoint);
        }

        public WarpZone SmallDoor(Point position)
        {
            return new WarpZone(new Rectangle(new Point(position.X, position.Y - smallDoor.Height), new Point(smallDoor.Width, smallDoor.Height)), smallDoor);
        }

        public LockedDoor LockedDoorZone(Point position)
        {
            return new LockedDoor(new Rectangle(new Point(position.X, position.Y - lockedDoor.Height), lockedDoor.Bounds.Size), 4, lockedDoor);
        }

    }
}
