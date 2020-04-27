using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.CollisionZones
{
    public class Checkpoint : LimitedHiddenZone
    {
        private Rectangle bar;
        private int currentTexture;
        private int lastChange;

        public Checkpoint(Rectangle box, Texture2D texture) : base(box, texture)
        {
            SourceRectangle = new Rectangle(new Point(0, 0), new Point(64, 128));
            bar = new Rectangle(new Point(1024, 0), new Point(38, 6));
            currentTexture = 0;
            lastChange = 0;
            NeedsUpdating = true;
        }

        public override void Update(GameTime gameTime)
        {
            if(lastChange < 2*Frame/3)
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                currentTexture++;
                lastChange = 0;
                if(currentTexture > 15)
                {
                    currentTexture = 0;
                }
                SourceRectangle = new Rectangle(new Point(SourceRectangle.Width * currentTexture, 0), SourceRectangle.Size);
            }
            if (used && bar.Width > 6)
            {
                bar = new Rectangle(bar.Location, new Point(6, bar.Height));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y - SourceRectangle.Height), SourceRectangle, (Color.White * Opacity), 0f, Vector2.Zero, 1f, SpriteEffects.None, Order);
            spriteBatch.Draw(Texture, new Vector2(Position.X + 10, Position.Y - 2 * SourceRectangle.Height / 3), bar, (Color.White * Opacity), 0f, Vector2.Zero, 1f, SpriteEffects.None, Order);

        }

    }
}
