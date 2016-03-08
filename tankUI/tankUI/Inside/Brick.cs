using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class Brick                               //brick objects
    {
        public int locationX { get; set; }
        public int locationY { get; set; }
        public int damageLevel { get; set; }         // there 4 damage levels 
        public Boolean isFull { get; set; }     
    
    }
}
