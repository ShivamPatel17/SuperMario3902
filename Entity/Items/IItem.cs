using Microsoft.Xna.Framework;

namespace TechSupportMario.Entity.Items
{
    interface IItem
    {
        void ChangeDirection();
        void BounceFromBlock(Vector2 v);
    }

}
