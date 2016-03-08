using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class Parser
    {
        public Tokenizer tokenizer;                                        // the tokenizer  object

        public Parser(Game2 game) {                                        //Constructor
            tokenizer = new Tokenizer(game);                               //Create tokenizer for the game 
        }

        public void parse(String msgFrmServer) {
            try
            {
                if (msgFrmServer.StartsWith("I") && msgFrmServer.EndsWith("#"))           //Identifing initializ map details using first character "I"
                {
                    Console.WriteLine("Received initializ map details.");
                    tokenizer.Initiation(msgFrmServer);                                   //Then pass the message to tokenizer for initiation
                }
                else if (msgFrmServer.StartsWith("S") && msgFrmServer.EndsWith("#"))      //Identifing join request asseptaed by server and server started communication using first character "S"
                {
                    Console.WriteLine("Server accepted the JOIN request.");
                    tokenizer.Acceptance(msgFrmServer);
                }

                else if (msgFrmServer.StartsWith("G") && msgFrmServer.EndsWith("#"))       //Identifing game world updates using first character "G"
                {
                    Console.WriteLine("Received periodic game world updates");
                    tokenizer.MovingAnDshooting(msgFrmServer);
                }

                else if (msgFrmServer.StartsWith("C") && msgFrmServer.EndsWith("#"))       //Identifing coin details using first character "C"
                {
                    // Console.WriteLine("Received new coin pile");
                    tokenizer.Coins(msgFrmServer);
                }

                else if (msgFrmServer.StartsWith("L") && msgFrmServer.EndsWith("#"))        //Identifing lifePacket details using first character "L"
                {
                    // Console.WriteLine("Received a new Life pack");
                    tokenizer.lifePacks(msgFrmServer);
                }
                else
                {
                    Console.WriteLine("Server rejected the request");                        // put all other messages to rejection
                    tokenizer.Rejection(msgFrmServer);
                }

            }
            catch (Exception e)
            {
              
            }
        }
    }
}
