using JR.Utils.GUI.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Schoolyear.xaml
    /// </summary>
    public partial class Schoolyear : Window
    {
        private static string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;


        public static string cursyid = string.Empty;
        public static string curschoolyear = string.Empty;

        public static string cursemid = string.Empty;
        public static string cursemester = string.Empty;

        public static string gobacksemid = string.Empty;


        public Schoolyear()
        {
            InitializeComponent();

            this.DataContext = this;
            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);
            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);
            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);

            load_acad_year();
            load_acad_sem();
            txtCuraysem.Text = "ACADEMIC YEAR: " + curschoolyear + "   SEMESTER: " + cursemester;
        }

        #region MenuToolBox
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

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            Main win = new Main();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }
        #endregion
        // ===================== MAIN ============================
        public void load_acad_year()
        {
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
                            string systart = dr[1].ToString();
                            string syend = dr[2].ToString();
                            curschoolyear = systart + " - " + syend;
                            cursyid = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtEndAY.Text = "(GO BACK TO CURRENT A.Y.)";
                            }
                            else
                            {
                                txtEndAY.Text = "(END ACADEMIC YEAR)";
                            }
                        }
                    }
                }
            }
        }

        public void load_acad_sem()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_cur='1' and Sy_id='" + cursyid + "'", con);
                con.Open();
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cursemester = dr[1].ToString();
                            cursemid = dr[0].ToString();
                        }
                    }
                }
            }
            txtcuray.Text = curschoolyear;
            txtcursem.Text = cursemester;
        }

        void ComboBox_Sem()
        {
            cbSem.ItemsSource = null;
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_name != '" + cursemester + "'AND Sy_id='" + cursyid + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbSem.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        private void BtnManageSem_Click(object sender, RoutedEventArgs e)
        {
            mSem UCobj = new mSem();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schoolyear))
                {
                    (window as Schoolyear).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var text = sender as TextBlock;
            text.TextDecorations = TextDecorations.Underline;
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            var text = sender as TextBlock;
            text.TextDecorations = null;
        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ComboBox_Sem();
            var text = sender as TextBlock;
            if (text.Text == "(CHANGE)")
            {
                text.Text = "(CANCEL)";
                cbSem.Visibility = Visibility.Visible;
                txtcursem.Visibility = Visibility.Collapsed;
            }
            else if (text.Text == "(CANCEL)")
            {
                text.Text = "(CHANGE)";
                cbSem.Visibility = Visibility.Collapsed;
                txtcursem.Visibility = Visibility.Visible;
            }
        }

        private void TxtEndAY_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(txtEndAY.Text == "(END ACADEMIC YEAR)")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to end this Academic Year?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (con = new MySqlConnection(Connection))
                    {
                        MessageBox.Show("Successfully ended academic year: " + curschoolyear);
                        string[] sysplit = curschoolyear.Split(' ', '-');
                        int systart = int.Parse(sysplit[3]);
                        cmd = new MySqlCommand("UPDATE `tblsem` SET `Sem_cur`=0 WHERE `Sem_cur`=1 AND `Sy_id`='" + cursyid + "';UPDATE `tblsy` SET `Sy_cur`=0, `Sy_prev`=1 WHERE Sy_id='" + cursyid + "';", con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        cmd = new MySqlCommand("INSERT INTO `tblsy`(`Sy_start`,`Sy_end`,`Sy_cur`,`Sy_prev`) VALUES (@sys,@sye,1,0)", con);
                        cmd.Parameters.AddWithValue("@sys", systart);
                        cmd.Parameters.AddWithValue("@sye", systart + 1);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        load_acad_year();
                        Create_Sem();
                        load_acad_sem();
                    }
                }
            }
            else if (txtEndAY.Text == "(GO BACK TO CURRENT A.Y.)")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to go back to the current Academic Year?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (con = new MySqlConnection(Connection))
                    {
                        cmd = new MySqlCommand("UPDATE `tblsem` SET `Sem_cur`=0 WHERE `Sem_cur`=1 AND `Sy_id`='" + cursyid + "';UPDATE `tblsem` SET `Sem_cur`=1 WHERE `Sem_id`='" + gobacksemid + "';UPDATE `tblsy` SET `Sy_cur`=0 WHERE Sy_id='" + cursyid + "';UPDATE `tblsy` SET `Sy_cur`=1 WHERE Sy_prev=0", con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        using (MySqlConnection con = new MySqlConnection(Connection))
                        {
                            cmd = new MySqlCommand("SELECT * FROM tblsy WHERE Sy_prev='0'", con);
                            con.Open();
                            using (dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        string sys = dr[1].ToString();
                                        string sye = dr[2].ToString();
                                        curschoolyear = sys + " - " + sye;
                                        cursyid = dr[0].ToString();
                                        if (dr[4].ToString() == "True")
                                        {
                                            txtEndAY.Text = "(GO BACK TO CURRENT A.Y.)";
                                        }
                                        else
                                        {
                                            txtEndAY.Text = "(END ACADEMIC YEAR)";
                                        }
                                    }
                                }
                            }
                        }

                        MessageBox.Show("Successfully returned to the current academic year: " + curschoolyear);

                        load_acad_year();
                        load_acad_sem();
                    }
                }
            }
        }

        void Create_Sem()
        {
            cmd = new MySqlCommand("INSERT INTO `tblsem`(`Sem_name`, `Sem_cr`, `Sem_cp`, `Sem_ca`, `Sem_cur`, `Sy_id`) VALUES ('FIRST',NULL,NULL,NULL,1,@syid)", con);
            cmd.Parameters.AddWithValue("@syid", cursyid);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            cmd = new MySqlCommand("INSERT INTO `tblsem`(`Sem_name`, `Sem_cr`, `Sem_cp`, `Sem_ca`, `Sem_cur`, `Sy_id`) VALUES ('SECOND',NULL,NULL,NULL,0,@syid)", con);
            cmd.Parameters.AddWithValue("@syid", cursyid);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            cmd = new MySqlCommand("INSERT INTO `tblsem`(`Sem_name`, `Sem_cr`, `Sem_cp`, `Sem_ca`, `Sem_cur`, `Sy_id`) VALUES ('MID YEAR',NULL,NULL,NULL,0,@syid)", con);
            cmd.Parameters.AddWithValue("@syid", cursyid);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // MANAGE ACAD ADMIN
        private void BtnManageAa_Click(object sender, RoutedEventArgs e)
        {
            mSemAa obj = new mSemAa();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schoolyear))
                {
                    (window as Schoolyear).grdcontent.Children.Add(obj);
                    bdrContent.IsEnabled = false;
                }
            }
        }

        private void CbSem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSem.SelectedIndex != -1)
            {
                using (con = new MySqlConnection(Connection))
                {
                    cmd = new MySqlCommand("UPDATE `tblsem` SET `Sem_cur`=0 WHERE `Sem_cur`=1 AND `Sy_id`='" + cursyid + "';UPDATE `tblsem` SET `Sem_cur`=1 WHERE `Sem_cur`=0 AND `Sem_id`='" + cbSem.SelectedValue.ToString() + "'AND `Sy_id`='" + cursyid + "';", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    txtsemchange.Text = "(CHANGE)";
                    cbSem.Visibility = Visibility.Collapsed;
                    txtcursem.Visibility = Visibility.Visible;
                    load_acad_sem();
                    MessageBox.Show("Successfully changed the current semester.");
                }
            }
        }
    }
}
