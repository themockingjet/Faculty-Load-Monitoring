using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Recover.xaml
    /// </summary>
    public partial class Recover : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        bool recover = false;
        public Recover()
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
                btn_Close.Margin = new Thickness(232, -11, -9, 0);
                btn_Close.Height = 28;
                btn_Close.Width = 28;
            }

            void btn_Close_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Close.Margin = new Thickness(232, -14, -13, 0);
                btn_Close.Height = 32;
                btn_Close.Width = 32;
            }

            ComboBox_SecQues();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    if ((window as MainWindow).first != false)
                    {
                        MessageBox.Show("true");
                        recover = false;
                    }
                    else
                    {
                        recover = true;
                        txtSec.Text = "Security Question";
                        Title.Text = "Forgot Password";
                    }
                }
            }
        }


        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (recover == false)
            {
                MessageBoxResult result = MessageBox.Show("Cancel Account Recovery Set up and go back to login page?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow win = new MainWindow();
                    win.Owner = this.Owner;
                    win.Show();
                    this.Close();
                }
            }
            else if (recover == true)
            {
                MessageBoxResult result = MessageBox.Show("Cancel Account Recovery and go back to login page?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow win = new MainWindow();
                    win.Owner = this.Owner;
                    win.Show();
                    this.Close();
                }
            }
        }

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        void ComboBox_SecQues()
        {
            cbQuestion.Items.Add("What is the first name of your favorite niece/nephew?");
            cbQuestion.Items.Add("On what street is your grocery store?");
            cbQuestion.Items.Add("What is the name of your highschool's star athlete?");
            cbQuestion.Items.Add("What is the first name of the man/maid of honor at your wedding?");
            cbQuestion.Items.Add("What was the first live concert you attended?");
            cbQuestion.Items.Add("What is the name of the medical professional who delievered your first child?");
            cbQuestion.Items.Add("What is the name of a college you applied to but didn't attend?");
            cbQuestion.Items.Add("What is the first name of your hairdresser/barber?");
            cbQuestion.Items.Add("What was the first name of your favorite Teacher or Professor?");
            cbQuestion.Items.Add("What city were you in to celebrate 2019?");
            cbQuestion.Items.Add("As a child, what did you want to be when you grew up?");
            cbQuestion.Items.Add("Who is your favorite person in history?");
            cbQuestion.Items.Add("What is the last name of your family physician?");
            cbQuestion.Items.Add("What sports team do you love to see lose?");
            cbQuestion.Items.Add("What celebrity do you most resemble?");
            cbQuestion.Items.Add("In what city did you meet your spouse/significant other?");
            cbQuestion.Items.Add("What is the last name of your favorite author?");
            cbQuestion.Items.Add("What was your first job?");
            cbQuestion.Items.Add("What is the last name of your favorite sports hero?");
            cbQuestion.Items.Add("Who is your favorite comic book or cartoon character?");
        }

        void LetterNumber(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[A-Za-z0-9]$"))
            {
                e.Handled = true;
            }
        }

        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (cbQuestion.SelectedIndex != -1 && txtAns.Password.Length >= 6 && recover == false)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("UPDATE `tblaccounts` SET `Logged`=1,`Sec_quest`=@ques,`Sec_ans`=SHA2(@ans,256) WHERE Username ='Admin'", con);
                cmd.Parameters.AddWithValue("@ques", cbQuestion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@ans", txtAns.Password);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Account Recovery has been updated successfully.");
                Main win = new Main();
                win.Owner = this.Owner;
                win.Show();
                this.Close();
            }
            else if (cbQuestion.SelectedIndex != -1 && txtAns.Password.Length >= 6 && recover == true)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT * FROM `tblaccounts` WHERE Sec_quest ='" + cbQuestion.SelectedValue.ToString() + "' AND Sec_ans =SHA2('" + txtAns.Password + "',256)", con);
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    con.Close();
                    cmd = new MySqlCommand("UPDATE `tblaccounts` SET `Password`=SHA2(@pass,256) WHERE Username ='Admin'", con);
                    cmd.Parameters.AddWithValue("@pass", "Admin");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Password has been successfully reset.");

                    MainWindow win = new MainWindow();
                    win.Owner = this.Owner;
                    win.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect account recovery info.");
                }
            }
        }

        private void TxtAns_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Submit_Button();
        }

        private void CbQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Submit_Button();
        }
        
        void Submit_Button()
        {
            if (txtAns.Password.Length >= 6 && cbQuestion.SelectedIndex != -1)
            {
                btn_Submit.IsEnabled = true;
            }
            else
            {
                btn_Submit.IsEnabled = false;
            }
        }
    }
}
