package com.gdx.arona;

import java.io.*;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketAddress;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.TimeUnit;

public class telemetry implements Runnable{
    int runswit = 1;
    public void run(){
        try{
            while(runswit==1)
            {
                try {
                    Thread.sleep(1);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                
                if(!arona.ctxt.equals("")&&arona.msgontherocks==0)
                {
                    Socket soc = new Socket();
                    SocketAddress adr = new InetSocketAddress("127.0.0.1", 47966);
                    soc.connect(adr);
                    InputStream in = soc.getInputStream();
                    BufferedReader rin = new BufferedReader(new InputStreamReader(in, StandardCharsets.UTF_8));

                    OutputStream out = soc.getOutputStream();
                    OutputStreamWriter wout = new OutputStreamWriter(out, "UTF-8");
                    
                    wout.write(arona.ctxt);
                    wout.flush();
                    
                    while (!rin.ready()){
                    }
                    arona.rtxt = rin.readLine();
                    arona.msgontherocks = 1;
                    arona.ctxt = "";

                    soc.close( );
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void post(){
        try{
            Socket soc = new Socket();
            SocketAddress adr = new InetSocketAddress("127.0.0.1", 47966);
            soc.connect(adr);
            InputStream in = soc.getInputStream();
            BufferedReader rin = new BufferedReader(new InputStreamReader(in, StandardCharsets.UTF_8));
            OutputStream out = soc.getOutputStream();
            OutputStreamWriter wout = new OutputStreamWriter(out, "UTF-8");
            
            wout.write(arona.ctxt);
            wout.flush( );

            while (!rin.ready()){
            }
            arona.rtxt = rin.readLine();
            arona.msgontherocks = 1;
            soc.close( );
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    
    public void get(){
    }
}
