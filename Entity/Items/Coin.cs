using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;

namespace TechSupportMario.Entity.Items
{
    class Coin : AbstractItem
    {
        private readonly int width, height;
        private Stack<Rectangle> sourceRectangles1;
        private Stack<Rectangle> sourceRectangles2;
        private double previousTime = 0;
        private readonly double frameRate = 80;

        public Coin(Texture2D texture) : base(texture, EntityType.Coin)
        {

            MaxHeightChange = 90;
            Velocity = new Vector2(0, -2);
            width = texture.Width;
            height = texture.Height;
            Rows = 1;
            Columns = 4;
            CreateRectangles();
            NeedsUpdating = true;
        }

        public override void BounceFromBlock(Vector2 position)
        {
            SoundFactory.Instance.Coin();
            FromBlock = true;
            Anchor = (int)position.Y;
            Position = new Vector2(position.X+2, position.Y - Texture.Height);
            Velocity = new Vector2(0, -1.75f);
            Collidable = false;
            Stage.AddEntity(this);
        }
        private void CreateRectangles()
        {
            sourceRectangles1 = new Stack<Rectangle>();
            sourceRectangles2 = new Stack<Rectangle>();
            for(int i = 0; i < Columns; i++)
            {
                Rectangle temp = new Rectangle(i * (width / Columns), 0, width / Columns, height / Rows);
                sourceRectangles1.Push(temp);
            }
            ChangeSource();
        }

        private void ChangeSource()
        {
            SourceRectangle = sourceRectangles1.Peek();
            sourceRectangles2.Push(sourceRectangles1.Pop());
            if (sourceRectangles1.Count == 0)
            {
                sourceRectangles1 = sourceRectangles2;
                sourceRectangles2 = new Stack<Rectangle>();
            }
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is Mario)
            {
                SoundFactory.Instance.Coin();
                Collect = true;
            }
        }

        public override void Update(GameTime gameTime) 
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - previousTime > frameRate)
            {
                ChangeSource();
                previousTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if(FromBlock)
            {
                Vector2 nextPosition = Position;
                nextPosition.Y += Velocity.Y;
                Position = nextPosition;
                if(Position.Y <= Anchor - MaxHeightChange)
                {
                    Velocity = new Vector2(0, 2);
                    Anchor -= 32;
                }
                else if (Position.Y >= Anchor)
                {
                    Position = new Vector2((int)Position.X, Anchor);
                    Velocity = new Vector2(0, 0);
                    Collect = true;
                    Collidable = false;
                    FromBlock = false;
                }
                else
                {
                    Position += Velocity;
                }
            }
            
           
        }

        public override IEntity Clone()
        {
            Coin coin = new Coin(Texture);
            IEntity clone = coin;
            base.Clone(ref clone);
            coin = (Coin)clone;
            return coin;
        }
    }
}
