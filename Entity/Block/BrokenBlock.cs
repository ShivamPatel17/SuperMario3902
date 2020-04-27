using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TechSupportMario.Entity.Block
{
    public class BrokenBlock : AbstractEntity
    {
        private double lastChange;
        private Vector2 p0, p1, p2;
        private readonly int part;

        public BrokenBlock(Texture2D text, Vector2 Position, int part) : base(text, EntityType.Basic)
        {
            lastChange = 0.0;
            SetPosition((int)Position.X, (int)Position.Y);
            //setting points
            int breakHeight = text.Height / 3;
            SourceRectangle = new Rectangle(0, breakHeight + (part - 1) * (text.Height / 6), text.Width / 2, text.Height / 6);
            this.part = part;
            SetPosition((int)Position.X, (int)Position.Y - SourceRectangle.Height * 2);
            Opacity = 1f;
            NeedsUpdating = true;
            Collidable = false;
        }

        public void SetPosition(int x, int y)
        {
            p0 = new Vector2(x, y);
            switch (part)
            {
                case 1:
                    p1.X = p0.X - 8;
                    p1.Y = p0.Y;
                    p2.X = p0.X - 24;
                    p2.Y = p0.Y + 48;
                    break;
                case 2:
                    p1.X = p0.X - 16;
                    p1.Y = p0.Y - 24;
                    p2.X = p0.X - 36;
                    p2.Y = p0.Y + 48;
                    break;
                case 3:
                    p1.X = p0.X + 16;
                    p1.Y = p0.Y - 24;
                    p2.X = p0.X + 36;
                    p2.Y = p0.Y + 48;
                    break;
                case 4:
                    p1.X = p0.X + 8;
                    p1.Y = p0.Y;
                    p2.X = p0.X + 24;
                    p2.Y = p0.Y + 48;
                    break;

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (lastChange <= 1)
                spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White);
            else
                Collect = true;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = new Vector2
            {
                X = (float)(Math.Pow((1.0 - lastChange), 2) * p0.X + 2 * (1.0 - lastChange) * lastChange * p1.X + lastChange * lastChange * p2.X),
                Y = (float)(Math.Pow((1.0 - lastChange), 2) * p0.Y + 2 * (1.0 - lastChange) * lastChange * p1.Y + lastChange * lastChange * p2.Y)
            };
            Position = pos;
            lastChange += 0.03;
        }

        public override IEntity Clone()
        {
            BrokenBlock block = new BrokenBlock(Texture, new Vector2(p0.X, p0.Y), part);
            IEntity clone = block;
            base.Clone(ref clone);
            block = (BrokenBlock)clone;
            return block;
        }
    }
}
