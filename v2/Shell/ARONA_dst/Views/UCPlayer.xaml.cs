using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace ARONA_dst.Views
{
    public partial class UCPlayer : UserControl
    {
        public IPlayer player;
        public UCPlayer()
        {
            InitializeComponent();

            if(App.appXC == null)
            {
                App.appXC = new WpfXnaControl.XnaControl();
            }

            if(player != null)
            {
                player.Dispose();
            }

            switch (App.globalValues.SelectSpineVersion)
            {
                case "3.8.95":
                    player = new Player_3_8_95();
                    break;
            }

            App.appXC.Initialize += player.Initialize;
            App.appXC.Update += player.Update;
            App.appXC.LoadContent += player.LoadContent;
            
            App.appXC.Draw += player.Draw;
            App.appXC.Width = App.globalValues.FrameWidth;
            App.appXC.Height = App.globalValues.FrameHeight;
            App.appXC.Background = Brushes.Transparent;

            var transformGroup = (TransformGroup)Frame.RenderTransform;
            var tt = (TranslateTransform)transformGroup.Children.Where(x => x.GetType() == typeof(TranslateTransform)).FirstOrDefault();

            var transformGroupL = (TransformGroup)Frame.LayoutTransform;
            var st = (ScaleTransform)transformGroupL.Children.Where(x => x.GetType() == typeof(ScaleTransform)).FirstOrDefault();
            st.ScaleX = App.gscale;
            st.ScaleY = st.ScaleX;

            tt.X = (float)0;
            tt.Y = (float)0;

            Frame.Children.Add(App.appXC);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            player.SizeChange();
        }

        public void ChangeSet()
        {
            player.ChangeSet();
        }

        public void Reload()
        {
            var transformGroupL = (TransformGroup)Frame.LayoutTransform;
            var st = (ScaleTransform)transformGroupL.Children.Where(x => x.GetType() == typeof(ScaleTransform)).FirstOrDefault();
            st.ScaleX = 1;
            st.ScaleY = 1;
            var transformGroupR = (TransformGroup)Frame.RenderTransform;
            var tt = (TranslateTransform)transformGroupR.Children.Where(x => x.GetType() == typeof(TranslateTransform)).FirstOrDefault();
            tt.X = (float)0;
            tt.Y = (float)0;
            Frame.Children.Remove(App.appXC);
            App.appXC.Initialize -= player.Initialize;
            App.appXC.Update -= player.Update;
            App.appXC.LoadContent -= player.LoadContent;
            App.appXC.Draw -= player.Draw;


            App.appXC.Initialize += player.Initialize;
            App.appXC.Update += player.Update;
            App.appXC.LoadContent += player.LoadContent;
            App.appXC.Draw += player.Draw;
            App.appXC.Width = App.globalValues.FrameWidth;
            App.appXC.Height = App.globalValues.FrameHeight;

            Frame.Children.Add(App.appXC);
            player.ChangeSet();
        }

    }
}
