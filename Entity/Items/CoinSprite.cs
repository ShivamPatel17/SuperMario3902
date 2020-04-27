using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportMario.Collisions;

namespace TechSupportMario.Entity.Items
{
    public class CoinSprite : AbstractEntity
    {
      

        public CoinSprite(Texture2D texture) :base(texture, EntityType.Coin)
        {
            SourceRectangle = new Rectangle((texture.Width/ 4), 0, texture.Width/4, texture.Height / 1);
        }

        public override IEntity Clone()
        {
            throw new NotImplementedException();
        }
    }
}
