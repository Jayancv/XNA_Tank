using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Threading;
using tankUI.Inside;
using tankUI.AI;

namespace tankUI.ClientConnector
{
    class Client
    {  // create a Tcp socket  to connect to server
        private static TcpClient _clientSocket = null;


        TcpListener listener = null;
        TcpClient reciever = null;
        Stream r_stream = null;

        private static BinaryWriter writer;
        private Parser parser;
        private static NetworkStream stream = null;

        private Thread thread;
        private Game2 game;

        private bool errorOcurred;
        int attempt;
        private Cell nextMove;
        private Search ai;
        private bool packPresents;


        public Client(Game2 game)
        {
            this.game = game;
            this.parser = new Parser(game);
            thread = new Thread(new ThreadStart(receiveData)); //strat receiveing thread
            ai = new Search(game);  
            packPresents = false;
            errorOcurred = false;

        }

        /// <summary>
        /// connecting to the server socket
        /// </summary>
        public void sendJOINrequest()
        {
            sendData("JOIN#");
            thread.Start();
        }

        /// <summary>
        /// To fetch data from server
        /// </summary>
        public void receiveData()
        {   try
            {

                //Creating listening Socket
                this.listener = new TcpListener(IPAddress.Any, 7000);
                String messageFromServer;
                while (true)
                {
                    //Starts listening
                    listener.Start();
                    reciever = listener.AcceptTcpClient();
                    r_stream = reciever.GetStream();
                    Byte[] bytes = new Byte[256];

                    int i;
                    messageFromServer = null;

                    while ((i = r_stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        messageFromServer = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    }

                   


                    //-------------------------//
                    parser.parse(messageFromServer);

                    if (messageFromServer != null)
                    {
                        if (messageFromServer.StartsWith("G"))
                        {

                            game.gameClock += 1;                           // to keep sync the game clock with server clock
                            try
                            {
                               game.addPacksToBoard();
                               game.updatePacks(game.gameClock);


                            //path searching starts ( AI part starts here )
                            nextMove = ai.findPath(game);


                            int currentX = game.myTank.playerLocationX;       //get my current location
                            int currentY = game.myTank.playerLocationY;

                            // no movements if there isn't a reachable goal on the board (surrent location and nextMove are equals)
                            if (nextMove.x != currentX || nextMove.y != currentY)
                            {
                                packPresents = true;
                            }




                                
                            //--------- Detect an Enemy who can be shoot out ---------------
                            /* STEPS

                            1. A* search other players
                            2. filter the straight paths (enemy can see mee - I can see enemy) - using a new method in Ai class
                            3. cancel the other tasks
                            4. if health is low? don't engage (hide) : engage (shoot) face to face
                            
                            */
                           
                                if (game.enemyPresents){
                                int myDirection = game.myTank.direction;
                              
                                if (nextMove.x == currentX + 1)
                                {
                                    if (myDirection == 1)
                                    {
                                        shoot();
                                    }
                                  
                                    else
                                    {
                                        sendData("RIGHT#");
                                    }
                                    r_stream.Close();
                                    listener.Stop();
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
                                    else
                                    {
                                        sendData("LEFT#");
                                    }

                                    r_stream.Close();
                                    listener.Stop();
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

                                    else
                                    {
                                        sendData("DOWN#");
                                    }

                                    r_stream.Close();
                                    listener.Stop();
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

                                    else
                                    {
                                        sendData("UP#");
                                    }

                                    r_stream.Close();
                                    listener.Stop();
                                    reciever.Close();
                                    game.enemyPresents = false;
                                    continue;

                                }

                                game.enemyPresents = false;
                            }


                            // TO DO:- initialy tank direction is up, it wants to go right... timeCostToTarget is lack of the time to turn right... has to fix this.             

                            
                                if (packPresents)
                                {
                                  
                                    if (nextMove.x == currentX + 1)
                                    {
                                        sendData("RIGHT#");
                                    }
                                    else if (nextMove.x == currentX - 1)
                                    {
                                        sendData("LEFT#");
                                    }
                                    else if (nextMove.y == currentY + 1)
                                    {
                                        sendData("DOWN#");
                                    }
                                    else if (nextMove.y == currentY - 1)
                                    {
                                        sendData("UP#");
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
                                
                                shoot();
                                continue;
                            }

                    }
                    }


                    r_stream.Close();
                    listener.Stop();
                    reciever.Close();
                }


            
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed!  " + e.Message + "\n" + e.Source + "\n" + e.Data);
                errorOcurred = true;
            }
            finally
            {


                /*if (reciever != null)
                    if (reciever.Connected)
                        reciever.Close();*/
                // if (errorOcurred)
                Console.WriteLine("restarting receiving ;)");
                 receiveData();
            }
        }

        public void shoot()
        {
            sendData("SHOOT#");
            
           
        }

        /// <summary>
        /// method to send data to an already connected server
        /// </summary>
        /// <param name="data"></param>
        public void sendData(String data)
        {
            try
            {
                // Create a new TCP client socket to send data to the server
                _clientSocket = new TcpClient();

                //192.168.1.100            
                _clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 6000);

                if (_clientSocket.Connected)
                {
                    //To write to the socket
                    stream = _clientSocket.GetStream();

                    //Create objects for writing across stream
                    writer = new BinaryWriter(stream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(data);
                    // Console.WriteLine("Sentdata "+data);
                    //writing to the port                
                    writer.Write(tempStr);

                    writer.Close();
                    stream.Close();

                }
            }
            catch (Exception e)
            {
                attempt++;
                // Console.Clear();
                Console.WriteLine("Sending data to server failed due to " + e.Message);
                Console.WriteLine("Attempt " + attempt + " to send data to server.....");
                sendData(data);
            }


        }
    }
}
