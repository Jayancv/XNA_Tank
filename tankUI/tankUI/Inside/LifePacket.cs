using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class LifePacket
    {
       
         public int locationX { get; set; }
        public int locationY { get; set; }

        public int lifeTime { get; set; }

        public int appearTimeStamp { get; set; }


        public LifePacket(int locationX, int locationY, int lifeTime, int appearTimeStamp)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.lifeTime = lifeTime;
            this.appearTimeStamp = appearTimeStamp;
          
        }
     

    }
}
