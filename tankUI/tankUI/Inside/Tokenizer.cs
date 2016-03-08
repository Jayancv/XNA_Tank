using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace tankUI.Inside
{
   public class Tokenizer
    {
        private Game2 game;

        public Tokenizer(Game2 game)
        {
            this.game = game;
        }

        /// <summary>
        /// Parse the server's messege if the JOIN request was accepted
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Acceptance(String text)
        {
            try
            {

                // eg text      S    :     P0;0,0;0    :    P1;0,9;0    #
                // discription  S:P<num>; < player location  x>,< player location  y>:<Direction>#

                // Console.WriteLine(text);

                text = text.Remove(text.Length - 1); // remove the #
                text = text.Remove(0, 2); // remove the S:

                //  Console.WriteLine(text);
                String[] tokens = text.Split(':'); // each token represents a player.

                game.totalPlayers = tokens.Length;
                //  Console.WriteLine(game.totalPlayers);
                game.initializePlayers(game.totalPlayers);

                for (int i = 0; i < game.totalPlayers; i++)
                {

                    // P0;0,0;0 
                    String[] interTokens = tokens[i].Split(';');
                    // P0 0,0 0 

                    game.player[i].playerNumber = int.Parse(interTokens[0].Substring(1, 1)); ;
                    game.player[i].playerLocationX = int.Parse(interTokens[1].Substring(0, 1));
                    game.player[i].playerLocationY = int.Parse(interTokens[1].Substring(2, 1));
                    game.player[i].direction = int.Parse(interTokens[2]);
                }


                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in messege sent from server :- " + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Parse the map details
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Initiation(String text)
        {

            //  I:P<num>: < x>,<y>;< x>,<y>;< x>,<y>…..< x>,<y>: < x>,<y>;< x>,<y>;< x>,<y>…..< x>,<y>: < x>,<y>;< x>,<y>;< x>,<y>…..< x>,<y>#

            text = text.Remove(0, 2); // remove I:
            text = text.Remove(text.Length - 1);  // remove # 


            String[] tokens = text.Split(':');

            game.myPlayerNumber = int.Parse(tokens[0].Substring(1, 1));
            // Console.WriteLine("my player number is " + game.myPlayerNumber);

            //  if (tokens[0].Substring(1, 1).Equals(game.player[game.myPlayerNumber].playerNumber.ToString()))
            //{
            clearBoard();
            game.brickWalls = tokens[1].Split(';');
            game.stone = tokens[2].Split(';');
            game.water = tokens[3].Split(';');
            game.brickLen = game.brickWalls.Length;
            for (int i = 0; i < game.brickWalls.Length; i++)
            {
                String[] j = game.brickWalls[i].Split(',');
                game.gameBoard[int.Parse(j[1]), int.Parse(j[0])] = "B";

            }
            for (int i = 0; i < game.stone.Length; i++)
            {
                String[] j = game.stone[i].Split(',');
                game.gameBoard[int.Parse(j[1]), int.Parse(j[0])] = "S";

            }
            for (int i = 0; i < game.water.Length; i++)
            {
                String[] j = game.water[i].Split(',');
                game.gameBoard[int.Parse(j[1]), int.Parse(j[0])] = "W";

            }
            // keep a copy of the initial board without player locations
            backupBoard();

            // Set the Console Title
            //Console.Title = "Player \"" + game.myPlayerNumber.ToString() + "\" Console - Tank Game";

            // }
            // else
            // {
            //      Console.WriteLine("Player name mismatch while initializing");
            //  }

            return 0;
        }

        /// <summary>
        /// Parse the server's global broadcasting message
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int MovingAnDshooting(String text)
        {
            // restore the board with initial map details
            restoreBoard();

            text = text.Remove(text.Length - 1);
            text = text.Remove(0, 2);
            String[] tokens = text.Split(':');

            //game.totalPlayers = tokens.Length - 1;


            // Reason for the for loop:- number of tokens vary with the number of players connected to the server
            for (int i = 0; i < tokens.Length; i++)
            {


                if (tokens[i].StartsWith("P"))
                {

                    string str = tokens[i].Remove(0, 3);

                    string[] tokens2 = str.Split(';');

                    for (int j = 0; j < tokens2.Length; j++)
                    {
                        // if (game.player[i].health == 0) { continue; }


                        if (j == 0)
                        {

                            game.player[i].playerLocationX = int.Parse(tokens2[j].Substring(0, 1));

                            game.player[i].playerLocationY = int.Parse(tokens2[j].Substring(2, 1));
                        }

                        // Update the map with players 
                        /*  if (int.Parse(tokens2[3]) != 0) // prevent adding dead players to the board
                          {
                              Console.WriteLine("Health is " + int.Parse(tokens2[3]));
                              game.board[game.player[i].playerLocationY, game.player[i].playerLocationX] = i.ToString();
                              Console.WriteLine("Player no " + i);
                          }*/

                        if (j == 1)
                        {
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

                //newly added by prabath
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
                                Console.WriteLine("damages to 4********************************");
                                game.bricks[j].isFull = false;
                            }


                        }

                    }

                }
            }

            return 0;
        }

        /// <summary>
        /// Parse the server message of coin appearences
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Coins(String text)
        {

            text = text.Remove(text.Length - 1);
            text = text.Remove(0, 2);
            string[] tokens = text.Split(':');

            int lifeTimeInMills = int.Parse(tokens[1]);
            if (lifeTimeInMills == 5000) { lifeTimeInMills = 200000; } // spoils of war - set lifetime unlimited
            int lifeTime = lifeTimeInMills / (1000);


            Coin Coin = new Coin(int.Parse(tokens[0].Substring(0, 1)), int.Parse(tokens[0].Substring(2, 1)), lifeTime, int.Parse(tokens[2]), game.gameClock);
            // Console.WriteLine("***Coin*** value = " + Coin.value + " life time = "+ Coin.lifeTime + " X,Y "+ Coin.locationX+","+Coin.locationY);
            game.Coin.Add(Coin);
            return 0;
        }

        /// <summary>
        /// Parse the server message of life packets appearences
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int lifePacks(String text)
        {
            text = text.Remove(text.Length - 1); //remove #
            text = text.Remove(0, 2); // remove L and :
            string[] tokens = text.Split(':');
            LifePacket LifePacket = new LifePacket(int.Parse(tokens[0].Substring(0, 1)), int.Parse(tokens[0].Substring(2, 1)), int.Parse(tokens[1]) / (1000), game.gameClock);
            //  Console.WriteLine("***LifePacket*** life time = " + LifePacket.lifeTime + " X,Y " + LifePacket.locationX + "," + LifePacket.locationY);
            game.Lifepacket.Add(LifePacket);

            return 0;
        }

        /// <summary>
        /// Handles the rest of messages (client request rejections)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Rejection(String text)
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

        /// <summary>
        /// Print the map on the Console
        /// </summary>
        public void printBoard()
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

        /// <summary>
        /// Clear the board
        /// </summary>
        public void clearBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.gameBoard[i, j] = ".";
                }
            }
        }

        /// <summary>
        /// Backup the board
        /// </summary>
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

        /// <summary>
        /// Restore the board from the backup
        /// </summary>
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

    }
}