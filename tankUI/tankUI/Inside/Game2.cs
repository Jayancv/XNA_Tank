using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace tankUI.Inside
{
    public class Game2
    {
        public Player[] player { get; set; }                  // Stores the players
        public Brick[] bricks { get; set; }                   //Get all bricks to brick object array
        public int brickLen { get; set; }                    // brick length

        public bool enemyPresents { get; set; }              //if there is enemy is in the surround 

        public Player myTank { get; set; }
        public int myPlayerNumber { get; set; }              // My player Number
        public int totalPlayers { get; set; }

        public String[] brickWalls { get; set; }
        public String[] stone { get; set; }
        public String[] water { get; set; }
        public List<Coin> Coin { get; set; }                // Coin object list
        public List<LifePacket> Lifepacket { get; set; }    //LifePacket list


        public List<LifePacket> killListLifePack;           // tempory list to kill lifepackets

        public List<Coin> killListCoinPile;                 // tempory list to kill coin piles

        public String[,] gameBoard { get; set; }            // Two dimentional array for the game board

        public String[,] InitialBoard { get; set; }         // Backup of initial board without player locations
        
        public int gameClock { get; set; }                  //for synchronise with server

        public Game2()
        {

            bricks = new Brick[20];                         //there are only 20 bricks 
            gameClock = 1;                                  // Server starts to communicate with client then clock starts

            myTank = null;                                  //In the begging there no tank objects therefor assigned myTank to null

            for (int i = 0; i < 20; i++) {                  //create brick objects 
                bricks[i] = new Brick();
                bricks[i].isFull = true;
            }
            gameBoard = new String[10, 10];                // the grid is 10 by 10 , then create game board as 10*10 array
            InitialBoard = new String[10, 10];

            Coin = new List<Coin>();                       //Creat coin and life pack objectLists
            Lifepacket = new List<LifePacket>();

            killListLifePack = new List<LifePacket>();   
            killListCoinPile = new List<Coin>();
            enemyPresents = false;                         //In the begging assume there is no enemy
        }

        public void initializePlayers(int totalPlayers) {  //create all player objects
            player = new Player[totalPlayers];             
            for (int i = 0; i < totalPlayers; ++i){
                player[i] = new Player();                  //add player objects to player object array
            }
        }






        //--------------Update coins, life packets and players-----------------------

        public void updatePacks(int currentTime)                        
        {
            foreach (var pack in Lifepacket)                            //get all lifepacks in the array
            {
                 if (currentTime >= pack.appearTimeStamp + pack.lifeTime) //check its lifetime exceeded or not
                {                                                         //If it exceed lifetime then update game board("L" to ".") and add to killing list
                    gameBoard[pack.locationY, pack.locationX] = ".";
                    killListLifePack.Add(pack);
                }
            }
            foreach (var i in killListLifePack){                           //Remove lifePackets in the killing list
                Lifepacket.Remove(i);
            }

            foreach (var pack in Coin)                                     // Same as lifepackets
            {
                //Console.WriteLine("current time:- " + currentTime + " start:- " + pack.appearTimeStamp + " lifeTime:- " + pack.lifeTime + " jayan :P");
                if (currentTime >= pack.appearTimeStamp + pack.lifeTime)
                {
                    // Console.WriteLine("Removing expired coin pile..... current time:- " + currentTime + " start:- " + pack.appearTimeStamp + " lifeTime:- " + pack.lifeTime + " wt?");
                    gameBoard[pack.locationY, pack.locationX] = ".";
                    killListCoinPile.Add(pack);
                }
            }
            foreach (var i in killListCoinPile){
                Coin.Remove(i);
            }

            foreach (var p in player){                                     
                if (p.health != 0) {                                       //check all the players health and its not equal to 0 then update the game board
                    gameBoard[p.playerLocationY, p.playerLocationX] = p.playerNumber.ToString();
                }
            }
        }

        public void addPacksToBoard(){                                // For searching algorithm add lifepacks, coins and players to game board graph
            foreach (var pack in Lifepacket)   {
              gameBoard[pack.locationY, pack.locationX] = "L";
            }

            foreach (var pack in Coin){
                gameBoard[pack.locationY, pack.locationX] = "C";
            }

            foreach (var p in player){
                if (p.health != 0) {
                    gameBoard[p.playerLocationY, p.playerLocationX] = p.playerNumber.ToString();
                }
            }
        }
    }
}
    