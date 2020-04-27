using System;

namespace TechSupportMario.Entity.Block.BlockEventArgs
{
    public class WarpEventArgs : EventArgs
    {
        public WarpEventArgs(int nextArea, int direction)
        {
            NextArea = nextArea;
            Direction = direction;
        }

        public int NextArea { get; set; }
        public int Direction { get; set; }
    }
}
