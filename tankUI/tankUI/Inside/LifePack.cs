using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    class LifePack
    {
       
        int time;
        public int x { get; set; }
        public int y { get; set; }
        public int life { get; set; }
        public int appearTimeStamp { get; set; }
       // Form1 com;
        public LifePack(int x, int y, int time, int val)
        {
            this.time = time;
            this.life = val;
            this.x = x;
            this.y = y;
            this.appearTimeStamp = appearTimeStamp;
           // this.com = com;

        }
     

    }
}
