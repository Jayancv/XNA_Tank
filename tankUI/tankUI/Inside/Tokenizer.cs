using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace tankUI.Inside
{
   public class Tokenizer                                   // use to split server meaasges and create & update objects
    {
        private Game2 game;                                 

        public Tokenizer(Game2 game){                       //Constructor of tokenizer 
            this.game = game;                               //use game 
        }

        /// <summary>
        /// Parse the server's messege if the JOIN request was accepted
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Acceptance(String text) {                             //server Acceptance message evaluate
            try
            {

                //  S    :  P0    ;    0                ,  0                  ;  0          :    P1;0,9;0      #
                //  S    :  P<num>; < player location x>,< player location  y>;<Direction>  : <another player> #
                text = text.Remove(text.Length - 1);                     // for remove the #
                text = text.Remove(0, 2);                                // for remove the S:

                String[] tokens = text.Split(':');                       // each token represents a player. Player details array

                game.totalPlayers = tokens.Length;                       //update player count
                //  Console.WriteLine(game.totalPlayers);
                game.initializePlayers(game.totalPlayers);               //Initiate game players to game(create all player objects for game)

                for (int i = 0; i < game.totalPlayers; i++)  {           //Get player details one by one

                    // P0;0,0;0 
                    String[] playerDetails = tokens[i].Split(';');         //split player details to plsyer number, player location, player directon
                    // P0 0,0 0 

                    //Update player objects
                    game.player[i].playerNumber = int.Parse(playerDetails[0].Substring(1, 1)); ;   
                    game.player[i].playerLocationX = int.Parse(playerDetails[1].Substring(0, 1));
                    game.player[i].playerLocationY = int.Parse(playerDetails[1].Substring(2, 1));
                    game.player[i].direction = int.Parse(playerDetails[2]);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in messege sent from server :- " + e.Message);
                return -1;
            }
        }


        public int Initiation(String text)              //Game world initiation 
        {

            //  I:    P<num>        : < x>,<y>;< x>,<y>;….< x>,<y>: < x>,<y>;< x>,<y>…..< x>,<y>:< x>,<y>;< x>,<y>…..< x>,<y>#
            //  I:P<my playerNumber>:   <Bricks co-ordinates>     :   <stone co-ordinates>      :    <water co-ordinates>    #

            text = text.Remove(0, 2);                                 //For  remove I:
            text = text.Remove(text.Length - 1);                      //For  remove # 

            String[] tokens = text.Split(':');                        //split to player number,brics, stone and water co-ordinates

            game.myPlayerNumber = int.Parse(tokens[0].Substring(1, 1));  // Upgate my tank number
           
            clearBoard();                                                //put '.' to all the nodes in the graph
            game.brickWalls = tokens[1].Split(';');                      //add bricks co ordinations to brickwall array 
            game.stone = tokens[2].Split(';');                           //same as bricks
            game.water = tokens[3].Split(';');

            game.brickLen = game.brickWalls.Length;               
    

            //---------- update game board graph(add bricks,stones and water)-------------
            for (int i = 0; i < game.brickWalls.Length; i++)  {
                String[] j = game.brickWalls[i].Split(',');                           // split loacation to x and y
                game.gameBoard[int.Parse(j[1]), int.Parse(j[0])] = "B";               //put "B" where the bricks located
            }
            for (int i = 0; i < game.stone.Length; i++) {
                String[] j = game.stone[i].Split(',');
                game.gameBoard[int.Parse(j[1]), int.Parse(j[0])] = "S";               //put "S" where the stones located

            }
            for (int i = 0; i < game.water.Length; i++) {
                String[] j = game.water[i].Split(',');
                game.gameBoard[int.Parse(j[1]), int.Parse(j[0])] = "W";                //put "W" where the water located

            }

            // keep a copy of the initial board without player locations
            backupBoard();

            return 0;
        }

   
        public int MovingAnDshooting(String text)   {                      //player updates 

            restoreBoard();                                                // restore the board with initial map details

            text = text.Remove(text.Length - 1);                          //for remove #
            text = text.Remove(0, 2);                                     //for frmove G:
            String[] tokens = text.Split(':');

           

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].StartsWith("P")) {
                    //x>,<y>,<damage-level>;< x>,<y>,<damage-level>;< x>,<y>,<damage-level>;< x>,<y>,<damage-level>…..< x>,<y>,<damage-level># 

                    string str = tokens[i].Remove(0, 3);               //for remove p<num>
                    string[] tokens2 = str.Split(';');                 //split to location, damage level
                    for (int j = 0; j < tokens2.Length; j++)
                    {
                        if (j == 0)
                        {
                            game.player[i].playerLocationX = int.Parse(tokens2[j].Substring(0, 1));

                            game.player[i].playerLocationY = int.Parse(tokens2[j].Substring(2, 1));
                        }


                        if (j == 1){
                            game.player[i].direction = int.Parse(tokens2[j]);
                        }
                        if (j == 2)
                        {
                            game.player[i].whetherShot = int.Parse(tokens2[j]);
                            if (game.player[i].whetherShot == 1)
                            {
                                game.player[i].timeToShot = true;
                            }

                        }
                        if (j == 3)
                        {
                            game.player[i].health = int.Parse(tokens2[j]);
                            //  if (game.player[i].playerNumber == game.me.playerNumber) { game.player[i].health = 80; }
                        }
                        if (j == 4)
                        {
                            game.player[i].coins = int.Parse(tokens2[j]);
                        }
                        if (j == 5)
                        {
                            game.player[i].points = int.Parse(tokens2[j]);
                        }
                    }
                }

     
                else
                {
                    string[] tokens3 = tokens[i].Split(';');

                    for (int j = 0; j < tokens3.Length; j++)
                    {

                        if (game.bricks[j].isFull)
                        {
                            game.bricks[j].locationX = int.Parse(tokens3[j].Substring(0, 1));

                            game.bricks[j].locationY = int.Parse(tokens3[j].Substring(2, 1));

                            int damageLevel = int.Parse(tokens3[j].Substring(4, 1));
                            game.bricks[j].damageLevel = damageLevel;

                            if (damageLevel == 4)
                            {
                               
                                game.bricks[j].isFull = false;
                            }


                        }

                    }

                }
            }

            return 0;
        }

   
        public int Coins(String text)                                        //initiate new coins
        {
            //C:<x>,<y>:<LT>:<Val>#
            text = text.Remove(text.Length - 1);           //for remove #
            text = text.Remove(0, 2);                      //remove  C:
            string[] tokens = text.Split(':');             //split to location, lifetime and value


            int lifeTimeInMills = int.Parse(tokens[1]);
            if (lifeTimeInMills == 5000) { lifeTimeInMills = 200000; } // spoils of war - set lifetime unlimited
            int lifeTime = lifeTimeInMills / (1000);

            //create new coin object
            Coin Coin = new Coin(int.Parse(tokens[0].Substring(0, 1)), int.Parse(tokens[0].Substring(2, 1)), lifeTime, int.Parse(tokens[2]), game.gameClock);
            // Console.WriteLine("***Coin*** value = " + Coin.value + " life time = "+ Coin.lifeTime + " X,Y "+ Coin.locationX+","+Coin.locationY);
            
            game.Coin.Add(Coin);                                        // new coin add to game
            return 0;
        }

       
        public int lifePacks(String text)                    //same as coins
        {
            //L:<x>,<y>:<LT>#

            text = text.Remove(text.Length - 1); //remove #
            text = text.Remove(0, 2);            // remove L and :
            string[] tokens = text.Split(':');
            LifePacket LifePacket = new LifePacket(int.Parse(tokens[0].Substring(0, 1)), int.Parse(tokens[0].Substring(2, 1)), int.Parse(tokens[1]) / (1000), game.gameClock);
            //  Console.WriteLine("***LifePacket*** life time = " + LifePacket.lifeTime + " X,Y " + LifePacket.locationX + "," + LifePacket.locationY);
            game.Lifepacket.Add(LifePacket);

            return 0;
        }

     

      
     /*   public void printBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(game.gameBoard[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
      */

     
        public void clearBoard()                           //clear game board; set "." to all the nodes in the graph
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.gameBoard[i, j] = ".";
                }
            }
        }

        public void backupBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.InitialBoard[i, j] = game.gameBoard[i, j];
                }
            }
        }


        public void restoreBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.gameBoard[i, j] = game.InitialBoard[i, j];
                }
            }
            Console.WriteLine("restoring bode");
            foreach (var b in game.bricks)
            {
                if (!(b.isFull))
                {
                    Console.WriteLine("Detected a fully damaged brick!!");
                    game.gameBoard[b.locationY, b.locationX] = ".";
                }
            }
        }

        public int Rejection(String text)   //for other cases
        {
            switch (text)
            {
                case "PLAYERS_FULL#":
                    return 1;
                case "ALREADY_ADDED#":
                    return 2;
                case "GAME_ALREADY_STARTED#":
                    return 3;

                case "OBSTACLE#":
                    return 4;
                case "CELL_OCCUPIED#":
                    return 5;
                case "DEAD":
                    return 6;
                case "TOO_QUICK":
                    return 7;
                case "INVALID_CELL":
                    return 8;
                case "GAME_HAS_FINISHED":
                    return 9;
                case "GAME_NOT_STARTED_YET":
                    return 10;
                case "NOT_A_VALID_CONTESTANT":
                    return 11;
                default: return 0;
            }
        }

    }
}