using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSupportMario.Entity.MarioEntity
{
    public class MarioCollisionEventArgs : EventArgs
    {

        public IEntity CollidedWith { get; set; }
        public bool TookDamage { get; set; }
        public MarioCollisionEventArgs(IEntity entity, bool damage = false)
        {
            CollidedWith = entity;
            TookDamage = damage;
        }
    }
}
