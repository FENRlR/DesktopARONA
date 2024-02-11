using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using ARONA_dst;
using ControlzEx.Standard;

namespace ARONA_dst.PublicFunction.Player
{
    public class audiocable
    {
        int runswit = 1;
        public void audiocablerun()
        {
            try
            {
                TcpListener server = new TcpListener(IPAddress.Loopback, 17469);
                server.Start();


                while (runswit == 1)
                {
                    TcpClient csocket = server.AcceptTcpClient();

                    NetworkStream networkStream = csocket.GetStream();
                    StreamReader reader = new StreamReader(networkStream, Encoding.UTF8);

                    String temptxt = null;
                    while (temptxt == null)
                    {
                        temptxt = reader.ReadLine();
                        Thread.Sleep(33);
                    }

                    App.lipvar = temptxt;
                    csocket.Close();
                }
                server.Stop();
            }
            catch (IOException e)
            {
            }

            
        }
    }
}