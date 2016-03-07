using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace tankUI.Inside
{
    public class Game2
    {
        // Stores the players
        public Player[] player { get; set; }
        public Brick[] bricks { get; set; }
        public int brickLen { get; set; }

        public bool enemyPresents { get; set; }

        public Player me { get; set; }


        // My player Number
        public int myPlayerNumber { get; set; }
        public int totalPlayers { get; set; }

        public String[] brickWalls { get; set; }
        public String[] stone { get; set; }
        public String[] water { get; set; }
        public List<Coin> Coin { get; set; }
        public List<LifePacket> Lifepacket { get; set; }

        // tempory list to kill lifepackets
        public List<LifePacket> killListLifePack;

        // tempory list to kill coin piles
        public List<Coin> killListCoinPile;

        // Two dimentional array for the game board
        public String[,] board { get; set; }

        // Backup of initial board without player locations
        public String[,] InitialBoard { get; set; }

        public int timeCostToTarget { get; set; }
        public int gameClock { get; set; }

        public Game2()
        {

            bricks = new Brick[20];
            gameClock = 1;

            me = null;

            for (int i = 0; i < 20; i++)
            {
                bricks[i] = new Brick();
                bricks[i].isFull = true;
            }


            board = new String[10, 10];
            InitialBoard = new String[10, 10];

            Coin = new List<Coin>();
            Lifepacket = new List<LifePacket>();

            killListLifePack = new List<LifePacket>();
            killListCoinPile = new List<Coin>();
            enemyPresents = false;
        }

        public void initializePlayers(int totalPlayers)
        {

            player = new Player[totalPlayers];

            for (int i = 0; i < totalPlayers; ++i)
            {
                player[i] = new Player();
                // player[i].playerNumber = i;
            }
        }


        public void updatePacks(int currentTime)
        {
            foreach (var pack in Lifepacket)
            {
                //Console.WriteLine("current time:- "+currentTime+" start:- "+pack.appearTimeStamp+" lifeTime:- "+pack.lifeTime+" *****************----------------");
                if (currentTime >= pack.appearTimeStamp + pack.lifeTime)
                {
                    board[pack.locationY, pack.locationX] = ".";
                    killListLifePack.Add(pack);
                }
            }
            foreach (var i in killListLifePack)
            {
                Lifepacket.Remove(i);
            }

            foreach (var pack in Coin)
            {
                //Console.WriteLine("current time:- " + currentTime + " start:- " + pack.appearTimeStamp + " lifeTime:- " + pack.lifeTime + " *****************----------------");
                if (currentTime >= pack.appearTimeStamp + pack.lifeTime)
                {
                    // Console.WriteLine("Removing expired coin pile..... current time:- " + currentTime + " start:- " + pack.appearTimeStamp + " lifeTime:- " + pack.lifeTime + " *****************----------------");
                    board[pack.locationY, pack.locationX] = ".";
                    killListCoinPile.Add(pack);
                }
            }
            foreach (var i in killListCoinPile)
            {

                Coin.Remove(i);
            }

            //  Console.WriteLine(player.Length);
            foreach (var p in player)
            {
                if (p.health != 0)
                {
                    board[p.playerLocationY, p.playerLocationX] = p.playerNumber.ToString();
                }
            }
        }

        public void addPacksToBoard()
        {
            foreach (var pack in Lifepacket)
            {

                board[pack.locationY, pack.locationX] = "L";
            }

            foreach (var pack in Coin)
            {

                board[pack.locationY, pack.locationX] = "C";
            }

            foreach (var p in player)
            {
                if (p.health != 0)
                {
                    board[p.playerLocationY, p.playerLocationX] = p.playerNumber.ToString();
                }
            }
        }
    }
}
    