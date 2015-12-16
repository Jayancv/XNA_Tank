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

namespace tankUI.Inside
{
    class Client
    {
        private TcpClient client;       //variables for client
        private string ip = "127.0.0.1";
        private Game1 com;
        private StringEvaluator eval;
        private Thread thread;      //creating  the thread
        private Int32 portIn = 6000;   //port use to connect
        private Int32 portOut = 7000;  //port to recieve

        public string data;

        public Client()
        {
            thread = new Thread(new ThreadStart(recieve));      //create new thread object
            eval = new StringEvaluator();                    //create eval object
        }

        //to send message to the server
        public void send(string message)
        {
           // this.com = com;                                   //initiate variable
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

                while ((i = r_stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                }
                string[] lines = Regex.Split(data, ":");
                //eval.evaluate(data);
                
                r_stream.Close();
                listner.Stop();
                reciever.Close();
            }
        }
      

    }
}
