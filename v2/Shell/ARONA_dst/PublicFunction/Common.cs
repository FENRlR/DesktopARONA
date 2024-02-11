using ARONA_dst;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
//using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
//using SpineViewerWPF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

public class Common
{
    public static void Reset()
    {
        App.globalValues.PosX = 0;
        App.globalValues.PosY = 0;
        App.globalValues.Scale = 1;
        App.globalValues.ViewScale = 1;
        App.globalValues.SelectAnimeName = "";
        App.globalValues.SelectSkin = "";
        App.globalValues.SetSkin = false;
        App.globalValues.SetAnime = false;
        App.globalValues.Rotation = 0;
        App.globalValues.UseBG = false;
        App.globalValues.SelectBG = "";
        App.globalValues.ControlBG = false;
        App.globalValues.TimeScale = 1;
        App.globalValues.Lock = 0f;
        App.globalValues.IsRecoding = false;
        App.globalValues.FilpX = false;
        App.globalValues.FilpY = false;
        App.globalValues.PosBGX = 0;
        App.globalValues.PosBGY = 0;
        if (App.textureBG != null)
            App.textureBG.Dispose();

        if (App.globalValues.AnimeList != null)
            App.globalValues.AnimeList.Clear();
        if (App.globalValues.SkinList != null)
            App.globalValues.SkinList.Clear();

    }


    public static string GetDirName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public static string GetFileNameNoEx(string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    public static bool IsBinaryData(string path)
    {
        if (File.Exists(path.Replace(".atlas", ".skel")) && path.IndexOf(".skel") > -1)
            return true;
        else
            return false;
    }

    public static bool CheckSpineFile(string path)
    {
        if (File.Exists(path.Replace(".atlas", ".skel")))
        {

            App.globalValues.SelectSpineFile = path.Replace(".atlas", ".skel");
            return true;
        }
        else if (File.Exists(path.Replace(".atlas", ".json")))
        {
            App.globalValues.SelectSpineFile = path.Replace(".atlas", ".json");
            return true;
        }
        else
        {
            App.globalValues.SelectSpineFile = "";
            return false;
        }
          
    }


    public static string GetSkelPath(string path)
    {
        return path.Replace(".atlas", ".skel");
    }

    public static string GetJsonPath(string path)
    {
        return path.Replace(".atlas", ".json");
    }


    public static Texture2D SetBG(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fileStream))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);
                    return Texture2D.FromStream(App.appXC.GraphicsDevice, ms);
                }
            }
        }

    }

    public static void SetXY(double MosX, double MosY, double oldX, double oldY)
    {
        App.globalValues.PosX = (float)(MosX + App.globalValues.PosX - oldX);
        App.globalValues.PosY = (float)(MosY + App.globalValues.PosY - oldY);
    }

    public static void SetBGXY(double MosX, double MosY, double oldX, double oldY)
    {
        App.globalValues.PosBGX = (float)(MosX + App.globalValues.PosBGX - oldX);
        App.globalValues.PosBGY = (float)(MosY + App.globalValues.PosBGY - oldY);
    }


    public static void ClearCacheFile()
    {
        string[] fileList = Directory.GetFiles($"{App.rootDir}\\Temp\\", "*.*", SearchOption.AllDirectories);
        if (fileList.Length > 0)
        {
            foreach(string path in fileList)
            {
                File.Delete(path);
            }
        }
    }

    public static void SetInitLocation(float height)
    {
        if (App.isNew)
        {
            App.globalValues.PosX = Convert.ToSingle(App.globalValues.FrameWidth / 2f);
            App.globalValues.PosY = Convert.ToSingle((height + App.globalValues.FrameHeight) / 2f);
        }
    }

}


