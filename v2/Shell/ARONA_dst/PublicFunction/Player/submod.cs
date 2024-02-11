using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARONA_dst;
using static System.Windows.Forms.AxHost;


public class submod
{
    public string motion(int x)
    {
        string[] mlist = new string[] {"00"
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

    
    public string mslot(int x)
    {
        string[] slist = new string[] {
            "L_B_Hair_Original",//0
            "L_B_Hair_Pattern_01",//1
            "L_B_Hair_Pattern_02",//2
            "R_B_Hair_Originall",//3
            "R_B_Hair_Pattern_01",//4
            "R_B_Hair_Pattern_02",//5
            "R_Acc_Original",//6
            "R_Acc_Pattern_01",//7
            "R_Acc_Pattern_02",//8
            "R_B_Acc_01",//9
            "Weapon_01",//10
            "R_F_Acc_01",//11
            "R_Arm_Original",//12
            "R_Arm_Original2",//13
            "R_Arm_Pattern_02",//14
            "R_Arm_Pattern_01",//15
            "L_Arm_Original",//16
            "L_Arm_Original2",//17
            "L_Arm_Pattern_01",//18
            "L_Arm_Pattern_02",//19
            "R_Thigh_01",//20
            "L_Thigh_01",//21
            "Skirt_01",//22
            "Torso_Original",//23
            "Torso_Original2",//24
            "Torso_Pattern_01",//25
            "Torso_Original3",//26
            "Torso_Pattern_02",//27
            "F_B_Ribborn_Shadow",//28
            "F_L_B_Ribborn_Original",//29
            "F_L_B_Ribborn_Pattern_01",//30
            "F_L_B_Ribborn_Pattern_02",//31
            "F_R_B_Ribborn_Original",//32
            "F_R_B_Ribborn_Pattern_01",//33
            "F_R_B_Ribborn_Pattern_02",//34
            "F_Ribborn_Original",//35
            "F_Ribborn_Pattern_01",//36
            "F_Ribborn_Pattern_02",//37
            "B_L_Hair_03",//38
            "B_R_Hair_01",//39
            "L_Ear_01",//40
            "B_L_Hair_02",//41
            "B_L_Hair_01",//42
            "L_Eye_White_01",//43
            "R_Eye_White_01",//44
            "L_Eye_01",//45
            "R_Eye_01",//46
            "L_Eye_PP",//47
            "R_Eye_PP",//48
            "L_Eye_P_01",//49
            "R_Eye_P_01",//50
            "L_Eye_P_01_0",//51
            "R_Eye_P_01_0",//52
            "L_Eye_P_02_0",//53
            "R_Eye_P_02_0",//54
            "Head_01",//55
            "L_Eye_Cover_01",//56
            "R_Eye_Cover_01",//57
            "Head_Hair_01",//58
            "Head_Hair_03",//59
            "Head_Hair_02",//60
            "L_B_Eyebrows_01",//61
            "R_B_Eyebrows_01",//62
            "L_Eyebrows_01",//63
            "R_eyebrows_01",//64
            "Mouse_01",//65
            "Nose_01",//66
            "Flush_01",//67
            "Face_Shadow_01",//68
            "Sweat_01",//69
            "Sweat_1",//70
            "Sweat_2",//71
            "L_Tears_00",//72
            "R_Tears_00",//73
            "Despressed_Line_01",//74
            "F_Hair_Shadow",//75
            "T_R_eyebrow_01",//76
            "T_L_eyebrow_01",//77
            "F_Hair_01",//78
            "F_Hair_02_shadow_01",//79
            "F_Hair_02_shadow_00",//80
            "F_Hair_02",//81
            "T_Hairband_01",//82
            "R_Hair_01",//83
            "L_Hair_01",//84
            "T_Ribborn_Original",//85
            "T_Ribborn_Pattern_01",//86
            "T_Ribborn_Pattern_02",//87
            "Hair_L_Halo_Light",//88
            "Hair_L_Halo_01",//89
            "halo_normal_00_ad",//90
            "halo_normal_00_ad2",//91
            "halo_normal_00_adnomal",//92
            "halo_normal_00",//93
            "halo_happy_P_06_ad",//94
            "halo_happy_P_06_ad2",//95
            "halo_happy_P_06",//96
            "halo_happy_P_6",//97
            "halo_happy_P_05_ad",//98
            "halo_happy_P_05",//99
            "halo_happy_P_04_ad",//100
            "halo_happy_P_04_ad2",//101
            "halo_happy_P_04",//102
            "halo_happy_P_4",//103
            "halo_happy_P_03_ad",//104
            "halo_happy_P_03",//105
            "halo_happy_P_02_ad",//106
            "halo_happy_P_02",//107
            "halo_happy_P_01_ad",//108
            "halo_happy_P_01",//109
            "halo_happy_P_00_ad",//110
            "halo_happy_P_00_ad3",//111
            "halo_happy_P_00_ad2",//112
            "halo_happy_P_00",//113
            "halo_happy_P_1",//114
            "halo_happy_P_0",//115
            "halo_suprise_P_00_ad",//116
            "halo_suprise_P_00",//117
            "halo_suprise_P_01_ad",//118
            "halo_suprise_P_01",//119
            "halo_suprise_P_02_ad",//120
            "halo_suprise_P_02",//121
            "halo_suprise_P_03_ad",//122
            "halo_suprise_P_03",//123
            "halo_love_p_00_ad",//124
            "halo_love_p_00_ad2",//125
            "halo_love_p_oo",//126
            "halo_love_p_oo2",//127
            "halo_love_p_01_ad",//128
            "halo_love_p_01_ad2",//129
            "halo_love_p_01_ad3",//130
            "halo_love_p_01",//131
            "halo_love_p_1",//132
            "halo_love_p_2",//133
            "halo_love_p_02_ad",//134
            "halo_love_p_02_ad2",//135
            "halo_love_p_02",//136
            "halo_love_p_3"//137
        };
        return slist[x];
    }

    public string emcl(string em)
    {
        if (em.Equals("기쁨")
            || em.Equals("놀람")
            || em.Equals("화남")
            || em.Equals("안도") 
            || em.Equals("선호")
            || em.Equals("신남")
            || em.Equals("흐믓")
            || em.Equals("짜증")
            || em.Equals("걱정")
            || em.Equals("흥분")
            || em.Equals("따분")
            || em.Equals("당황")
            || em.Equals("혼란"))
        {
            return em;
        }
        else
        {
            return "";
        }
    }
}

