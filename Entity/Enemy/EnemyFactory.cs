using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Entity.Block;
using static TechSupportMario.Entity.AbstractEntity;

namespace TechSupportMario.Entity.Enemy
{
    public class EnemyFactory
    {
        private static EnemyFactory _instance;
        private Texture2D goombaSpriteSheet;
        private Texture2D redKoopaSpriteSheet;
        private Texture2D greenKoopaSpriteSheet;
        private Texture2D deadEnemySpriteSheet;
        private Texture2D greenKoopaShellSpriteSheet;
        //not implementing with red shells yet
        private Texture2D redKoopaShellSpriteSheet;
        private Texture2D piranhaPlantSpriteSheet;
        private Texture2D booSpriteSheet;
        private Texture2D canonSpriteSheet;
        private Texture2D bulletBillSpriteSheet;
        private Texture2D boss;
        private Texture2D fireball;
        private EnemyFactory()
        {

        }
        public void LoadFactory(Game game)
        {
            //load textures
            goombaSpriteSheet = game?.Content.Load<Texture2D>("enemies/goomba_sprite");
            redKoopaSpriteSheet = game.Content.Load<Texture2D>("enemies/red_koopa");
            greenKoopaSpriteSheet = game.Content.Load<Texture2D>("enemies/green_koopa");
            deadEnemySpriteSheet = game.Content.Load<Texture2D>("enemies/explosion");
            piranhaPlantSpriteSheet = game.Content.Load<Texture2D>("enemies/piranhaPlant");
            booSpriteSheet = game.Content.Load<Texture2D>("enemies/boo");
            bulletBillSpriteSheet = game.Content.Load<Texture2D>("enemies/bulletbill");
            canonSpriteSheet = game.Content.Load<Texture2D>("enemies/cannon");
            redKoopaShellSpriteSheet = game.Content.Load<Texture2D>("enemies/red_shell");
            greenKoopaShellSpriteSheet = game.Content.Load<Texture2D>("enemies/green_shell");
            boss = game.Content.Load<Texture2D>("enemies/boss");
            fireball = game.Content.Load<Texture2D>("enemies/fireball");
        }

        public static EnemyFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EnemyFactory();
                }
                return _instance;
            }
        }

        public IEntity NewEnemy(EntityType entity, IBlock block)
        {
            IEntity returningEnemy;
            switch (entity)
            {
                case EntityType.Goomba:
                    returningEnemy = Goomba();
                    break;
                case EntityType.GreenKoopa:
                    returningEnemy = GreenKoopa();
                    break;
                case EntityType.RedKoop:
                    returningEnemy = RedKoopa();
                    break;
                case EntityType.Boo:
                    returningEnemy = Boo();
                    break;
                case EntityType.Canon:
                    //set default to right for now. should be changed to right and left eventually
                    returningEnemy = Canon(1);
                    break;
                case EntityType.BulletBill:
                    returningEnemy = BulletBill(1);
                    break;
                default:
                    return PiranhaPlant(block);
            }
            returningEnemy.Position = new Vector2(block.CollisionBox.Location.X, block.CollisionBox.Location.Y);
            return returningEnemy;
        }

        public IEntity PiranhaPlant(IBlock block)
        {
            return new PiranhaPlant(piranhaPlantSpriteSheet, block);
        }

        public IEntity Goomba()
        {
            return new Goomba(goombaSpriteSheet);
        }

        public IEntity RedKoopa()
        {
            return new RedKoopa(redKoopaSpriteSheet);
        }

        public IEntity GreenKoopa()
        {
            return new GreenKoopa(greenKoopaSpriteSheet);
        }

        public IEntity Canon(int direction)
        {
            return new Canon(canonSpriteSheet, direction);
        }
        
        public IEntity BulletBill(int direction)
        {
            return new BulletBill(bulletBillSpriteSheet, direction);
        }

        public IEntity Boo()
        {
            return new Boo(booSpriteSheet);
        }
        public IEntity Explosion(Vector2 position)
        {
            return new DeadEnemy(deadEnemySpriteSheet, position);
        }

        public Texture2D SwapToGreenShell()
        {
            return greenKoopaShellSpriteSheet;
        }
        public Texture2D SwapToGreenKoopa()
        {
            return greenKoopaSpriteSheet;
        }
        public Texture2D SwapToRedShell()
        {
            return redKoopaShellSpriteSheet;
        }
        public Texture2D SwapToRedKoopa()
        {
            return redKoopaSpriteSheet;
        }

        public AbstractEnemy Boss()
        {
            return new Boss(boss);
        }

        public EnemyFire MakeFireball(Vector2 position, int dir)
        {
            bool facing = dir >= 2;

            Vector2 pos;
            if (facing)
            {
                pos = new Vector2(position.X + 32 + 2, position.Y - 30);
            }
            else
            {
                pos = new Vector2(position.X - fireball.Width - 1, position.Y - 30);
            }
            EnemyFire fb = new EnemyFire(fireball, facing)
            {
                Position = pos
            };
            Rectangle source = fb.SourceRectangle;
            source.Width /= 2;
            fb.SourceRectangle = source;
            return fb;
        }
    }
}
