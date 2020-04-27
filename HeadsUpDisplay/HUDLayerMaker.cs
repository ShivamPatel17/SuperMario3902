using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Background;
using TechSupportMario.Entity.SpriteMaking;

namespace TechSupportMario.HeadsUpDisplay
{
    class HUDLayerMaker
    {
        private ISprite mainHud;
        private ISprite pauseHud;
        private ISprite gameoverHud;
        private ISprite winHud;
        private ISprite arrow;
        private ISprite blackBackground;
        private ISprite winSelectionHud;
        private Texture2D keyHudTexture;
        private List<ISprite> circleBackgrounds;

        public int Order { get; set; }

        private static HUDLayerMaker _instance;
        public static HUDLayerMaker Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new HUDLayerMaker();
                }
                return _instance;
            }
        }

        public void LoadFactory(Game1 game)
        {
            keyHudTexture = game.Content.Load<Texture2D>("HUD/key_hud");

            Texture2D mainHudTexture = game.Content.Load<Texture2D>("HUD/base_hud_texture");
            mainHud = new Sprite(mainHudTexture);
            Texture2D pauseTexture = game.Content.Load<Texture2D>("HUD/pause_menu");
            pauseHud = new Sprite(pauseTexture);
            Texture2D winTexture = game.Content.Load<Texture2D>("HUD/winscreen");
            winHud = new Sprite(winTexture);
            Texture2D arrowTexture = game.Content.Load<Texture2D>("HUD/arrow");
            arrow = new Sprite(arrowTexture);
            Texture2D gameoverTexture = game.Content.Load<Texture2D>("HUD/gameoverscreen");
            gameoverHud = new Sprite(gameoverTexture);
            Texture2D background = game.Content.Load<Texture2D>("EndingCircle/black");
            blackBackground = new Sprite(background);
            circleBackgrounds = new List<ISprite>
            {
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle11")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle10")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle9")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle8")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle7")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle6")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle5")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle4")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle3")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle2")),
                new Sprite(game.Content.Load<Texture2D>("EndingCircle/circle1"))
            };
            winSelectionHud = new Sprite(game.Content.Load<Texture2D>("HUD/winselectionscreen"));
        }

        public Layer NormalHud(Player player, int time, Camera camera)
        {
            Layer layer = new Layer(camera)
            {
                Parallax = Vector2.Zero
            };

            ISet<ISprite> sprites = new HashSet<ISprite>();
            mainHud.Position = new Vector2(62, 1);
            sprites.Add(mainHud);
            WordSprite livesCount = WordAndNumberMaking.Instance.Number(player.Lives, 2, WordAndNumberMaking.CharColor.yellow);
            livesCount.Position = new Vector2(100, 2);
            sprites.Add(livesCount);
            for(int i = 0; i < player.Keys; i++)
            {
                ISprite keyHud = new Sprite(keyHudTexture)
                {
                    Position = new Vector2(175 + (keyHudTexture.Width + 2) * i, 1)
                };
                sprites.Add(keyHud);
            }

            WordSprite timeNum = WordAndNumberMaking.Instance.Number(time, 3, WordAndNumberMaking.CharColor.yellow);
            timeNum.Position = new Vector2(328, 17);
            sprites.Add(timeNum);
            WordSprite coinNum = WordAndNumberMaking.Instance.Number(player.Coins, 1, WordAndNumberMaking.CharColor.white);
            coinNum.Position = new Vector2(camera.DrawRectangle.Width - (coinNum.SourceRectangles.Count * 14) - 3, 2);
            sprites.Add(coinNum);
            WordSprite scoreNum = WordAndNumberMaking.Instance.Number(player.Score, 1, WordAndNumberMaking.CharColor.white);
            scoreNum.Position = new Vector2(camera.DrawRectangle.Width - (scoreNum.SourceRectangles.Count * 14) - 3, 18);
            sprites.Add(scoreNum);
            layer.Sprites = sprites;
            return layer;
        }

        /// <summary>
        /// carrotPos = 1 Continue, 2 = Restart, 3 = Quit
        /// </summary>
        /// <param name="player"></param>
        /// <param name="time"></param>
        /// <param name="camera"></param>
        /// <param name="carrotPos"></param>
        /// <returns></returns>
        public Layer PauseScreen(Player player, int time, Camera camera, int carrotPos)
        {
            //add the normal hud to the game
            Layer layer = NormalHud(player, time, camera);
            //add pause sprites to the hud
            Vector2 pos = new Vector2()
            {
                X = camera.DrawRectangle.Width / 2 - pauseHud.Texture.Width / 2,
                Y = camera.DrawRectangle.Height / 2 - pauseHud.Texture.Height / 3
            };
            pauseHud.Position = pos;
            layer.Sprites.Add(pauseHud);
            layer.Sprites.Add(SelectionArrow(pos, carrotPos));
            return layer;
        }

        public Layer WinScreen(Player player, int time, Camera camera, int mult)
        {
            //add the normal hud to the game
            Layer layer = NormalHud(player, time, camera);

            Vector2 pos = new Vector2()
            {
                X = camera.DrawRectangle.Width / 2 - pauseHud.Texture.Width / 2,
                Y = camera.DrawRectangle.Height / 2 - pauseHud.Texture.Height / 3
            };
            winHud.Position = pos;
            layer.Sprites.Add(winHud);
            //add time and what it equals
            WordSprite timeNum = WordAndNumberMaking.Instance.Number(time, 3, WordAndNumberMaking.CharColor.white);
            timeNum.Position = new Vector2(pos.X + 20, pos.Y + 34);
            layer.Sprites.Add(timeNum);
            WordSprite scorePart = WordAndNumberMaking.Instance.Number(mult, 1, WordAndNumberMaking.CharColor.white);
            scorePart.Position = new Vector2(pos.X + winHud.Texture.Width - (scorePart.SourceRectangles.Count * 12), pos.Y + 34);
            layer.Sprites.Add(scorePart);
            return layer;
        }

        public Layer BlackBackground(float opacity, Camera camera, int circle = -1, Point center = new Point())
        {
            Layer black = new Layer(camera)
            {
                Parallax = Vector2.Zero
            };
            if(circle == -1 || circle >= circleBackgrounds.Count)
            {
                blackBackground.Opacity = opacity;
                black.Sprites.Add(blackBackground);
            }
            else
            {
                black.Parallax = Vector2.One;
                Vector2 pos = circleBackgrounds[circle].Position;
                pos.X = center.X - circleBackgrounds[circle].Texture.Width / 2;
                pos.Y = center.Y - circleBackgrounds[circle].Texture.Height / 2;
                circleBackgrounds[circle].Position = pos;
                black.Sprites.Add(circleBackgrounds[circle]);
            }
            return black;
        }

        public Layer GameOverScreen(Player player, int time, Camera camera, int carrotPos)
        {
            Layer layer = NormalHud(player, time, camera);

            Vector2 pos = new Vector2()
            {
                X = camera.DrawRectangle.Width / 2 - pauseHud.Texture.Width / 2,
                Y = camera.DrawRectangle.Height / 2 - pauseHud.Texture.Height / 3
            };
            gameoverHud.Position = pos;
            layer.Sprites.Add(gameoverHud);
            WordSprite scorePart = WordAndNumberMaking.Instance.Number(player.Score, 1, WordAndNumberMaking.CharColor.white);
            scorePart.Position = new Vector2(pos.X + winHud.Texture.Width - (scorePart.SourceRectangles.Count * 12), pos.Y + 131);
            layer.Sprites.Add(scorePart);
            layer.Sprites.Add(SelectionArrow(pos, carrotPos));
            return layer;
        }

        public Layer WinSelectionScreen(Camera camera, int carrotPos)
        {
            Layer layer = new Layer(camera)
            {
                Parallax = Vector2.Zero
            };
            Vector2 pos = new Vector2()
            {
                X = camera.DrawRectangle.Width / 2 - pauseHud.Texture.Width / 2,
                Y = camera.DrawRectangle.Height / 2 - pauseHud.Texture.Height / 3
            };
            winSelectionHud.Position = pos;
            winSelectionHud.Order = 1f;
            layer.Sprites.Add(winSelectionHud);
            layer.Sprites.Add(SelectionArrow(pos, carrotPos));
            return layer;
        }

        private ISprite SelectionArrow(Vector2 pos, int carrotPos)
        {
            Vector2 arrowPos = new Vector2()
            {
                X = pos.X + 15,
                Y = pos.Y + 48 + (32 * (carrotPos - 1))
            };
            arrow.Position = arrowPos;
            return arrow;
        }

    }
}
