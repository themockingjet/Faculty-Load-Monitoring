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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ChangePass.xaml
    /// </summary>
    public partial class ChangePass : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlDataReader dr;
        public static string user_id = Main.userid;

        bool oldpass = true;
        bool newpass = true;
        bool conpass = true;
        bool newpassold = true;
        bool overall = false;
        public ChangePass()
        {
            InitializeComponent();
        }

        private void Oldpass_TextChanged(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(Connection);
            con.Open();
            cmd = new MySqlCommand("SELECT * FROM tblaccounts WHERE User_id='" + user_id + "' AND Password= SHA2('" + Oldpass.Password + "',256)", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                oldpass = true;
                bdrOld.BorderBrush = Brushes.Black;
                ppOldp.Visibility = Visibility.Collapsed;
            }
            else
            {
                oldpass = false;
                bdrOld.BorderBrush = Brushes.Red;
                ppOldp.Visibility = Visibility.Visible;
            }
            con.Close();

            if (Oldpass.Password == "")
            {
                oldpass = true;
                bdrOld.BorderBrush = Brushes.Black;
                ppOldp.Visibility = Visibility.Collapsed;
            }

            if (Oldpass.Password != "" && Newpass.Password != "" && oldpass == true)
            {
                if (Newpass.Password == Oldpass.Password)
                {
                    newpass = false;
                    bdrNew.BorderBrush = Brushes.Red;
                    ppNewOld.Visibility = Visibility.Visible;
                }
                else
                {
                    newpass = true;
                    bdrNew.BorderBrush = Brushes.Black;
                    ppNewOld.Visibility = Visibility.Collapsed;
                }
            }
            Submit_Check();
        }

        private void Oldpass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (oldpass == false)
            {
                ppOldp.Visibility = Visibility.Visible;
            }
        }

        private void Oldpass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ppOldp.Visibility == Visibility.Visible)
            {
                ppOldp.Visibility = Visibility.Collapsed;
            }
        }

        private void Newpass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(Newpass.Password, "^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{6,}$", RegexOptions.IgnoreCase).Success)
            {
                bdrNew.BorderBrush = Brushes.Red;
                ppNewp.Visibility = Visibility.Visible;
                newpass = false;
            }
            else
            {
                newpass = true;
                bdrNew.BorderBrush = Brushes.Black;
                ppNewp.Visibility = Visibility.Collapsed;
            }
            
            if (Newpass.Password == "")
            {
                newpass = false;
                bdrNew.BorderBrush = Brushes.Black;
                ppNewp.Visibility = Visibility.Collapsed;
            }

            if (Conpass.Password != "")
            {
                if (Conpass.Password != Newpass.Password)
                {
                    newpass = false;
                    bdrCon.BorderBrush = Brushes.Red;
                }
                else
                {
                    newpass = true;
                    bdrCon.BorderBrush = Brushes.Black;
                }
            }

            if (Oldpass.Password != "" && Newpass.Password != "" && oldpass == true)
            {
                if (Newpass.Password == Oldpass.Password)
                {
                    newpassold = false;
                    bdrNew.BorderBrush = Brushes.Red;
                    ppNewOld.Visibility = Visibility.Visible;
                }
                else
                {
                    newpassold = true;
                    bdrNew.BorderBrush = Brushes.Black;
                    ppNewOld.Visibility = Visibility.Collapsed;
                }
            }
            Submit_Check();
        }

        private void Newpass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (newpass == false)
            {
                ppNewp.Visibility = Visibility.Visible;
            }
            if (newpassold == false)
            {
                ppNewOld.Visibility = Visibility.Visible;
            }
        }

        private void Newpass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ppNewp.Visibility == Visibility.Visible)
            {
                ppNewp.Visibility = Visibility.Collapsed;
            }
            if (ppNewOld.Visibility == Visibility.Visible)
            {
                ppNewOld.Visibility = Visibility.Collapsed;
            }
        }

        private void Conpass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Conpass.Password != Newpass.Password)
            {
                if (Newpass.Password != "")
                {
                    bdrCon.BorderBrush = Brushes.Red;
                    conpass = false;
                    ppConp.Visibility = Visibility.Visible;
                }
            }
            else
            {
                conpass = true;
                bdrCon.BorderBrush = Brushes.Black;
                ppConp.Visibility = Visibility.Collapsed;
            }
            Submit_Check();
        }

        private void Conpass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (conpass == false)
            {
                ppConp.Visibility = Visibility.Visible;
            }
        }

        private void Conpass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ppConp.Visibility == Visibility.Visible)
            {
                ppConp.Visibility = Visibility.Visible;
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("UPDATE `tblaccounts` SET `Password`=SHA2(@pass,256) WHERE `User_id`='" + user_id + "'", con);
                cmd.Parameters.AddWithValue("@pass", Newpass.Password);
                con.Open();
                cmd.ExecuteNonQuery();
                Conpass.Password = "";
                Newpass.Password = "";
                Oldpass.Password = "";
                con.Dispose();


                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Main))
                    {
                        (window as Main).grdcontent.Children.Remove(this);
                        (window as Main).grdmaincontent.IsEnabled = true;
                        (window as Main).Logout.IsHitTestVisible = true;
                    }
                }
                MessageBox.Show("Password has been changed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        void Submit_Check()
        {
            if (Oldpass.Password != "" && Newpass.Password != "" && Conpass.Password != "")
            {
                if(oldpass == true && newpass == true && conpass == true && newpassold == true)
                {
                    overall = true;
                }
                else
                {
                    overall = false;
                }
            }
            else
            {
                overall = false;
            }

            if (overall == true)
            {
                btnSubmit.IsEnabled = true;
            }
            else
            {
                btnSubmit.IsEnabled = false;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdcontent.Children.Remove(this);
                    (window as Main).grdmaincontent.IsEnabled = true;
                    (window as Main).Logout.IsHitTestVisible = true;
                }
            }
        }
    }
}
