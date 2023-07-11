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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainP1.xaml
    /// </summary>
    public partial class MainP1 : Page
    {
        public MainP1()
        {
            InitializeComponent();
        }

        private void Btn_ins_Click(object sender, RoutedEventArgs e)
        {
            Instructors win = new Instructors();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_sub_Click(object sender, RoutedEventArgs e)
        {
            Sub_List win = new Sub_List();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_osub_Click(object sender, RoutedEventArgs e)
        {
            OpenSub win = new OpenSub();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_cour_Click(object sender, RoutedEventArgs e)
        {
            Course win = new Course();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_room_Click(object sender, RoutedEventArgs e)
        {
            Rooms win = new Rooms();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_mins_Click(object sender, RoutedEventArgs e)
        {
            manageins win = new manageins();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_mng_rooms_Click(object sender, RoutedEventArgs e)
        {
            managerms win = new managerms();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_mng_section_Click(object sender, RoutedEventArgs e)
        {
            managesec win = new managesec();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }
    }
}
