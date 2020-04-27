using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.CameraItems;

namespace TechSupportMario.State.GameStates
{
    public interface IGameState:IState
    {
        void Pause();
        void Start();
        void End();
        void Warp(int NextArea, int dir);
        void Update(GameTime gameTime, Camera camera);
        void Draw(SpriteBatch spriteBatch, Camera camera);
        void Restart();
        void IncrementSelection();
        void DecrementSelection();
        void Selection(Game1 game);
    }
}
