using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSupportMario.Entity.Items
{
    class SmallOneUp : AbstractEntity
    {
        public SmallOneUp(Texture2D texture) : base(texture, EntityType.Basic)
        {

        }

        public override IEntity Clone()
        {
            throw new NotImplementedException();
        }
    }
}
