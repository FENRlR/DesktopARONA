using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Spine3_8_95;
//using ControlzEx.Standard;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Media.Media3D;
//using System.Windows;
//using System.Windows.Input;
//using System.Runtime.CompilerServices;
//using System.Windows.Forms;
//using System.Data.Common;
//using System.IO;
//using System.Reflection.Emit;
//using System.Runtime.ConstrainedExecution;
//using System.Diagnostics;
using ARONA_dst.PublicFunction.Player;
using ARONA_dst;
using System.Threading;
using System.Net.Mail;
using System.Xml.Linq;
//using System.Runtime.Remoting.Contexts;
//using static Spine3_8_95.SkeletonBinary;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Windows.Media;
//using static System.Net.Mime.MediaTypeNames;
//using SixLabors.ImageSharp;

public class Player_3_8_95 : IPlayer
{
    public static int testcase = 0;

    private static int fpsfix;

    private static Skeleton skeleton;
    private static AnimationState state;
    private static SkeletonRenderer skeletonRenderer;

    private static Atlas atlas;
    private static SkeletonData skeletonData;
    private static AnimationStateData stateData;
    private static SkeletonBinary binary;
    private static SkeletonJson json;

    private Animation rani;
    private Animation pat;

    static float facex;
    static float facey;

    static double patx1;
    static double patx2;
    static double paty1;
    static double paty2;

    static double uptx1;
    static double uptx2;
    static double upty1;
    static double upty2;

    static double dwtx;
    static double dwty;
    static double dwtr;

    static double eyerad = 113 * App.scalemod;

    static float relx;
    static float rely;
    static float degree;
    static float unit = 360;
    static float offset = (float)(3.33 * 0.24);
    static float actualdur = (float)(3.33 * 0.61);
    static float meta;

    static int motsw = 0;

    static submod sb = new submod();

    public static int eyeclose = 0;
    public static int eyecount = 0;
    static Random rand = new Random();
    public static int eyelimit;

    public static int lipcount = 0;

    public static int brcount = 0;
    public static int brduration = 0;
    public static int orgsm = 0;

    private static textboxin textboxin = new textboxin();

    static int tblimit = App.globalValues.Speed * 24;

    

    public void getmodelsize()
    {
        string parent = System.IO.Directory.GetParent("../..").FullName;
        string atlaspath = parent + @"\aronares\arona\arona_spr.atlas";
        string skelpath = parent + @"\aronares\arona\arona_spr.skel";

        skeletonData = json.ReadSkeletonData(skelpath);

        App.globalValues.SpineVersion = skeletonData.Version;
        App.mdw = skeletonData.width;
        App.mdh = skeletonData.height;

        skeleton = new Skeleton(skeletonData);
    }

    public void Initialize()
    {
        Player.Initialize(ref App.graphicsDevice, ref App.spriteBatch);
    }

