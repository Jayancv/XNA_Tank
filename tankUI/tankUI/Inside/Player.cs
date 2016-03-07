using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
   public  class Player
    {
        public int playerNumber { get; set; }
        public int playerLocationX { get; set; }
        public int playerLocationY { get; set; }
        public int direction { get; set; }

        public int whetherShot { get; set; }
        public int health { get; set; }
        public int coins { get; set; }
        public int points { get; set; }
        public Boolean timeToShot { get; set; }

        /// <summary>
        /// display the player details
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return "\nPlayer Number: " + (playerNumber).ToString() + "\nPlayer Location: " + (playerLocationX).ToString() + "," + (playerLocationY).ToString() + "\nPlayer direction: " + (direction).ToString() + "\nWhether shot: " + (whetherShot).ToString() + "\nCoins: " + (coins).ToString() + "\nHealth : " + (health).ToString() + "\nPoints : " + (points).ToString();
        }
    }
}
