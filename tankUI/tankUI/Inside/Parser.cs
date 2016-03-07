using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tankUI.Inside
{
    public class Parser
    {
        public Tokenizer tokenizer;

        public Parser(Game2 game)
        {
            tokenizer = new Tokenizer(game);
        }

        public void parse(String msgFrmServer)
        {
            try
            {
                if (msgFrmServer.StartsWith("I") && msgFrmServer.EndsWith("#"))
                {
                    Console.WriteLine("Received initializ map details.");
                    tokenizer.Initiation(msgFrmServer);
                }
                else if (msgFrmServer.StartsWith("S") && msgFrmServer.EndsWith("#"))
                {
                    Console.WriteLine("Server accepted the JOIN request.");
                    tokenizer.Acceptance(msgFrmServer);
                }

                else if (msgFrmServer.StartsWith("G") && msgFrmServer.EndsWith("#"))
                {
                    Console.WriteLine("Received periodic game world updates");
                    tokenizer.MovingAnDshooting(msgFrmServer);
                }

                else if (msgFrmServer.StartsWith("C") && msgFrmServer.EndsWith("#"))
                {
                    // Console.WriteLine("Received new coin pile");
                    tokenizer.Coins(msgFrmServer);
                }

                else if (msgFrmServer.StartsWith("L") && msgFrmServer.EndsWith("#"))
                {
                    // Console.WriteLine("Received a new Life pack");
                    tokenizer.lifePacks(msgFrmServer);
                }
                else
                {
                    Console.WriteLine("Server rejected the request");
                    tokenizer.Rejection(msgFrmServer);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(msgFrmServer);
            }
        }
    }
}
