using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity.Background
{
    class Sprite : ISprite
    {
        public Vector2 Position { get; set; }
        public float Opacity { get; set; }
        public Texture2D Texture { get; set; }
        public bool Collidable { get { return false; } set { ; } }
        public Rectangle CollisionBox => new Rectangle(new Point((int)Position.X, (int)Position.Y - Texture.Height), new Point(Texture.Width, Texture.Height));
        public virtual bool NeedsUpdating { get { return false; } set {; } }
        public Rectangle DrawArea { get; set; }
        public float Order { get; set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Opacity = 1f;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DrawArea != Rectangle.Empty)
                spriteBatch.Draw(Texture, Position, DrawArea, Color.White * Opacity);
            else
                spriteBatch.Draw(Texture, Position, Color.White * Opacity);
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