    public void LoadContent(ContentManager contentManager)
    {
        skeletonRenderer = new SkeletonRenderer(App.graphicsDevice);
        skeletonRenderer.PremultipliedAlpha = App.globalValues.Alpha;

        if(App.mulitTexture != null && App.mulitTexture.Length == 0)
        {
            atlas = new Atlas(App.globalValues.SelectAtlasFile, new XnaTextureLoader(App.graphicsDevice));
        }
        else
        {
            atlas = new Atlas(App.globalValues.SelectAtlasFile,new XnaTextureLoader(App.graphicsDevice,true,App.mulitTexture));
        }
        

        if (Common.IsBinaryData(App.globalValues.SelectSpineFile))
        {
            binary = new SkeletonBinary(atlas);
            binary.Scale = App.globalValues.Scale;
            skeletonData = binary.ReadSkeletonData(App.globalValues.SelectSpineFile);
        }
        else
        {
            json = new SkeletonJson(atlas);
            json.Scale = App.globalValues.Scale;
            skeletonData = json.ReadSkeletonData(App.globalValues.SelectSpineFile);
        }
        App.globalValues.SpineVersion = skeletonData.Version;


        skeleton = new Skeleton(skeletonData);


        Common.SetInitLocation(skeleton.Data.Height);
        App.globalValues.FileHash = skeleton.Data.Hash;

        stateData = new AnimationStateData(skeleton.Data);
        stateData.SetMix(sb.motion(39), sb.motion(39), 0.2f);
        for (int i = 0; i < 38; i++)
        {
            stateData.SetMix(sb.motion(i), sb.motion(i), 0.04f);
            
        }
        stateData.SetMix(sb.motion(37), sb.motion(39), 0.02f);


        String[] liplist = { "01", "20", "31", "02" };
        for (int i = 0; i < liplist.Length; i++)
        {
            for (int j = 0; j < liplist.Length; j++)
            {
                if (!liplist[i].Equals(liplist[j]))
                {
                    stateData.SetMix(liplist[i], liplist[j], 0.02f);
                    stateData.SetMix(liplist[j], liplist[i], 0.02f);
                }
            }
        }

        state = new AnimationState(stateData);


        if (App.isNew)
        {
            App.globalValues.PosX = (float)(App.canvasWidth - skeletonData.width * App.globalValues.Scale/1.4);
            App.globalValues.PosY = (float)(App.canvasHeight - skeletonData.height * App.globalValues.Scale/2);
            

            facex = (float)(App.scw - 200 * App.scalemod);
            facey = (float)(App.sch - 635 * App.scalemod);

            patx1 = (App.scw - 240 * App.scalemod);
            patx2 = (App.scw - 140 * App.scalemod);
            paty1 = (App.sch - 760 * App.scalemod);
            paty2 = (App.sch - 680 * App.scalemod);

            uptx1 = App.scw - 230 * App.scalemod;
            uptx2 = App.scw - 175 * App.scalemod;
            upty1 = App.sch - 577 * App.scalemod;
            upty2 = App.sch - 543 * App.scalemod;

            dwtx = App.scw - 205 * App.scalemod;
            dwty = App.sch - 435 * App.scalemod;
            dwtr = 15 * App.scalemod;

        }
        App.isNew = false;

        eyelimit = rand.Next(0, 61) + 240;
        rani = skeletonData.FindAnimation(sb.motion(35));
        pat = skeletonData.FindAnimation(sb.motion(37));        

        skeleton.X = App.globalValues.PosX;
        skeleton.Y = App.globalValues.PosY;

        state.TimeScale = App.globalValues.TimeScale;

        fpsfix = (int)((float)(1)/ (float)(App.globalValues.Speed)*1000);

        state.SetEmptyAnimation(2, 0.03f);
    }



    public void Update(GameTime gameTime)
    {
        if (App.globalValues.SelectAnimeName != "" && App.globalValues.SetAnime)
        {
            state.ClearTracks();
            skeleton.SetToSetupPose();
            state.SetAnimation(0, App.globalValues.SelectAnimeName, App.globalValues.IsLoop);
            App.globalValues.SetAnime = false;
        }

        if (App.globalValues.SelectSkin != "" && App.globalValues.SetSkin)
        {
            skeleton.SetSkin(App.globalValues.SelectSkin);
            skeleton.SetSlotsToSetupPose();
            App.globalValues.SetSkin = false;
        }
    }

