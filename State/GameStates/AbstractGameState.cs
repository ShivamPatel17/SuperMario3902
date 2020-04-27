using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.CameraItems;

namespace TechSupportMario.State.GameStates
{
    public abstract class AbstractGameState : IGameState
    {
        public Level Context { get; set; }
        public AbstractGameState()
        {

        }
        public virtual void End()
        {
            
        }

        public virtual void Enter()
        {
           
        }

        public virtual void Leave(IState nextState)
        {
            
        }

        public virtual void Restart()
        {

        }

        public virtual void Pause()
        {
            
        }

        public virtual void Start()
        {
            
        }

        public virtual void Update(GameTime gameTime, Camera camera)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {

        }

        public virtual void Warp(int NextArea, int dir)
        {
            
        }

        public virtual void IncrementSelection()
        {
            
        }

        public virtual void DecrementSelection()
        {
            
        }

        public virtual void Selection(Game1 game) { }
    }
}
