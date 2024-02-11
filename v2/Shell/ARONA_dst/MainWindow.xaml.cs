using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ARONA_dst.Views;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using ARONA_dst.Windows;
using System.Runtime.InteropServices;
using System.Threading;
using ARONA_dst.PublicFunction.Player;
using System.Windows.Forms;
using ARONA_dst;
using System.Runtime.CompilerServices;


public class Hooker
{
    private const int WH_MOUSE_LL = 14;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONUP = 0x0202;
    private const int WM_MOUSEMOVE = 0x0200;

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_KEYUP = 0x0101;
    private const int WM_SYSKEYUP = 0x0105;
    private static HookProc keyboardHookProc;
    private static IntPtr keyboardHookId = IntPtr.Zero;

    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static HookProc hookProc;
    private static IntPtr hookId = IntPtr.Zero;


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);


    private static IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {

        if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
        {
            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            App.isGLJPress = true;
            App.isGLPress = true;
            App.GmouseLocation[0] = hookStruct.pt.x;
            App.GmouseLocation[1] = hookStruct.pt.y;
        }
        else if (App.isGLPress && wParam == (IntPtr)WM_MOUSEMOVE)
        {
            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            App.GmouseLocation[0] = hookStruct.pt.x;
            App.GmouseLocation[1] = hookStruct.pt.y;
        }
        else if(nCode >= 0 && wParam == (IntPtr)WM_LBUTTONUP)
        {
            App.isGLPress = false;
        }

        return CallNextHookEx(hookId, nCode, wParam, lParam);
    }


    private static IntPtr KeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            uint virtualKeyCode = hookStruct.vkCode;

            if (hookStruct.vkCode == 162)
            {
                App.isKBlctrlPress = true;
            }
            else if (hookStruct.vkCode == 13)
            {
                App.isKBenterPress = true;
            }
        }
        else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
        {
            KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            uint virtualKeyCode = hookStruct.vkCode;
            if (hookStruct.vkCode==162)
            {
                App.isKBlctrlPress = false;
            }
            else if(hookStruct.vkCode == 13)
            {
                App.isKBenterPress = false;
            }
        }

        return CallNextHookEx(keyboardHookId, nCode, wParam, lParam);
    }

    public static void Sethook()
    {
        hookProc = MouseHookProc;
        hookId = SetWindowsHookEx(WH_MOUSE_LL, hookProc, GetModuleHandle(null), 0);

        keyboardHookProc = KeyboardHook;
        keyboardHookId = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardHookProc, GetModuleHandle(null), 0);

        if (hookId == IntPtr.Zero)
        {
            Console.WriteLine("FAIL");
        }
        App.HookEN = true;
    }

    public static void Unhook()
    {
        if (hookId != IntPtr.Zero)
        {
            UnhookWindowsHookEx(hookId);
            hookId = IntPtr.Zero;
        }
        if (keyboardHookId != IntPtr.Zero)
        {
            UnhookWindowsHookEx(keyboardHookId);
            keyboardHookId = IntPtr.Zero;
        }

        App.HookEN = false;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public KBDLLHOOKSTRUCTFlags flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
    [Flags]
    private enum KBDLLHOOKSTRUCTFlags : uint
    {
        LLKHF_EXTENDED = 0x01,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80,
    }
}


namespace ARONA_dst
{
    public partial class MainWindow : Window
    {
        public float scale = 0.25f;
        public float scalemod;

        int speedx = 30;
        float tscalex = 0.5f;
        
        telemetry telemetryprep = new telemetry();
        audiocable audiocableprep = new audiocable();
        Thread telemetry;
        Thread audiocable;

        info infobox = new info();

        [DllImport("gdi32.dll", EntryPoint = "AddFontResource", SetLastError = true)]
        public static extern int AddFontResource(string lpszFilename);

        public static MainWindow MasterMain;
        public static ContentControl MasterControl;
        public static UCPlayer UC_Player;
        public static Open open;
        public static string dvf = "3.8.95";

