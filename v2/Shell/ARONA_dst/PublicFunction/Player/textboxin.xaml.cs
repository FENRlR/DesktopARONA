using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Data.Common;

namespace ARONA_dst.PublicFunction.Player
{
    public partial class textboxin : Window
    {
        public textboxin()
        {
            App.isKBenterPress = false;

            InitializeComponent();
            this.Background = System.Windows.Media.Brushes.Transparent;
            this.Topmost = true;
            this.ShowInTaskbar = false;

            this.Left = (SystemParameters.VirtualScreenWidth - this.Width)/2;
            this.Top = (SystemParameters.VirtualScreenHeight - this.Height)/2;


            string parent = System.IO.Directory.GetParent("../..").FullName;
            string fontpath = parent + @"\aronares\arona\font\MainFont.ttf";

            this.Textinputbox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Button clicked!");
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && App.isKBlctrlPress==false)
            {
                if (Textinputbox.Text.Equals(""))
                {
                    App.ctxt = Textinputbox.Text;
                    App.ifboxnull = 1;

                    this.Hide();
                    Textinputbox.Text = "";

                    App.txton = 0;
                    App.internalcounter = 0;
                }
                else if (!Textinputbox.Text.Equals(""))
                {
                    App.ctxt = Textinputbox.Text;

                    this.Hide();
                    Textinputbox.Text = "";

                    App.internalcounter = 0;
                }
                else if (App.internalcounter >= App.globalValues.Speed)
                {
                    this.Hide();
                    Textinputbox.Text = "";

                    App.txton = 0;
                    App.internalcounter = 0;
                    
                }


            }
            
        }


    }
}
