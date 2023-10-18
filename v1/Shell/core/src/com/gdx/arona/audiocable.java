package com.gdx.arona;

import java.io.*;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketAddress;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.TimeUnit;

public class audiocable implements Runnable{
    int runswit = 1;
    public void run() {
        try{
            ServerSocket server = new ServerSocket(17469);
            while(runswit==1)
            {
                Socket csocket = server.accept();
                BufferedReader reader = new BufferedReader(new InputStreamReader(csocket.getInputStream()));
                while (!reader.ready()){
                }
                arona.lipvar = reader.readLine();
                csocket.close();
            }
            server.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
