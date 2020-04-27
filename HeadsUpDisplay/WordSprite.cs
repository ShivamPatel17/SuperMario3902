using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.Entity;

namespace TechSupportMario.HeadsUpDisplay
{
    class WordSprite : ISprite
    {
        public Vector2 Position { get; set; }
        public float Opacity { get; set; }
        public float Order { get; set; }
        public Texture2D Texture { get; set; }
        public IList<Rectangle> SourceRectangles { get; set; }
        public Rectangle CollisionBox { get; set; }
        public bool Collidable { get { return false; } set {; } }
        public bool NeedsUpdating { get { return false; } set {; } }

        public WordSprite()
        {
            SourceRectangles = new List<Rectangle>();
            CollisionBox = new Rectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < SourceRectangles.Count; i++) 
            {
                spriteBatch.Draw(Texture, new Vector2(Position.X + SourceRectangles[i].Width * i, Position.Y), SourceRectangles[i], Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