        private readonly Properties.Settings settings = Properties.Settings.Default;

        public static MainWindow myMainWindow;

        private System.Windows.Forms.ContextMenu TrayMenu = new System.Windows.Forms.ContextMenu();
        private NotifyIcon notifyIcon;

        public MainWindow()
        {

            telemetry = new Thread(new ThreadStart(telemetryprep.sockrun));
            telemetry.Start();

            audiocable = new Thread(new ThreadStart(audiocableprep.audiocablerun));
            audiocable.Start();

            myMainWindow = this;

            InitializeComponent();


            string parent = System.IO.Directory.GetParent("../..").FullName;
            string fontpath = parent + @"\aronares\arona\font\MainFont.ttf";
            LoadFont(fontpath);

            Game game = new Game();
            game.IsFixedTimeStep = true;
            game.TargetElapsedTime = TimeSpan.FromTicks(1);

            this.Title = $"ARONA      v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            MasterMain = this;

            this.Background = Brushes.Transparent;

            scalemod = scale / 0.35f;
            App.scalemod = scalemod;

            App.taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.VirtualScreenWidth - 430*scalemod;
            this.Top = SystemParameters.VirtualScreenHeight - 770*scalemod - App.taskbarHeight;

            App.scw = SystemParameters.VirtualScreenWidth;
            App.sch = SystemParameters.VirtualScreenHeight;


            this.Width = (SystemParameters.VirtualScreenWidth/4)* scale/0.35f;
            this.Height = (SystemParameters.VirtualScreenHeight/1.25)* scale/0.35f;


            if (settings.Topmost == true)
            {
                this.Topmost = true;
            }
            else
            {
                this.Topmost = false;
            }

            this.ShowInTaskbar = false;


            double rsize = 0.7;
            this.Gridoutbox.Width = 602 * rsize *scalemod;
            this.Gridoutbox.Height = 163 * rsize *scalemod;

            this.Textoutputbox.Width = this.Gridoutbox.Width;
            this.Textoutputbox.Height = this.Gridoutbox.Height;

            this.Gridoutbox.Margin = new Thickness(0, 210 * scalemod, 45*scalemod, 0);
            this.Textoutputbox.FontSize = 29 * scalemod;
            this.Textoutputbox.Padding = new Thickness(0, 45*scalemod, 0, 0);
            this.Textoutputbox.FontWeight = FontWeights.Medium;

            outboxhide();

            TrayIcon();

            LoadSetting();
        }

        private void TrayIcon()
        {
            string parent = System.IO.Directory.GetParent("../..").FullName;
            string iconpath = parent + @"\ARONA_dst\icon.ico";
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon(iconpath);
            notifyIcon.Visible = true;
            notifyIcon.Text = "ARONA";

            TrayMenu.MenuItems.Add("숨기기", (s, e) => HideMainWindow());
            TrayMenu.MenuItems.Add("종료", (s, e) => ExitApplication());
            notifyIcon.ContextMenu = TrayMenu;

            notifyIcon.DoubleClick += (s, e) => ShowMainWindow();
        }
        private void ShowMainWindow()
        {
            if (App.HookEN == false && App.isIDLE == false)
            {
                Hooker.Sethook();
            }

            telemetry.Resume();
            audiocable.Resume();

            TrayMenu.MenuItems.Clear();
            TrayMenu.MenuItems.Add("숨기기", (s, e) => HideMainWindow());
            TrayMenu.MenuItems.Add("종료", (s, e) => ExitApplication());
            App.globalValues.Speed = speedx;
            App.globalValues.TimeScale = tscalex;
            Show();
            Activate();
        }
        
