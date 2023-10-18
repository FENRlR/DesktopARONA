package com.gdx.arona;

import java.util.ArrayList;
import java.util.HashMap;

public class submod {
    public String motion(int x){
        String mlist[] = {"00"
                ,"01"
                ,"02"
                ,"03"
                ,"04"
                ,"05"
                ,"06"
                ,"07"
                ,"08"
                ,"09"
                ,"10"
                ,"11"
                ,"12"
                ,"13"
                ,"14"
                ,"15"
                ,"16"
                ,"17"
                ,"18"
                ,"19"
                ,"20"
                ,"21"
                ,"22"
                ,"23"
                ,"24"
                ,"25"
                ,"26"
                ,"27"
                ,"28"
                ,"29"
                ,"30"
                ,"31"
                ,"32"
                ,"99"
                ,"Dev_LookEnd_01_M"
                ,"Dev_Look_01_M"
                ,"Dev_PatEnd_01_M"
                ,"Dev_Pat_01_M"
                ,"Eye_Close_01"
                ,"Idle_01"
                ,"LookEnd_01_A"
                ,"LookEnd_01_M"
                ,"Look_01_A"
                ,"Look_01_M"
                ,"PatEnd_01_A"
                ,"PatEnd_01_M"
                ,"Pat_01_A"
                ,"Pat_01_M"
        };
        return mlist[x];
    }

    public String com(String x){
        HashMap<String, String> mdic = new HashMap<String, String>();
        mdic.put("ㅓ", "02");
        mdic.put("ㅏ", "20");
        mdic.put("ㅔ", "31");
        return x;
    }
}
