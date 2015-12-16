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
        private string data;
        private Player P1, P2, P3, P4, P0;     //player objects , only 5 object

        public StringEvaluator()
        {

        }
        public void conn()
        {


        }
        public void evaluate(String data, Game1 com)
        {
            this.com = com;
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
            else if (lines[0] == "S")
            {                  // S means players initiate
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


        private void initiate_Evaluate(String[] lines)
        {
            for (int x = 2; x < 5; x++)
            {
                string[] sublines = Regex.Split(lines[x], ";");
                for (int y = 0; y < sublines.Length; y++)
                {
                    string[] cell = Regex.Split(sublines[y], ",");
                    int a = Int32.Parse(cell[0]) * 50;
                    int b = Int32.Parse(cell[1]) * 50;
                    Vector2 v = new Vector2(a, b);
                    if (x == 2)
                        com.bricks.Add(v);
                    if (x == 3)
                        com.stones.Add(v);
                    if (x == 4)
                        com.water1.Add(v);

                    // Button bn= com.selectbtn(a, b);
                    //  com.updateBtn(bn,x);
                }
            }
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
            // Button bn = com.selectbtn(x,y);

            //   Thread coin_thread = new Thread(()=>com.coinUpdate(bn,time)); //create new thread to update coin ;
            //   coin_thread.Start();                                          //start thread
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
            //  Button bn = com.selectbtn(x, y);

            //  Thread coin_thread = new Thread(() => com.lifeUpdate(bn, time)); //create new thread to update life Packt ;
            //  coin_thread.Start();


        }

        //create new tank/players
        private void newPlayer(String[] lines)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                string[] sublines = Regex.Split(lines[i], ";");

                string[] location = Regex.Split(sublines[1], ",");
                int x = Int32.Parse(location[0]);
                int y = Int32.Parse(location[1]);
                int dir = Int32.Parse(sublines[2]);

                if (sublines[0] == "P0")
                {
                    //P1 = new Player(x,y,dir); 
                    com.player0.setX(x);
                    com.player0.setY(y);
                }
                else if (sublines[0] == "P1")
                {
                    //P2 = new Player(x,y,dir); 
                    com.player1.setX(x);
                    com.player1.setY(y);
                }
                else if (sublines[0] == "P2")
                {
                    //P3 = new Player(x,y,dir);  
                    com.player2.setX(x);
                    com.player2.setY(y);
                }
                else if (sublines[0] == "P3")
                {
                    //P4 = new Player(x,y,dir);  
                    com.player3.setX(x);
                    com.player3.setY(y);
                }
                else if (sublines[0] == "P4")
                {
                    //P0 = new Player(x,y,dir);   
                    com.player4.setX(x);
                    com.player4.setY(y);
                }
                // Button bn = com.selectbtn(x, y);
                // com.tankDisplay(bn, sublines[0], dir);

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
                int d = Int32.Parse(sublines[2]);
                bool s;
                if (sublines[3] == "1")
                    s = true;
                else
                    s = false;
                int h = Int32.Parse(sublines[4]);
                int c = Int32.Parse(sublines[5]);
                int p = Int32.Parse(sublines[6]);
              //  int x0 = 0;
              //  int y0 = 0;
                if (h != 0)
                {
                    if (sublines[0] == "P1")
                    {
                       // P1.move(x1, y1, d, s, h, p, c);
                       // x0 = P1.getPreviousX();
                       // y0 = P1.getPreviousY();
                        com.player0.setX(x1);
                    }
                    else if (sublines[0] == "P2")
                    {
                        P2.move(x1, y1, d, s, h, p, c);
                        x0 = P2.getPreviousX();
                        y0 = P2.getPreviousY();
                    }
                    else if (sublines[0] == "P3")
                    {
                        P3.move(x1, y1, d, s, h, p, c);
                        x0 = P3.getPreviousX();
                        y0 = P3.getPreviousY();
                    }
                    else if (sublines[0] == "P4")
                    {
                        P4.move(x1, y1, d, s, h, p, c);
                        x0 = P4.getPreviousX();
                        y0 = P4.getPreviousY();
                    }
                    else if (sublines[0] == "P0")
                    {
                        P0.move(x1, y1, d, s, h, p, c);
                        x0 = P0.getPreviousX();
                        y0 = P0.getPreviousY();
                    }
                    //     Button bn = com.selectbtn(x1, y1);

                    //    Button pre = com.selectbtn(x0, y0);
                    //    com.tankMove(pre, bn, sublines[0], d);
                }
                else
                {
                    if (sublines[0] == "P1")
                    {

                        x0 = P1.getPreviousX();
                        y0 = P1.getPreviousY();
                    }
                    else if (sublines[0] == "P2")
                    {

                        x0 = P2.getPreviousX();
                        y0 = P2.getPreviousY();
                    }
                    else if (sublines[0] == "P3")
                    {

                        x0 = P3.getPreviousX();
                        y0 = P3.getPreviousY();
                    }
                    else if (sublines[0] == "P4")
                    {

                        x0 = P4.getPreviousX();
                        y0 = P4.getPreviousY();
                    }
                    else if (sublines[0] == "P0")
                    {

                        x0 = P0.getPreviousX();
                        y0 = P0.getPreviousY();
                    }
                    // Button bn = com.selectbtn(x1, y1);
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
                // Button bn2 = com.selectbtn(x2, y2);
                // com.brickDamage(bn2, damage);

            }

        }

    }
}
