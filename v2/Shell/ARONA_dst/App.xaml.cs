using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfXnaControl;

namespace ARONA_dst
{
    public partial class App : Application
    {
        public static GlobalValue globalValues = new GlobalValue();
        public static string rootDir = Environment.CurrentDirectory;
        public static string lastDir = "";
        public static XnaControl appXC;
        public static Texture2D textureBG;
        public static string[] mulitTexture;

        public static double scw;
        public static double sch;

        public static double gscale = 1.0;

        public static int txton = 0;
        public static int msgontherocks = 0;
        public static string rtxt = "";
        public static string ctxt = "";

        public static string emotion = "";
        public static string lipvar = "";

        public static int ifboxnull = 0;
        public static int txtboxtime = 0;

        public static int outboxup = 0;

        public static double scalemod;
        public static double taskbarHeight;

        public static int internalcounter = 0;

        public static float mdw;
        public static float mdh;

        public static bool isPress = false;
        public static bool isLPress = false;
        public static bool isRPress = false;
        public static bool isNew = true;
        public static System.Windows.Point mouseLocation;

        public static bool isGLPtemp = false;
        public static bool isGLJPress = false;
        public static bool isGLPress = false;
        public static int[] GmouseLocation = new int[2];

        public static bool isKBPress = false;
        public static bool isKBlctrlPress = false;
        public static bool isKBenterPress = false;

        public static SpriteBatch spriteBatch;
        public static GraphicsDevice graphicsDevice;
        public static int recordImageCount;
        public static double canvasWidth = SystemParameters.WorkArea.Width;
        public static double canvasHeight = SystemParameters.WorkArea.Height;
        public static double mainWidth ;
        public static double mainHeight ;

        public static  string tempDirPath = $"{App.rootDir}\\Temp\\";

        public static bool HookEN = false;
        public static bool isIDLE = false;
    }
}
