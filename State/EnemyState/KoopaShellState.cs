using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Entity.Enemy;

namespace TechSupportMario.State.EnemyState
{
    class KoopaShellState : AbstractEnemyState
    {
        private bool _deadly;
        private bool _swap;
        private int _frameCount;
        public KoopaShellState(AbstractEnemy context) : base(context)
        {

        }
        public override IEnemyState Clone(AbstractEnemy copy)
        {
            return new KoopaShellState(copy);
        }

        public void Right()
        {
            Context.Velocity = new Vector2(1.5f, 0);
        }
        public void Left()
        {
            Context.Velocity = new Vector2(-1.5f, 0);
        }

        public override void Enter()
        {
            ((AbstractKoopa)Context).UpdateTextureAndBox();
            Context.Velocity = new Vector2(0, 0);
            _deadly = false;
            Context.Collidable = false;
            _frameCount = 0;
            _swap = false;
        }

        public override void Leave(IState next)
        {
            Context.EnemyState = (AbstractEnemyState)next;
            ((AbstractKoopa)Context).UpdateTextureAndBox();
            Context.EnemyState.Enter();
        }
        public override void ChangeDirections()
        {
            if(Context.Velocity.X > 0)
            {
                Left();
            } 
            else
            {
                Right();
            }
        }
        public bool GetDeadly()
        {
            return _deadly;
        }
        public bool GetSwap()
        {
            return _swap;
        }

        public override void Update(GameTime gameTime)
        {
            if (((AbstractKoopa)Context).Held)
            {
                Context.Collidable = false;
                _deadly = false;
            }
            else
            {
                if (Context.Velocity.X != 0 && !_deadly)
                {
                    _frameCount += 1;
                    if (_frameCount >= 30)
                    {
                        _deadly = true;
                        Context.Collidable = true;
                        _frameCount = 0;
                    }
                }
                else if (Context.Velocity.X == 0 && !Context.Collidable)
                {
                    _frameCount += 1;
                    if (_frameCount >= 10)
                    {
                        Context.Collidable = true;
                        _frameCount = 0;
                    }
                }
                else if (Context.Velocity.X == 0 && Context.Collidable)
                {
                    _frameCount += 1;
                    if (_frameCount >= 300)
                    {
                        _swap = true;
                    }
                }
            }
            
        }
    }
}
