using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.SoundItems;
using TechSupportMario.State.BlockState;

namespace TechSupportMario.Entity.Block
{
    public class BrickBlock : AbstractBlock
    {
        private readonly List<ISprite> brokenParts;

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

        public BrickBlock(Texture2D texture) : base(texture, EntityType.Brick)
        {
            Opacity = 1f;
            SourceRectangle = new Rectangle(0 , 0, texture.Width, texture.Height / 3);
            BlockState = new BrickBlockStateNormal(this);
            BlockState.Enter();

            brokenParts = new List<ISprite>
            {
                new BrokenBlock(Texture, new Vector2(), 1),
                new BrokenBlock(Texture, new Vector2(), 2),
                new BrokenBlock(Texture, new Vector2(), 3),
                new BrokenBlock(Texture, new Vector2(), 4)
            };
            NeedsUpdating = true;
        }

        public override void CollisionTransition(Mario mario)
        {
            NeedsUpdating = true;

            foreach (ISprite sprite in brokenParts)
            {
                ((BrokenBlock)sprite).SetPosition((int)Position.X, (int)Position.Y);
            }
            BlockState.Collision(mario);
        }
        public override void CollisionTransition()
        {
            NeedsUpdating = true;

            foreach (ISprite sprite in brokenParts)
            {
                ((BrokenBlock)sprite).SetPosition((int)Position.X, (int)Position.Y);
            }
            BlockState.Collision();
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {
            if(entity is Mario)
            {
                if(direction == CollisionDetection.Direction.down || direction == CollisionDetection.Direction.downnov)
                {
                    CollisionTransition((Mario)entity);
                    SoundFactory.Instance.BrickDes();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            System.Diagnostics.Contracts.Contract.Requires(spriteBatch != null);
            if (!(BlockState is BrickBlockStateDestroyed)) //Since Even Bumping Still use this.
            {
                spriteBatch.Draw(Texture, new Vector2((int)Position.X,(int) Position.Y - SourceRectangle.Height), SourceRectangle, Color.White * Opacity);
            }
            else
            {
                if (BlockState is BrickBlockStateDestroyed)
                {
                    foreach (ISprite sprite in brokenParts)
                    {
                        sprite.Draw(spriteBatch);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(BlockState is BrickBlockStateDestroyed)
            {
                foreach(ISprite sprite in brokenParts)
                {
                    sprite.Update(gameTime);
                }

            }else
            {
                 BlockState.Update(gameTime);
            }
        }

        public override IEntity Clone()
        {
            BrickBlock block = new BrickBlock(Texture);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (BrickBlock)clone;
            return block;
        }
    }
}

