using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    class Coin
    {
        int value;            //coin value
        int time;             //display time
        int x;
        int y;
        Game1 com;
        public Coin(int x, int y, int time, int val)
        {
            this.time = time;
            this.value = val;
            this.x = x;
            this.y = y;
           // this.com = com;
        }
    }
}
