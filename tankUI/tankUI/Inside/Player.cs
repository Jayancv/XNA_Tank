using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
   public  class Player
    {
        private int x0, y0, x, y;
        private int dir;
        private int health, points, coins;
        bool shot;
        public Vector2 Position;
        public bool IsAlive= true;
        public Color Color;
        
        /*
        public Player(Vector2 p, int y, Color c, int dir)
        {
            Position = p;
            Color = c;
            this.dir = dir;
            health = 0;
            points = 0;
            coins = 0;

            shot = false;
        }
         * */
        public Player() {
            
            dir = 0;
        }
        public void move(int x1, int y1, int dir1, bool sh, int h1, int p1, int c1)
        {
            x0 = x;                                //for previous place 
            y0 = y;                                //for previous place 
            x = x1;                                //new place 
            y = y1;
            dir = dir1;
            health = h1;
            points = p1;
            coins = c1;
            shot = sh;

        }
        public int getPreviousX()
        {
            return x0;
        }
        public int getPreviousY()
        {
            return y0;
        }
        public void setX(int x1)
        {
            this.x = x1;
        }
        public void setY(int y1)
        {
            this.y =y1;
        }
        public Vector2 getPossition(){
            Vector2 vect = new Vector2((x*50)+25, (y*50)+25);
            return vect;
        }
        public void setDirection(int d)
        {
            this.dir = d;
        }
        public int getDirection()
        {
            return dir;
        }
        public void setVariable(int d,bool s,int helth, int pt, int c)
        {

            dir = d;
            health = helth;
            points = pt;
            coins = c;
            shot = s;

        }
    }
}
