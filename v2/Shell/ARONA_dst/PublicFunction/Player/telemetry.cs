using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using static System.Windows.Forms.AxHost;

namespace ARONA_dst.PublicFunction.Player
{
    public class telemetry
    {
        int runswit = 1;
        submod sb = new submod();
        public void sockrun()
        {
            try
            {
                while (runswit == 1)
                {
                    if (!App.ctxt.Equals("") && App.msgontherocks == 0)
                    {
                        TcpClient soc = new TcpClient();
                        soc.Connect("127.0.0.1", 47966);


                        NetworkStream networkStream = soc.GetStream();
                        StreamReader rin = new StreamReader(networkStream, Encoding.UTF8);
                        StreamWriter wout = new StreamWriter(networkStream, Encoding.UTF8);


                        wout.Write(App.ctxt);
                        wout.Flush();

                        String temptxt = null;
                        while (temptxt == null)
                        {
                            temptxt = rin.ReadLine();
                            Thread.Sleep(33);
                        }

                        String[] tempem = temptxt.Split((",").ToCharArray(), 2);
                        if (sb.emcl(tempem[0]).Equals(""))
                        {
                            App.emotion = "";
                            App.rtxt = temptxt;
                        }
                        else
                        {
                            App.emotion = tempem[0];
                            App.rtxt = tempem[1];
                        }

                        App.msgontherocks = 1;
                        App.ctxt = "";
                        Console.WriteLine("message from server : " + App.rtxt);

                        soc.Close();

                    }

                }
            }
            catch(Exception e){
            }
        }
    }

}
