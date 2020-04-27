using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.SoundItems;

namespace TechSupportMario.Entity.Items
{
    class SuperMushroom : AbstractItem
    {
        public SuperMushroom(Texture2D texture, int direction) : base(texture, EntityType.Super)
        {
            Direction = direction;
            MaxHeightChange = 33;
            SoundFactory.Instance.PowerShow();
        }

        public override IEntity Clone()
        {
            SuperMushroom super = new SuperMushroom(Texture, Direction);
            IEntity clone = super;
            base.Clone(ref clone);
            super = (SuperMushroom)clone;
            return super;
        }

        public override void Update(GameTime gameTime)
        {
            if(Collidable)
                base.Update(gameTime);

            if (FromBlock)
            {
                if (Position.Y <= Anchor - MaxHeightChange)
                {
                    Velocity = new Vector2(0, 0.25f);
                    Anchor -= 32;
                }
                else if (Position.Y >= Anchor)
                {
                    Position = new Vector2((int)Position.X, Anchor);
                    Collidable = true;
                    Velocity = new Vector2(.71f*Direction,0);
                    FromBlock = false;
                    OnGround = true;
                }
                else
                {
                    Position += Velocity;
                }
            }
        }
    }
}
