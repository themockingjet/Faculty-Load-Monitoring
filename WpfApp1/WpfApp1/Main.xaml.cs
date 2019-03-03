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
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
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
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Btn_ins_Click(object sender, RoutedEventArgs e)
        {
            Instructors win = new Instructors();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        private void Btn_sub_Click(object sender, RoutedEventArgs e)
        {
            Sub_List win = new Sub_List();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }
        

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Btn_osub_Click(object sender, RoutedEventArgs e)
        {
            OpenSub win = new OpenSub();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        private void Btn_cour_Click(object sender, RoutedEventArgs e)
        {
            Course win = new Course();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        private void Btn_room_Click(object sender, RoutedEventArgs e)
        {
            Rooms win = new Rooms();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        private void Btn_user_Click(object sender, RoutedEventArgs e)
        {
            Schedule win = new Schedule();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }
    }
}
