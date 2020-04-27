using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TechSupportMario.Entity.Enemy
{
    public class DeadEnemy : AbstractEnemy
    {
        private int currentFrame;
        private double time = 0;
        private const double frameRate = 100;

        public DeadEnemy(Texture2D texture, Vector2 position) : base(texture, EntityType.DeadEnemy)
        {
            Position = position;
            currentFrame = 0;
            Columns = 5;
            Rectangle source = SourceRectangle;
            source.Width = texture.Width / 5;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - time > frameRate)
            {
                time = gameTime.TotalGameTime.TotalMilliseconds;
                if (currentFrame < 5)
                {
                    SourceRectangle = new Rectangle(Texture.Width / Columns * currentFrame, 0, Texture.Width / Columns, Texture.Height);
                    currentFrame++;
                }
                else
                {
                    SourceRectangle = new Rectangle();
                    Collect = true;
                }

            }
        }

        public override IEntity Clone()
        {
            DeadEnemy enemy = new DeadEnemy(Texture, new Vector2(Position.X, Position.Y))
            {
                currentFrame = currentFrame,
                time = time
            };
            return enemy;
        }
    }
}
