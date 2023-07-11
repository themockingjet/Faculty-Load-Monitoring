using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
        private static string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlDataReader dr;

        public static string main_schoolyear = string.Empty;
        public static string main_semester = string.Empty;

        public static string schoolyear_id = string.Empty;
        public static string semester_id = string.Empty;

        public static string userid = MainWindow.userid;
        
        private static int curpage = 1;
        public Main()
        {
            InitializeComponent();
            this.DataContext = this;

            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);
            load_ay_sem();

            if (curpage == 1)
            {
                MainF.Navigate(new MainP1());
                button.Visibility = Visibility.Collapsed;
                button1.Visibility = Visibility.Visible;
            }
            else if (curpage == 2)
            {
                MainF.Navigate(new MainP2());
                button.Visibility = Visibility.Visible;
                button1.Visibility = Visibility.Collapsed;
            }

            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblaccounts WHERE User_id='" + userid + "'", con);
                con.Open();
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Logout.Text = dr[1].ToString();
                        }
                    }
                }
            }

        }

        #region Main Panel
        void btn_Min_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_Min.Background = Brushes.Transparent;
        }

        void btn_Min_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_Min.Background = new SolidColorBrush(Color.FromRgb(79, 83, 91));
        }

        void btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_Close.Background = Brushes.Transparent;
        }

        void btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_Close.Background = new SolidColorBrush(Color.FromRgb(220, 20, 60));
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        
        private void Logout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow win = new MainWindow();
                win.Owner = this.Owner;
                win.Show();
                this.Close();
            }
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            curpage = 1;
            MainF.Navigate(new MainP1());
            button.Visibility = Visibility.Collapsed;
            button1.Visibility = Visibility.Visible;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            curpage = 2;
            MainF.Navigate(new MainP2());
            button.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Collapsed;
        }

        void load_ay_sem()
        {
            string syid = string.Empty;
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblsy WHERE Sy_cur='1'", con);
                con.Open();
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            syid = dr[0].ToString();
                            string systart = dr[1].ToString();
                            string syend = dr[2].ToString();
                            main_schoolyear = systart + " - " + syend;
                            schoolyear_id = dr[0].ToString();
                        }
                    }
                }
            }
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_cur='1' and Sy_id='" + syid + "'", con);
                con.Open();
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            main_semester = dr[1].ToString();
                            semester_id = dr[0].ToString();
                        }
                    }
                }
            }
            txtCuraysem.Text = "ACADEMIC YEAR: " + main_schoolyear + "   SEMESTER: " + main_semester;
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var a = sender as TextBlock;
            a.TextDecorations = TextDecorations.Underline;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var a = sender as TextBlock;
            a.TextDecorations = null;
        }

        private void Listacc_MouseEnter(object sender, MouseEventArgs e)
        {
            listacc.Visibility = Visibility.Visible;
            
            if (listacc.Visibility == Visibility.Visible)
            {
                Logout.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                Logout.TextDecorations = null;
            }
        }

        private void Listacc_MouseLeave(object sender, MouseEventArgs e)
        {
            listacc.Visibility = Visibility.Collapsed;


            if (listacc.Visibility == Visibility.Visible)
            {
                Logout.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                Logout.TextDecorations = null;
            }
        }

        private void Logout_MouseEnter(object sender, MouseEventArgs e)
        {
            var a = sender as TextBlock;
            a.TextDecorations = TextDecorations.Underline;
            a.Foreground = Brushes.Blue;
            listacc.Visibility = Visibility.Visible;

            if (listacc.Visibility == Visibility.Visible)
            {
                Logout.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                Logout.TextDecorations = null;
            }
        }

        private void Logout_MouseLeave(object sender, MouseEventArgs e)
        {
            var a = sender as TextBlock;
            a.TextDecorations = TextDecorations.Underline;
            a.Foreground = Brushes.Blue;
            listacc.Visibility = Visibility.Collapsed;

            if (listacc.Visibility == Visibility.Visible)
            {
                Logout.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                Logout.TextDecorations = null;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangePass UCobj = new ChangePass();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdcontent.Children.Add(UCobj);
                    (window as Main).grdmaincontent.IsEnabled = false;
                    (window as Main).Logout.IsHitTestVisible = false;
                }
            }
        }
    }
}