        private void HideMainWindow()
        {
            Hooker.Unhook();

            telemetry.Suspend();
            audiocable.Suspend();
            
            TrayMenu.MenuItems.Clear();
            TrayMenu.MenuItems.Add("원래대로", (s, e) => ShowMainWindow());
            TrayMenu.MenuItems.Add("종료", (s, e) => ExitApplication());
            Hide();
            App.globalValues.Speed = 0;
            App.globalValues.TimeScale = 0;
        }
        private void ExitApplication()
        {
            Hooker.Unhook();
            Environment.Exit(0);
        }


        public static void outboxupdate(string text)
        {
            myMainWindow.Textoutputbox.Text = text;
            myMainWindow.Gridoutbox.Visibility = Visibility.Visible;
            App.outboxup = 1;
        }

        public static void outboxclear()
        {
            myMainWindow.Textoutputbox.Text = "";
        }
        public static void outboxhide()
        {
            myMainWindow.Gridoutbox.Visibility = Visibility.Collapsed;
            App.outboxup = 0;
        }
        private void LoadFont(string fontpath)
        {
            try
            {
                AddFontResource(fontpath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR : {ex.Message}");
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.HookEN == false)
            {
                Hooker.Sethook();
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Hooker.Unhook();
        }

        


        public void LoadSetting()
        {
            if (App.globalValues.Scale == 0)
            {
                App.globalValues.Scale = 1;
            }

            if (Properties.Settings.Default.LastSelectDir == "")
            {
                App.lastDir = App.rootDir;
            }
            else
            {
                App.lastDir = Properties.Settings.Default.LastSelectDir;
            }
            

            App.mainWidth = this.ActualWidth;
            App.mainHeight = this.ActualHeight;
            
            
            LoadPlayer(dvf);

        }


        public void UpdateSpine()
        {
            if (UC_Player != null)
            {
                UC_Player.ChangeSet();
            }
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            App.mainWidth = this.ActualWidth;
            App.mainHeight = this.ActualHeight;

            Player.Width = this.ActualWidth;
            Player.Height = this.ActualHeight;

        }

        private void ctm_Opened(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as System.Windows.Controls.ContextMenu;
            if (this.Topmost == true)
            {
                (contextMenu.Items[0] as System.Windows.Controls.MenuItem).Focus();
            }
        }

        public void LoadPlayer(string spineVersion)
        {
            Common.Reset();

            App.globalValues.Alpha = true;
            App.globalValues.IsLoop = true;
            App.globalValues.PreMultiplyAlpha = true;
            App.globalValues.SelectSpineVersion = "3.8.95";

            string parent = System.IO.Directory.GetParent("../..").FullName;
            string atlaspath = parent + @"\ARONA_dst\aronares\arona\arona_spr.atlas";
            string skelpath = parent + @"\ARONA_dst\aronares\arona\arona_spr.skel";

            App.globalValues.SelectAtlasFile = atlaspath;
            App.globalValues.SelectSpineFile = skelpath;
            App.globalValues.SelectAnimeName = "Idle_01";
            App.globalValues.SetAnime = true;

            double setWidth = this.Width;
            double setHeight = this.Height;
            App.globalValues.FrameWidth = setWidth;
            App.globalValues.FrameHeight = setHeight;
            App.canvasWidth = setWidth;
            App.canvasHeight = setHeight;
            App.isNew = true;

            App.globalValues.Scale = scale;

            App.globalValues.Speed = speedx;
            App.globalValues.TimeScale = tscalex;


            if (Player.Content != null)
            {
                if (App.globalValues.SelectSpineVersion != spineVersion)
                {
                    App.globalValues.SelectSpineVersion = "3.8.95";
                    App.isNew = true;
                    App.appXC.ContentManager.Dispose();
                    App.appXC.Initialize = null;
                    App.appXC.Update = null;
                    App.appXC.LoadContent = null;
                    App.appXC.Draw = null;

                    DependencyObject xnaParent = ((System.Windows.Controls.UserControl)Player.Content).Parent;
                    if (xnaParent != null)
                    {
                        xnaParent.SetValue(ContentPresenter.ContentProperty, null);
                    }
                    Canvas oldCanvas = (Canvas)App.appXC.Parent;
                    if (oldCanvas != null)
                    {
                        oldCanvas.Children.Clear();
                    }
                    Player.Content = null;
                    UC_Player = new UCPlayer();
                    Player.Content = UC_Player;
                }
                else
                {
                    UC_Player.Reload();
                }
            }
            else
            {
                App.globalValues.SelectSpineVersion = spineVersion;
                UC_Player = new UCPlayer();
                Player.Content = UC_Player;
            }

        }


        private void Frame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.isPress = true;
            App.mouseLocation = Mouse.GetPosition(this.MasterGrid);
        }

        private void Frame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //- LEGACY
            //App.isLPress = true;
            //Console.WriteLine(App.isLPress);
            //App.mouseLocation = Mouse.GetPosition(this.MasterGrid);
        }
        private void Frame_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.isRPress = true;
            App.mouseLocation = Mouse.GetPosition(this.MasterGrid);
            System.Windows.Controls.ContextMenu ctm = this.FindResource("ctm") as System.Windows.Controls.ContextMenu;
            ctm.PlacementTarget = sender as UIElement;
            ctm.IsOpen = true;
        }