    public void Draw()
    {
        if (App.isGLJPress && motsw != 3)
        {
            App.isGLJPress = false;
            if (motsw != 1 && orgsm == 0 && App.GmouseLocation[0] > uptx1 && App.GmouseLocation[0] < uptx2 && App.GmouseLocation[1] > upty1 && App.GmouseLocation[1] < upty2)
            {
                brcount++;
                state.Update((float)(1.0));
                if (brcount == 1)
                {
                    state.SetAnimation(4, sb.motion(16), false);
                    brduration = rand.Next(0, 61) + 120;
                }
                else if (brcount == 2)
                {
                    state.SetAnimation(4, sb.motion(17), false);
                    brduration = rand.Next(0, 61) + 120;
                }
                else
                {
                    state.SetAnimation(4, sb.motion(19), false).setTrackTime(1.0f);
                    brduration = rand.Next(0, 61) + 180;
                }
            }
        }
        else if (brcount > 0)
        {
            brduration--;
            if (brduration == 0)
            {
                brcount = 0;
                state.SetEmptyAnimation(4, 0);
            }
        }
        else if (App.isGLPress)
        {
            relx = (App.GmouseLocation[0] - facex);
            rely = (App.GmouseLocation[1] - facey);
            
            if (App.GmouseLocation[0] > patx1 && App.GmouseLocation[0] < patx2 && App.GmouseLocation[1] > paty1 && App.GmouseLocation[1] < paty2 && motsw < 2 || motsw == 1)
            {
                motsw = 1;
                state.SetAnimation(3, sb.motion(37), false).SetAnimationStart(lrgf(relx, (float)(0.5), (float)(0.0)));
            }
            else if (Math.Sqrt(Math.Pow(App.GmouseLocation[0] - dwtx, 2) + Math.Pow(App.GmouseLocation[1] - dwty, 2)) < dwtr && motsw != 1 && brcount == 0 || motsw == 3)
            {
                motsw = 3;
                state.SetAnimation(4, sb.motion(6), false);
                state.Update((float)(1.0));
                orgsm = 1;
            }
            else if (Math.Sqrt(Math.Pow(relx, 2) + Math.Pow(rely, 2)) < 113 && motsw != 1)
            {
                motsw = 2;
                rani.Apply(skeleton, -1, 0, false, null, 1, MixBlend.First, MixDirection.In);
            }
            else if (motsw != 1 && Math.Sqrt(Math.Pow(relx, 2) + Math.Pow(rely, 2)) > eyerad)
            {
                motsw = 2;
                degree = (float)(-Math.Atan2(rely, relx) * (180.0 / Math.PI)) + 180;
                if (degree > 360)
                {
                    degree -= 180;
                }
                meta = (float)((degree) * (actualdur / unit) )+ offset;
                rani.Apply(skeleton, -1, meta, false, null, 1, MixBlend.First, MixDirection.In);
            }
        }
        else if (motsw != 0)
        {
            if (motsw == 1)
            {
                state.AddAnimation(3, sb.motion(36), false, 0);
                state.SetEmptyAnimation(3, 0);
            }
            else if (motsw == 2)
            {
                state.AddAnimation(3, sb.motion(34), false, 0);
                state.AddEmptyAnimation(3, 0, 0);
            }
            else if (motsw == 3)
            {
                state.AddEmptyAnimation(4, 0, 0);
                orgsm = 0;
            }

            motsw = 0;
            eyecount = 0;
        }
        else
        {
            eyecount++;
            if (eyecount >= eyelimit)
            {
                if (eyeclose == 0)
                {
                    state.AddAnimation(1, sb.motion(38), false,0);
                    
                    eyecount = 0;
                    eyelimit = 10;
                    eyeclose = 1;
                }
                else
                {
                    state.SetEmptyAnimation(1, 0.5f);
                    eyecount = 0;
                    eyelimit = rand.Next(0,61) + 360;
                    eyeclose = 0;
                }
            }
        }


        if (!aninull(2).Equals(App.lipvar) && App.lipvar != "")
        {
            state.SetAnimation(2, App.lipvar, false);
            lipcount = 0;

            if(aninull(4).Equals(""))
            {
                if (App.emotion.Equals("기쁨"))
                {
                    state.AddAnimation(4, "11", true, 0);
                }
                else if (App.emotion.Equals("놀람"))
                {
                    state.AddAnimation(4, "04", true, 0);
                }
                else if (App.emotion.Equals("화남"))
                {
                    state.AddAnimation(4, "05", true, 0);
                }
                else if (App.emotion.Equals("안도"))
                {
                    state.AddAnimation(4, "10", true, 0);
                }
                else if (App.emotion.Equals("선호"))
                {
                    state.AddAnimation(4, "21", true, 0);
                }
                else if (App.emotion.Equals("신남"))
                {
                    state.AddAnimation(4, "12", true, 0);
                }
                else if (App.emotion.Equals("흐믓"))
                {
                    state.AddAnimation(4, "13", true, 0);
                }
                else if (App.emotion.Equals("짜증"))
                {
                    state.AddAnimation(4, "14", true, 0);
                }
                else if (App.emotion.Equals("걱정"))
                {
                    state.AddAnimation(4, "15", true, 0);
                }
                else if (App.emotion.Equals("흥분"))
                {
                    state.AddAnimation(4, "25", true, 0);
                }
                else if (App.emotion.Equals("따분"))
                {
                    state.AddAnimation(4, "26", true, 0);
                }
                else if (App.emotion.Equals("당황"))
                {
                    state.AddAnimation(4, "28", true, 0);
                }
                else if (App.emotion.Equals("혼란"))
                {
                    state.AddAnimation(4, "30", true, 0);
                }

                state.GetCurrent(4).Animation.timelines.Items.SetValue(state.GetCurrent(2).Animation.timelines.Items.GetValue(10), 11);
            }
        }
        else if (lipcount >= 12)
        {
            state.SetAnimation(2, "01", false);
            state.SetEmptyAnimation(2, 0.03f);
            App.lipvar = "";
            lipcount = 0;
        }
        else if(aninull(2).Equals(App.lipvar) && !(App.lipvar==""))
        {
            lipcount += 1;
        }
        else if(aninull(2).Equals("") && !aninull(4).Equals("") && !App.emotion.Equals(""))
        {
            App.emotion = "";
            state.ClearTrack(4);
            skeleton.SetToSetupPose();
        }


        if (App.isKBlctrlPress && App.isKBenterPress && App.txton == 0)
        {
            if (App.ctxt.Equals("") && App.outboxup == 1)
            {
                App.txtboxtime = 0;
                MainWindow.outboxhide();
                MainWindow.outboxclear();
            }
            textboxin.Show();

            App.txton = 1;
        }
        else if (App.txton == 1)
        {
            if(App.internalcounter<App.globalValues.Speed)
            {
                App.internalcounter++;
            }

            if (App.msgontherocks == 1)
            {
                if (!App.rtxt.Equals(""))
                {
                    MainWindow.outboxupdate(App.rtxt);
                }

                App.txton = 0;
                App.rtxt = "";

                App.msgontherocks = 0;
            }
            else if (App.ifboxnull == 1)
            {
                App.ifboxnull = 0;
                App.txton = 0;
            }

        }


        if (App.outboxup==1)
        {
            App.txtboxtime++;
            if (App.txtboxtime > tblimit)
            {
                App.txtboxtime = 0;
                MainWindow.outboxhide();
                MainWindow.outboxclear();
                App.txton = 0;
            }
        }

        App.graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
        state.Update(App.globalValues.Speed / 1000f);
        state.Apply(skeleton);

        

        skeleton.UpdateWorldTransform();
        ((BasicEffect)skeletonRenderer.Effect).Projection = Microsoft.Xna.Framework.Matrix.CreateOrthographicOffCenter(0, App.graphicsDevice.Viewport.Width, App.graphicsDevice.Viewport.Height, 0, 1, 0);
        


        skeletonRenderer.Begin();
        skeletonRenderer.Draw(skeleton);
        skeletonRenderer.End();

        if (App.HookEN == false)
        {
            Thread.Sleep(fpsfix);
        }
    }

    public void ChangeSet()
    {
        App.appXC.ContentManager.Dispose();
        atlas.Dispose();
        atlas = null;
        App.appXC.LoadContent.Invoke(App.appXC.ContentManager);
    }

    public void SizeChange()
    {
        if (App.graphicsDevice != null)
            Player.UserControl_SizeChanged(ref App.graphicsDevice);
    }

    public void Dispose()
    {
        ChangeSet();
    }

    private float lrgf(float x, float slope, float bias)
    {
        return (float)(0.667 * (0.23 + 0.52 / (1 + Math.Exp((double)(-slope * x - bias)))));
    }


    private String aninull(int ti)
    {
        try
        {
            return state.GetCurrent(ti).Animation.Name;
        }
        catch (Exception e)
        {
            return "";
        }
    }


}

