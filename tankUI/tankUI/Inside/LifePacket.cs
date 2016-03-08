using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class LifePacket                                             //lifePacket object 
    {
       
        //lifePacket location
        public int locationX { get; set; }
        public int locationY { get; set; }

        public int lifeTime { get; set; }                         //The visible time of the life packet

        public int appearTimeStamp { get; set; }                  // Time stamp of the lifepackect beggining


        public LifePacket(int locationX, int locationY, int lifeTime, int appearTimeStamp) {  //LifePacket  constructor , it set all  the variables when life packet creating
            this.locationX = locationX;
            this.locationY = locationY;
            this.lifeTime = lifeTime;
            this.appearTimeStamp = appearTimeStamp;
          
        }
    }
}
