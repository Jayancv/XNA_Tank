using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class Coin
    {
       
        Game1 com;
        public int x { get; set; }
        public int y{ get; set; }
        public int value { get; set; }
        public int time { get; set; }
        public int appearTimeStamp { get; set; }
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