        private void Frame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.isLPress = false;
        }
        private void Frame_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.isRPress = false;
        }

        private void Frame_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (App.isPress)
            {
                System.Windows.Point position = Mouse.GetPosition(this.MasterGrid);
                if (App.globalValues.UseBG && App.globalValues.ControlBG)
                {
                    Common.SetBGXY(position.X, position.Y, App.mouseLocation.X, App.mouseLocation.Y);
                }
                else if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    Common.SetXY(position.X, position.Y, App.mouseLocation.X, App.mouseLocation.Y);
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {

                    var transformGroup = (TransformGroup)MasterGrid.RenderTransform;
                    var tt = (TranslateTransform)transformGroup.Children.Where(x => x.GetType() == typeof(TranslateTransform)).FirstOrDefault();
                    tt.X = (float)(position.X + tt.X - App.mouseLocation.X);
                    tt.Y = (float)(position.Y + tt.Y - App.mouseLocation.Y);
                }
                App.mouseLocation = Mouse.GetPosition(this.MasterGrid);
            }
        }


        private void ctmenu1ck(object sender, RoutedEventArgs e)
        {
            settings.Topmost = true;
            this.Topmost = true;
        }
        private void ctmenu1unck(object sender, RoutedEventArgs e)
        {
            settings.Topmost= false;
            this.Topmost = false;
        }

        private void ctmenu11ck(object sender, RoutedEventArgs e)
        {
            Hooker.Unhook();
            App.isIDLE = true;
        }
        private void ctmenu11unck(object sender, RoutedEventArgs e)
        {
            if(App.HookEN == false)
            {
                Hooker.Sethook();
            }
            App.isIDLE = false;
        }


        private void ctmenu2(object sender, RoutedEventArgs e)
        {
            infobox.Show();
        }
        private void ctmenu3(object sender, RoutedEventArgs e)
        {
            Hooker.Unhook();
            Environment.Exit(0);
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastSelectDir = App.lastDir;
            Properties.Settings.Default.Save();
            if (open != null)
                open.Close();
        }

        private void mi_Exit_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LastSelectDir = App.lastDir;
            Properties.Settings.Default.Save();
            if (open != null)
                open.Close();
            System.Windows.Application.Current.Shutdown();
        }



        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            if (App.graphicsDevice != null && App.graphicsDevice.GraphicsDeviceStatus == Microsoft.Xna.Framework.Graphics.GraphicsDeviceStatus.NotReset)
            {
                App.graphicsDevice.Reset();
            }
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (App.graphicsDevice != null && App.graphicsDevice.GraphicsDeviceStatus == Microsoft.Xna.Framework.Graphics.GraphicsDeviceStatus.NotReset)
            {
                App.graphicsDevice.Reset();
            }
        }

    }
}
