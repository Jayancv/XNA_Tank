using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class Coin
    {                                                        //Coin objects
        // Location of coin 
        public int locationX { get; set; }
        public int locationY { get; set; }

        public int value { get; set; }                //Coin value
        public int lifeTime { get; set; }             //Display time of a coin 
        public int appearTimeStamp { get; set; }      //started time

        public Coin(int locationX, int locationY, int lifeTime, int value, int appearTimeStamp)                            
        {                                                                                          //coin class constructor set all the values
            this.locationX = locationX;
            this.locationY = locationY;
            this.value = value;
            this.lifeTime = lifeTime;
            this.appearTimeStamp = appearTimeStamp;
         
        }
    }
}
