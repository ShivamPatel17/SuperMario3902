using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TechSupportMario.Entity.Block
{
    class ToggleBlock : AbstractBlock
    {
        private int lastChange = 0;
        private const int HoldChange = 64;
        private bool justToggled = false;
        public int StartY { get; set; }

        public ToggleBlock(Texture2D texture): base(texture, EntityType.Toggle)
        {
            NeedsUpdating = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (justToggled)
            {
                if (lastChange < HoldChange)
                {
                    lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    lastChange = 0;
                    justToggled = false;
                }
            }
        }

        public void HandleButtonPress(object sender, EventArgs args)
        {
            if(!justToggled)
            {
                Collidable = !Collidable;
                justToggled = true;
                if (Collidable)
                {
                    SourceRectangle = new Rectangle(new Point(0, Texture.Height / 2), new Point(Texture.Width, Texture.Height / 2));
                }
                else
                {
                    SourceRectangle = new Rectangle(new Point(0, 0), new Point(Texture.Width, Texture.Height / 2));
                }
            }
        }

        public void Reset()
        {
            SourceRectangle = new Rectangle(new Point(0, StartY), new Point(Texture.Width, Texture.Height / 2));
            Collidable = StartY == SourceRectangle.Height / 2;
        }

        public override IEntity Clone()
        {
            return null;
        }
    }
}
