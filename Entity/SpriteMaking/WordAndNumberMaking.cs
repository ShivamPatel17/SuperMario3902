using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Entity.HeadsUpDisplay;
using TechSupportMario.HeadsUpDisplay;

namespace TechSupportMario.Entity.SpriteMaking
{
    class WordAndNumberMaking
    {
        private const int letterWidthHeight = 14;
        private const int numberWidth = 14;
        private const int numberHeight = 12;
        private Point letterSize;
        private Point numberSize;
        private Texture2D yellow;
        private Texture2D white;

        private WordAndNumberMaking() 
        {
            letterSize = new Point(letterWidthHeight);
            numberSize = new Point(numberWidth, numberHeight);
        }

        private static WordAndNumberMaking _instance;
        public static WordAndNumberMaking Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WordAndNumberMaking();
                }
                return _instance;
            }
        }

        public void LoadFactory(Game1 game)
        {
            yellow = game.Content.Load<Texture2D>("Letters_Numbers/yellow_numbers_letters");
            white = game.Content.Load<Texture2D>("Letters_Numbers/white_numbers_letters");
        }

        public enum CharColor
        {
            yellow,
            white
        }

        /// <summary>
        /// Only works if all letters are all leters and are all lowercase
        /// </summary>
        /// <param name="word"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public WordSprite Word(string word, CharColor color)
        {
            char[] chars = word.ToCharArray();
            WordSprite sprite = new WordSprite
            {
                Texture = CharTexture(color)
            };
            for (int i = 0; i < chars.Length; i++)
            {
                Rectangle source = new Rectangle
                {
                    Size = letterSize
                };
                source.Y = 0;
                source.X = (chars[i] - 'a') * letterWidthHeight;
                sprite.SourceRectangles.Add(source);
            }
            return sprite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        public WordSprite Number(int number, int places, CharColor color)
        {
            char[] theNumber = number.ToString().ToCharArray();
            WordSprite sprite = new WordSprite
            {
                Texture = CharTexture(color)
            };
            //add leading zeros
            for(int i = 0; i < places - theNumber.Length; i++)
            {
                sprite.SourceRectangles.Add(new Rectangle(new Point(0, letterWidthHeight), numberSize));
            }
            for(int i = 0; i < theNumber.Length; i++)
            {
                Rectangle source = new Rectangle(new Point(0, letterWidthHeight), numberSize)
                {
                    X = (theNumber[i] - '0') * numberWidth
                };
                sprite.SourceRectangles.Add(source);
            }

            return sprite;
        }

        public Texture2D CharTexture(CharColor color)
        {
            switch (color)
            {
                case CharColor.yellow:
                    return yellow;
                default:
                    return white;
            }
        }

    }
}
