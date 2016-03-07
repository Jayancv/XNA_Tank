using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using tankUI.Inside;
using tankUI.AI;

namespace tankUI.Inside
{
    class Client
    {
        private TcpClient client;       //variables for client
        private string ip = "127.0.0.1";
        private Game1 com;
        public Game2 game;
        private StringEvaluator eval;
        private Thread thread;      //creating  the thread
        private Int32 portIn = 6000;   //port use to connect
        private Int32 portOut = 7000;  //port to recieve

        public string data;
        //public int clock;


       
        private bool errorOcurred;
        int attempt;
       

        private Cell nextMove;
        private Search ai;
        private bool packPresents;

        public Client(Game1 com)
        {
            thread = new Thread(new ThreadStart(recieve));      //create new thread object
            eval = new StringEvaluator();                    //create eval object
            this.com = com;
            game = new Game2();
            ai = new Search(game);
            packPresents = false;
            errorOcurred = false;
            //clock = 0;
        }


        public void shoot()
        {
            send("SHOOT#");
           
        }
        //to send message to the server
        public void send(string message)
        {
                                               //initiate variable
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ip), portIn);
            Stream stream = client.GetStream();

            ASCIIEncoding asciiencode = new ASCIIEncoding();
            byte[] msg = asciiencode.GetBytes(message);

            stream.Write(msg, 0, msg.Length);
            stream.Close();
            client.Close();
            if (message.Equals("JOIN#"))    //starts the game with the command JOIN#
                thread.Start();
        }


        //to get messages from server
        public void recieve()
        {
            TcpListener listner = new TcpListener(IPAddress.Parse(ip), portOut);
            while (true)
            {
                listner.Start();
                TcpClient reciever = listner.AcceptTcpClient();
                Stream r_stream = reciever.GetStream();
                Byte[] bytes = new Byte[256];

                int i;
                data = null;
                try
                {
                    while ((i = r_stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    }
                    string[] lines = Regex.Split(data, ":");
                    //eval.evaluate(data);
                    com.call();
                    if (data.StartsWith("G")){
                    
                     //send("RIGHT#");
                   //  Gamer.gameClock++;
                    // com.gameClock += 1;
                        
                            //  Console.WriteLine("inide G");
                            // to keep syn the game clock with server clock
                            game.gameClock += 1;

                            Console.WriteLine("Game Clock is jcv" + game.gameClock);

                            try
                            {
                                game.addPacksToBoard();
                               // game.updatePacks(game.gameClock);

                                Console.WriteLine("------------befor--------- " + nextMove);
                                //path finding starts ( The whole ai business happens here..... )
                                nextMove = ai.findPath(game);
                                Console.WriteLine("------------next--------- "+nextMove);

                                /*  //solution to the exception (if me and an enemy tries to  jump to a cell simultaniously
                                String gameCell = game.board[nextMove.x, nextMove.y];
                                if (gameCell == "0" || gameCell == "1" || gameCell == "2" || gameCell == "3" || gameCell == "4")
                                {
                                    continue;
                                }
                                */
                                int currentX = game.me.x;
                                int currentY = game.me.y;

                                //    Console.WriteLine("\nNextX:- " + nextMove.x + " NextY:- " + nextMove.y + "\n");



                                // no movements if there isn't a reachable goal on the board
                                if (nextMove.x != currentX || nextMove.y != currentY)
                                {

                                    packPresents = true;
                                }

                                // #### Detect an Enemy who can be shoot out ####
                                /* STEPS

                                1. A* search other players
                                2. filter the straight paths (enemy can see mee - I can see enemy) - using a new method in Ai class
                                3. cancel the other tasks
                                4. if health is low? don't engage (hide) : engage (shoot) face to face
                            
                                */

                                if (game.enemyPresents)
                                {
                                    //  Console.WriteLine("I can see an enemy !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                                    int myDirection = game.me.getDirection();
                                    //    Console.WriteLine("My direction is " + myDirection);
                                    // shoot
                                    if (nextMove.x == currentX + 1)
                                    {
                                        if (myDirection == 1)
                                        {
                                            shoot();


                                        }
                                        // if Im gonna shoot, instead of protecting myself                            


                                        else
                                        {
                                            send("RIGHT#");
                                        }

                                        r_stream.Close();
                                        listner.Stop();
                                        reciever.Close();
                                        game.enemyPresents = false;
                                        continue;

                                    }

                                    else if (nextMove.x == currentX - 1)
                                    {
                                        if (myDirection == 3)
                                        {
                                            shoot();

                                        }

                                        // if Im gonna shoot, instead of protecting myself                            


                                        else
                                        {
                                            send("LEFT#");
                                        }

                                        r_stream.Close();
                                        listner.Stop();
                                        reciever.Close();
                                        game.enemyPresents = false;
                                        continue;
                                    }
                                    else if (nextMove.y == currentY + 1)
                                    {
                                        if (myDirection == 2)
                                        {
                                            shoot();

                                        }

                                        // if Im gonna shoot, instead of protecting myself                            


                                        else
                                        {
                                            send("DOWN#");
                                        }

                                        r_stream.Close();
                                        listner.Stop();
                                        reciever.Close();
                                        game.enemyPresents = false;
                                        continue;
                                    }
                                    else if (nextMove.y == currentY - 1)
                                    {
                                        if (myDirection == 0)
                                        {
                                            shoot();
                                        }

                                        // if Im gonna shoot, instead of protecting myself                            

                                        else
                                        {
                                            send("UP#");
                                        }

                                        r_stream.Close();
                                        listner.Stop();
                                        reciever.Close();
                                        game.enemyPresents = false;
                                        continue;

                                    }

                                    game.enemyPresents = false;
                                }


                                // TO DO:- initialy tank direction is up, it wants to go right... timeCostToTarget is lack of the time to turn right... has to fix this.             


                                if (packPresents)
                                {
                                    //    Console.WriteLine("inside pack presents");
                                    //    Console.WriteLine(currentX+","+ currentY+ " next move:- " + nextMove.x+ "," + nextMove.y);

                                    // move the tank
                                    if (nextMove.x == currentX + 1)
                                    {
                                        send("RIGHT#");
                                    }
                                    else if (nextMove.x == currentX - 1)
                                    {
                                        send("LEFT#");
                                    }
                                    else if (nextMove.y == currentY + 1)
                                    {
                                        send("DOWN#");
                                    }
                                    else if (nextMove.y == currentY - 1)
                                    {
                                        send("UP#");
                                    }
                                    packPresents = false;
                                }
                                else
                                {
                                    shoot();
                                }

                            }
                            catch (Exception ex)
                            {
                            
                               // shoot();
                                continue;
                            }

                        }
                    
                    
                   

                    r_stream.Close();
                    listner.Stop();
                    reciever.Close();
                }
                catch (Exception e) { }
            }
        }
      


    }
}
