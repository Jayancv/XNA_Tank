using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class Coin
    {
       public int locationX { get; set; }
        public int locationY { get; set; }
        public int value { get; set; }
        public int lifeTime { get; set; }
        public int appearTimeStamp { get; set; }

        public Coin(int locationX, int locationY, int lifeTime, int value, int appearTimeStamp)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.value = value;
            this.lifeTime = lifeTime;
            this.appearTimeStamp = appearTimeStamp;
         
        }
    }
}
