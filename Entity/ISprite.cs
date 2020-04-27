
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechSupportMario.Entity
{
    public interface ISprite
    {
        Vector2 Position { get; set; }
        float Opacity { get; set; }
        float Order { get; set; }
        Texture2D Texture { get; set; }
        Rectangle CollisionBox { get; }
        bool Collidable { get; set; }
        /// <summary>
        /// true means animated but not moving or needs to update for some other reason, if it is moving then it will be
        /// updated in the moving loop and doesn't need to be updated in the animation update loop. 
        /// Making it easier to manage the quad tree as the moving loop removes and reinserts the sprites 
        /// and the animation loop only calls update on the sprite.
        /// </summary>
        bool NeedsUpdating { get; set; }
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}
