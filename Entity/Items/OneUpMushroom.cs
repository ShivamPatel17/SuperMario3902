using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TechSupportMario.Entity.Items
{
    class OneUpMushroom : AbstractItem
    {
        public OneUpMushroom(Texture2D texture, int direction) : base(texture, EntityType.OneUp)
        {
            Direction = direction;
            MaxHeightChange = 33;
        }

        public override IEntity Clone()
        {
            OneUpMushroom one = new OneUpMushroom(Texture, Direction);
            IEntity clone = one;
            base.Clone(ref clone);
            one = (OneUpMushroom)clone;
            return one;
        }

        public override void Update(GameTime gameTime)
        {
            if (Collidable)
                base.Update(gameTime);
            if (FromBlock)
            {
                Vector2 nextPosition = Position;
                nextPosition.Y += Velocity.Y;
                Position = nextPosition;
                if (Position.Y <= Anchor - MaxHeightChange)
                {
                    Velocity = new Vector2(0, 0.25f);
                    Anchor -= 32;
                }
                else if (Position.Y >= Anchor)
                {
                    Position = new Vector2((int)Position.X, Anchor);
                    Collidable = true;
                    Velocity = new Vector2(0.5f*Direction,0);
                    FromBlock = false;
                }
                else
                {
                    Position += Velocity;
                }
            }
            if (Collidable){
                Position += Velocity;
            }
        }
    }
}
