using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.AI
{
    struct Point
    {
        // cell location on the grid
        public readonly int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
