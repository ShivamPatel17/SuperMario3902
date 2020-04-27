using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.BlockState;

namespace TechSupportMario.Entity.Block
{
    public class QuestionBlock : AbstractBlock
    {
        private readonly int width, height;
        private Queue<Rectangle> sourceRectangle;
        private int lastChange = 0;

        public override Rectangle CollisionBox
        {
            get
            {
                if (BlockState is BlockStateBump)
                {
                    Rectangle rec = new Rectangle(new Point((int)Position.X, (int)Position.Y - SourceRectangle.Height), SourceRectangle.Size);
                    rec.Height = ((BlockStateBump)BlockState).Anchor - rec.Y;

                    return rec;
                }
                return base.CollisionBox;
            }
        }

        public QuestionBlock(Texture2D texture) : base(texture, EntityType.Question)
        {
            width = texture.Width;
            height = texture.Height;
            Rows = 1;
            Columns = 5;
            CreateRectangles();
            lastChange = 0;

            SourceRectangle = new Rectangle(0, 0, width / Columns, height);
            BlockState = new QuestionBlockStateNormal(this);
            BlockState.Enter();
            Opacity = 1f;
            NeedsUpdating = true;
        }

        private void CreateRectangles()
        {
            sourceRectangle = new Queue<Rectangle>();
            for (int i = 0; i < Columns - 1; i++) // We don't want the last frame
            {
                Rectangle temp = new Rectangle(i * (width / Columns), 0, width / Columns, height / Rows);
                sourceRectangle.Enqueue(temp);
            }
            ChangeSource();
        }

        private void ChangeSource()
        {
            SourceRectangle = sourceRectangle.Peek();
            sourceRectangle.Enqueue(sourceRectangle.Dequeue());
        }
        

        public override void CollisionTransition(Mario mario)
        {
            NeedsUpdating = true;
            if (Items.Count > 0)
            {
                Items.Dequeue().Execute();
            }
          
            BlockState.Collision(mario);
        }
        public override void CollisionTransition()
        {
            NeedsUpdating = true;
            if (Items.Count > 0)
            {
                Items.Dequeue().Execute();
            }

            BlockState.Collision();
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if (entity is Mario)
            {
                if (direction == CollisionDetection.Direction.down || direction == CollisionDetection.Direction.downnov)
                {
                    CollisionTransition((Mario)entity);
                }
            }
        }

        public void StraightToUsed()
        {
            BlockState = new BlockStateUsed(this);
            BlockState.Enter();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2((int)Position.X, (int)Position.Y - SourceRectangle.Height), SourceRectangle, Color.White * Opacity);
        }

        public override void Update(GameTime gameTime)
        {
            if (!(BlockState is BlockStateUsed))
            {
                if (lastChange > Frame / 2)
                {
                    ChangeSource();
                    lastChange = 0;
                }
                else
                {
                    lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                BlockState.Update(gameTime);

            }
        }

        public override IEntity Clone()
        {
            QuestionBlock block = new QuestionBlock(Texture);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (QuestionBlock)clone;
            block.lastChange = lastChange;
            return block;
        }
    }
}

