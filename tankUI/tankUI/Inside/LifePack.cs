using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    class LifePack
    {
        int life;
        int time;
        int x;
        int y;
       // Form1 com;
        public LifePack(int x, int y, int time, int val)
        {
            this.time = time;
            this.life = val;
            this.x = x;
            this.y = y;
           // this.com = com;

        }

    }
}
