using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace TechSupportMario.Entity.Items
{
    class ItemFactory
    {
        private static ItemFactory instance;
        private Texture2D OneUpMushroomSpriteSheet;
        private Texture2D FireMushroomSpriteSheet;
        private Texture2D SuperMushroomSpriteSheet;
        private Texture2D StarSpriteSheet;
        private Texture2D CoinSpriteSheet;
        private Texture2D SmallOneUpSheet;
        private Texture2D KeySpriteSheet;
        

        private ItemFactory()
        {
        }
        public void LoadFactory(Game game)
        {
            OneUpMushroomSpriteSheet = game.Content.Load<Texture2D>("items/green_mushroom");
            FireMushroomSpriteSheet = game.Content.Load<Texture2D>("items/fire_flower");
            SuperMushroomSpriteSheet = game.Content.Load<Texture2D>("items/red_mushroom");
            StarSpriteSheet = game.Content.Load<Texture2D>("items/star");
            CoinSpriteSheet = game.Content.Load<Texture2D>("items/coin");
            SmallOneUpSheet = game.Content.Load<Texture2D>("items/green_mushroom_small");
            KeySpriteSheet = game.Content.Load<Texture2D>("blocks/key");
        }

        public static ItemFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemFactory();
                }
                return instance;

            }
        }

        public CoinSprite CoinSprite()
        {
            return new CoinSprite(CoinSpriteSheet);
        }

        public IItem QuestionCoin()
        {
            return new Coin(CoinSpriteSheet);
        }

        public IItem Coin()
        {
            Coin coin = new Coin(CoinSpriteSheet)
            {
                FromBlock = false
            };
            return coin;
        }

        public IItem FireMushroomItem()
        {
            return new FireFlower(FireMushroomSpriteSheet);
        }
        public IItem KeyItem()
        {
            return new Key(KeySpriteSheet);
        }
        private static bool CheckMario(float block){
            bool r;
            float bl = (block - 2);
            float m = Stage.mario.Position.X + (Stage.mario.SourceRectangle.Width / 2.0f);
            if(Math.Abs(bl - m) < 0.05f){
                r = true;
            }
            else{
                if(m < bl)
                    r = false;
                else
                    r = true;
            }
            return r;
            
        }
        public SmallOneUp SmallOneUp()
        {
            return new SmallOneUp(SmallOneUpSheet);
        }
        public IItem BlockedSuperMushroom(float block)
        {
            int direction = CheckMario(block) ? 1 : -1;
            return new SuperMushroom(SuperMushroomSpriteSheet,direction);
        }
        public IItem OneUpMushroomItem(float block)
        {
            int direction = CheckMario(block) ? -1 : 1;
            return new OneUpMushroom(OneUpMushroomSpriteSheet,direction);
        }
        public IItem StarItem(float block)
        {
            int direction = CheckMario(block) ? -1 : 1;
            return new Star(StarSpriteSheet,direction);
        }
    }
}
