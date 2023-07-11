using MySql.Data.MySqlClient;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        bool clicked = false;
        bool user = false;
        public bool first = false;
        public static string userid = string.Empty;

        private void checkifconnected()
        {
            MySqlConnection connect = new MySqlConnection(Connection);
            try
            {
                connect.Open();
            }
            catch
            {
                MessageBox.Show("Unable to connect to the database.", "ERROR");
                Close();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            void btn_Min_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Min.Margin = new Thickness(204, -11, 0, 0);
                btn_Min.Height = 28;
                btn_Min.Width = 28;
            }

            void btn_Min_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Min.Margin = new Thickness(200, -14, 0, 0); 
                btn_Min.Height = 32;
                btn_Min.Width = 32;
            }

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);

            void btn_Close_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Close.Margin = new Thickness(232,-11,-9,0);
                btn_Close.Height = 28;
                btn_Close.Width = 28;
            }

            void btn_Close_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Close.Margin = new Thickness(232, -14, -13, 0);
                btn_Close.Height = 32;
                btn_Close.Width = 32;
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            clicked = true;
            if (txtUser.Text == "")
            {
                txtUser.BorderBrush = Brushes.Red;
                ToolTip tooltip = new ToolTip { Content = "Username cannot be empty." };
                txtUser.ToolTip = tooltip;
            }
            if (txtPass.Password == "")
            {
                txtPass.BorderBrush = Brushes.Red;
                ToolTip tooltip = new ToolTip { Content = "Password cannot be empty." };
                txtPass.ToolTip = tooltip;
            }
            if (user == true && txtPass.Password != "")
            {
                MySqlConnection con = new MySqlConnection(Connection);
                //cmd = new MySqlCommand("SELECT * FROM `tblaccounts` WHERE Username ='" + txtUser.Text + "' AND Password = SHA2('" + txtPass.Password + "',256)", con);
                cmd = new MySqlCommand("SELECT * FROM `tblaccounts` WHERE Username ='" + txtUser.Text + "' AND Password = '" + txtPass.Password + "'", con);
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userid = dr[0].ToString();
                        if ((bool)dr[3] == false)
                        {
                            first = true;
                            Recover win = new Recover();
                            win.Owner = this.Owner;
                            win.Show();
                            this.Hide();
                        }
                        else
                        {
                            first = false;
                            Main win = new Main();
                            win.Owner = this.Owner;
                            win.Show();
                            this.Hide();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Password.");
                }
            }
        }

        private void TxtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (txtUser.Text.Length != 0)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT `Username` FROM `tblaccounts` WHERE Username ='" + txtUser.Text + "'", con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        txtUser.BorderBrush = Brushes.Black;
                        txtUser.ClearValue(TextBlock.ToolTipProperty);
                        user = true;
                    }
                    else
                    {
                        txtUser.BorderBrush = Brushes.Red;
                        ToolTip tooltip = new ToolTip { Content = "Username does not exist." };
                        txtUser.ToolTip = tooltip;
                        user = false;
                    }
                }
                else
                {
                    txtUser.BorderBrush = Brushes.Red;
                    ToolTip tooltip = new ToolTip { Content = "Username cannot be empty." };
                    txtUser.ToolTip = tooltip;
                    user = false;
                }
            }
            else
            {
                if (txtUser.Text.Length != 0)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT `Username` FROM `tblaccounts` WHERE Username ='" + txtUser.Text + "'", con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        txtUser.BorderBrush = Brushes.Black;
                        txtUser.ClearValue(TextBlock.ToolTipProperty);
                        user = true;
                    }
                    else
                    {
                        txtUser.BorderBrush = Brushes.Red;
                        ToolTip tooltip = new ToolTip { Content = "Username does not exist." };
                        txtUser.ToolTip = tooltip;
                        user = false;
                    }
                }
                else
                {
                    txtUser.BorderBrush = Brushes.Black;
                    txtUser.ClearValue(TextBlock.ToolTipProperty);
                }
            }
        }

        private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (txtPass.Password.Length != 0)
                {
                    txtPass.BorderBrush = Brushes.Black;
                    txtPass.ClearValue(TextBlock.ToolTipProperty);
                }
                else
                {
                    txtPass.BorderBrush = Brushes.Red;
                    ToolTip tooltip = new ToolTip { Content = "Password cannot be empty." };
                    txtPass.ToolTip = tooltip;
                }
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                Recover win = new Recover();
                win.Owner = this.Owner;
                win.Show();
                this.Hide();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            checkifconnected();
        }
    }
}
