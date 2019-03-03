using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for OpenSub.xaml
    /// </summary>
    public partial class OpenSub : Window
    {
        public string get_sem = "";
        public string get_sy = "";
        public OpenSub()
        {
            InitializeComponent();
            vOpenSub UCobj = new vOpenSub();
            bdrContent.Children.Add(UCobj);

            // Close and Minimize start
            this.DataContext = this;
            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            void btn_Min_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Min.Background = Brushes.Transparent;
            }

            void btn_Min_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Min.Background = new SolidColorBrush(Color.FromRgb(79, 83, 91));
            }

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);

            void btn_Close_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Close.Background = Brushes.Transparent;
            }

            void btn_Close_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Close.Background = new SolidColorBrush(Color.FromRgb(220, 20, 60));
            }

            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);

            void btn_Home_MouseLeave(object sender, MouseEventArgs e)
            {
                tblHome.TextDecorations = null;
                imgHome.Width = 50;
                imgHome.Height = 50;
                tblHome.FontSize = 14;
            }

            void btn_Home_MouseEnter(object sender, MouseEventArgs e)
            {
                imgHome.Width = 60;
                imgHome.Height = 60;
                tblHome.TextDecorations = TextDecorations.Underline;
                tblHome.FontSize = 16;
            }

        }

        // Navigation Buttons

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            Main win = new Main();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
