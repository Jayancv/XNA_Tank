using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
   public  class Player                                  //Player object class
    {
        public int playerNumber { get; set; }

       //Player location
        public int playerLocationX { get; set; }
        public int playerLocationY { get; set; }

        public int direction { get; set; }

        public int whetherShot { get; set; }           
        public int health { get; set; }                 //store health of the player
        public int coins { get; set; }
        public int points { get; set; }
        public Boolean timeToShot { get; set; }

      
    }
}
