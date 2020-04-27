using Microsoft.Xna.Framework.Graphics;
using System;

namespace TechSupportMario.Entity.Block
{
    public class DisappearingBlock : BasicBlock
    {

        public DisappearingBlock(Texture2D texture) : base(texture, EntityType.Disappearing)
        {

        }

        public void HandleButtonPress(object sender, EventArgs args)
        {
            Opacity = 0f;
            Collidable = false;
        }

        public void Reset()
        {
            Opacity = 1f;
            Collidable = true;
        }
    }
}
