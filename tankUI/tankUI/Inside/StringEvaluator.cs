using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace tankUI.Inside
{
    class StringEvaluator
    {

        private Game1 com;
        private Game2 game;
        private string data;
        public Player P1, P2, P3, P4, P0;     //player objects , only 5 object
        int counter;
       

        public StringEvaluator()
        {
            counter = 0;
        }
        public void conn()
        {


        }
        public void evaluate(String data, Game1 com ,Game2 game)
        {
            try { 
            this.com = com;
            this.game = game;
            this.data = data;
            data = data.Remove(data.Length - 1);
            string[] lines = Regex.Split(data, ":");    //split recevied data sting and split it :
            if (lines[0] == "I")                        //Check 1st part of the server msg
            {                                           //if 1st letter I means initiate game map
                initiate_Evaluate(lines);
            }
            else if (lines[0] == "C")                   // C means new coin created in the map
            {
                coin(lines);
            }
            else if (lines[0] == "S")                   // S means players initiate
            {                  
                 newPlayer(lines);
            }
            else if (lines[0] == "G")                   // G means Game world updates
            {
                tankMoves(lines);
            }
            else if (lines[0] == "L")                   // L means life packet
            {
                life(lines);
            }
            }
            catch (NullReferenceException e)
            {
            }
        }


        private void initiate_Evaluate(String[] lines)
        {
            for (int x = 2; x < 5; x++)
            {
                string[] sublines = Regex.Split(lines[x], ";");
                for (int y = 0; y < sublines.Length; y++)
                {
                    string[] cell = Regex.Split(sublines[y], ",");
                    int a = Int32.Parse(cell[0]) ;
                    int b = Int32.Parse(cell[1]) ;
                    Vector2 v = new Vector2(a*50, b*50);
                    if (x == 2) { 
                        com.bricks.Add(v);
                    game.board[b, a] = "B";
                       
                    }
                    if (x == 3){
                        com.stones.Add(v);
                    game.board[b, a] = "S";
                    }
                    if (x == 4)
                    {
                        com.water1.Add(v);
                        game.board[b, a] = "W";
                    }
                }
            }
            
            game.brickLen = com.bricks.Count;
            
        }


        //create coins
        private void coin(String[] lines)
        {
            string[] codinate = Regex.Split(lines[1], ",");
            int x = Int32.Parse(codinate[0]);
            int y = Int32.Parse(codinate[1]);
            int val = Int32.Parse(lines[3]);
            int time = Int32.Parse(lines[2]);

            Coin coin = new Coin(x, y, time, val);
            game.Coin.Add(coin);
            Vector2 co = new Vector2(x*50, y*50);
            com.coins.Add(co);
            
            // Button bn = com.selectbtn(x,y);

             Thread coin_thread = new Thread(()=>coinUpdate(time,co)); //create new thread to update coin ;
             coin_thread.Start();                                          //start thread
        }
        public void coinUpdate(int time, Vector2 v)
        {
            Thread.Sleep(time);
            com.coins.Remove(v);
        }

        //create Life Packets
        private void life(String[] lines)
        {
            string[] codinate = Regex.Split(lines[1], ",");
            int x = Int32.Parse(codinate[0]);
            int y = Int32.Parse(codinate[1]);
            int val = 10;
            int time = Int32.Parse(lines[2]);
           
            LifePack life = new LifePack(x, y, time, val);
            game.Lifepacket.Add(life);
            Vector2 li = new Vector2(x * 50, y * 50);
            com.lifePacks.Add(li);
            //  Button bn = com.selectbtn(x, y);

            Thread life_thread = new Thread(() => lifeUpdate(time,li)); //create new thread to update life Packt ;
            life_thread.Start();
        }
        public void lifeUpdate(int time, Vector2 li)
        {
            // coinDisplay(btn);
            Thread.Sleep(time);
            com.lifePacks.Remove(li);
        }

        //create new tank/players
        private void newPlayer(String[] lines)
        {
            if (counter < 5) { 
            for (int i = 1; i < lines.Length; i++)
            {
                string[] sublines = Regex.Split(lines[i], ";");
                string[] location = Regex.Split(sublines[1], ",");
                int x = Int32.Parse(location[0]);
                int y = Int32.Parse(location[1]);
                int dir = Int32.Parse(sublines[2]);

                if (sublines[0] == "P0")
                {
                    P0 = new Player(); 
                    P0.setX(x);
                    P0.setY(y);
                    P0.playerNumber = 1;
                    P0.Color = Color.Red;
                    com.players.Add(P0);
                    counter++;
                    //game.player[0] = P0;
                   game.me = P0;
                    
                }
                else if (sublines[0] == "P1")
                {
                    P1 = new Player(); 
                    P1.setX(x);
                    P1.setY(y);
                    P1.playerNumber = 2;
                    P1.Color = Color.Yellow;
                    com.players.Add(P1);
                    counter++;
                    //game.player[1] = P1;

                }
                else if (sublines[0] == "P2")
                {
                    P2 = new Player();  
                    P2.setX(x);
                    P2.setY(y);
                    P2.playerNumber = 3;
                    P2.Color = Color.Green;
                    com.players.Add(P2);
                    counter++;
                    //game.player[2] = P2;
                }
                else if (sublines[0] == "P3")
                {
                    P3 = new Player();  
                    P3.setX(x);
                    P3.setY(y);
                    P3.playerNumber = 4;
                    P3.Color = Color.Blue;
                    com.players.Add(P3);
                    counter++;
                    //game.player[3] = P3;
                }
                else if (sublines[0] == "P4")
                {
                    P4 = new Player();   
                    P4.setX(x);
                    P4.setY(y);
                    P4.playerNumber = 5;
                    P4.Color = Color.Purple;
                    com.players.Add(P4);
                    counter++;
                   // game.player[4] = P4;
                }
                // Button bn = com.selectbtn(x, y);
                // com.tankDisplay(bn, sublines[0], dir);

            } 
            }
           
        }



        private void tankMoves(String[] lines)
        {
            for (int x = 1; x < lines.Length - 1; x++)
            {
                string[] sublines = Regex.Split(lines[x], ";");
                String[] dire = Regex.Split(sublines[1], ",");
                int x1 = Int32.Parse(dire[0]);
                int y1 = Int32.Parse(dire[1]);
                int d = Int32.Parse(sublines[2]);            //Direction
                bool s;                                      //Shoot
                if (sublines[3] == "1")
                    s = true;
                else
                    s = false;
                int h = Int32.Parse(sublines[4]);             //health
                int c = Int32.Parse(sublines[5]);             //coin
                int p = Int32.Parse(sublines[6]);             //points
              //  int x0 = 0;
              //  int y0 = 0;
                if (h != 0)
                {
                    if (sublines[0] == "P0")
                    {
                       // P1.move(x1, y1, d, s, h, p, c);
                       // x0 = P1.getPreviousX();
                       // y0 = P1.getPreviousY();
                        P0.setX(x1);
                        P0.setY(y1);
                        P0.setVariable(d,s,h,p,c);
                    }
                    else if (sublines[0] == "P1")
                    {
                       // P2.move(x1, y1, d, s, h, p, c);
                       // x0 = P2.getPreviousX();
                       // y0 = P2.getPreviousY();
                        P1.setX(x1);
                        P1.setY(y1);
                        P1.setVariable(d, s, h, p, c);
                    }
                    else if (sublines[0] == "P2")
                    {
                       // P3.move(x1, y1, d, s, h, p, c);
                       // x0 = P3.getPreviousX();
                       // y0 = P3.getPreviousY();
                        P2.setX(x1);
                        P2.setY(y1);
                        P2.setVariable(d, s, h, p, c);
                    }
                    else if (sublines[0] == "P3")
                    {
                       // P4.move(x1, y1, d, s, h, p, c);
                       // x0 = P4.getPreviousX();
                       // y0 = P4.getPreviousY();
                        P3.setX(x1);
                        P3.setY(y1);
                        P3.setVariable(d, s, h, p, c);
                    }
                    else if (sublines[0] == "P4")
                    {
                       // P0.move(x1, y1, d, s, h, p, c);
                       // x0 = P0.getPreviousX();
                       // y0 = P0.getPreviousY();
                        P4.setX(x1);
                        P4.setY(y1);
                        P4.setVariable(d, s, h, p, c);
                    }
                    //     Button bn = com.selectbtn(x1, y1);

                    //    Button pre = com.selectbtn(x0, y0);
                    //    com.tankMove(pre, bn, sublines[0], d);
                }
                    
                else
                {
                    if (sublines[0] == "P1")
                    {
                        com.players.Remove(P1);
                    }
                    else if (sublines[0] == "P2")
                    {
                        com.players.Remove(P2);
                    }
                    else if (sublines[0] == "P3")
                    {
                        com.players.Remove(P3);
                    }
                    else if (sublines[0] == "P4")
                    {
                        com.players.Remove(P4);
                    }
                    else if (sublines[0] == "P0")
                    {
                        com.players.Remove(P0);
                    }
                  
                    // com.tankDisappear(bn);

                }


            }
            
            String dam = lines[lines.Length - 1];
            string[] bric = Regex.Split(dam, ";");
            for (int n = 0; n < bric.Length; n++)
            {
                String[] cod = Regex.Split(bric[n], ",");
                int x2 = Int32.Parse(cod[0]);
                int y2 = Int32.Parse(cod[1]);
                int damage = Int32.Parse(cod[2]);
                if (damage == 4)
                {
                    Vector2 vec1 = new Vector2(x2 * 50, y2 * 50);
                    com.bricks.Remove(vec1);
                }

                // Button bn2 = com.selectbtn(x2, y2);
                // com.brickDamage(bn2, damage);

            }
             

        }

    }
}
