using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JR.Utils.GUI.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class Rooms : Window
    {
        private static string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        public string roomid = string.Empty;
        string room = string.Empty;
        string type = string.Empty;
        string cap = string.Empty;

        DataTable dt;

        public Rooms()
        {
            InitializeComponent();

            // Close and Minimize start
            this.DataContext = this;
            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);
            
            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);


            //
            Load_Rooms();
            txtCuraysem.Text = "ACADEMIC YEAR: " + Main.main_schoolyear + "   SEMESTER: " + Main.main_semester;
        }

        #region MAIN MENUBAR
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

        #endregion

        // ========================= MAIN ========================
        public void Load_Rooms()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblroom ORDER BY Room_name", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dtRoom.ItemsSource = dt.DefaultView;
        }
        
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            mRoom UCobj = new mRoom();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Rooms))
                {
                    (window as Rooms).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            dtRoom.SelectedIndex = -1;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            roomid = string.Empty;
            mRoom UCobj = new mRoom();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Rooms))
                {
                    (window as Rooms).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            dtRoom.SelectedIndex = -1;
        }

        private void DtRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtRoom.SelectedIndex != -1)
            {
                btnUpdate.IsEnabled = true;
                roomid = ((DataRowView)dtRoom.SelectedItem).Row["Room_id"].ToString();

                RoomT.Text = ((DataRowView)dtRoom.SelectedItem).Row["Room_name"].ToString();
                TypeT.Text = ((DataRowView)dtRoom.SelectedItem).Row["Room_type"].ToString();
                CapT.Text = ((DataRowView)dtRoom.SelectedItem).Row["Room_cap"].ToString();
            }
            else
            {
                btnUpdate.IsEnabled = false;
            }
        }
        

        private void ChkAll_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["Check"] = true;
                }
            }
            else if (checkBox.IsChecked == false)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["Check"] = false;
                }
            }
        }

        private void S_codeT_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchW.Visibility = Visibility.Collapsed;
        }

        private void S_codeT_LostFocus(object sender, RoutedEventArgs e)
        {
            string x = S_codeT.Text;
            string xx = x.Replace("\\s", "");
            if (xx.Length == 0)
            {
                SearchW.Visibility = Visibility.Visible;
            }
        }

        private void S_codeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("Room_name LIKE '{0}%' OR Room_type LIKE '{0}%'", S_codeT.Text);
        }
    }
}
