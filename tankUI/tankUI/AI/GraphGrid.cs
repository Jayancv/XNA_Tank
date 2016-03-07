using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace tankUI.AI
{
    interface GraphGrid<L>
    {
        int Cost(Cell a, Cell b);
        IEnumerable<Cell> Neighbors(Cell id);
    }
}
