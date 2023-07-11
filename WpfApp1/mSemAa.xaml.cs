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
    /// Interaction logic for mSemAa.xaml
    /// </summary>
    public partial class mSemAa : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        string sem = Schoolyear.cursemester;
        string sy = Schoolyear.curschoolyear;

        string cr = string.Empty;
        string cp = string.Empty;
        string ca = string.Empty;
        
        
        public mSemAa()
        {
            InitializeComponent();
            Load_Sy_Details();
        }

        private void Load_Sy_Details()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_id='" + Schoolyear.cursemid + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            sem = dr[1].ToString();
                            cr = dr[2].ToString();
                            cp = dr[3].ToString();
                            ca = dr[4].ToString();

                            txtCr.Text = dr[2].ToString();
                            txtCp.Text = dr[3].ToString();
                            txtCa.Text = dr[4].ToString();
                            btnSave.IsEnabled = false;
                        }
                    }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Done managing Academic Administrator for Semester " + sem + " Academic Year: " + sy + " ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Schoolyear))
                    {
                        (window as Schoolyear).grdcontent.Children.Remove(this);
                        (window as Schoolyear).bdrContent.IsEnabled = true;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(Connection);
                MySqlCommand cmd = new MySqlCommand("UPDATE `tblsem` SET `Sem_cr`=@cr,`Sem_cp`=@cp,`Sem_ca`=@ca WHERE Sem_id='" + Schoolyear.cursemid + "'", con);
                cmd.Parameters.AddWithValue("@cr", txtCr.Text);
                cmd.Parameters.AddWithValue("@cp", txtCp.Text);
                cmd.Parameters.AddWithValue("@ca", txtCa.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Academic Administration has been successfully updated");
                Load_Sy_Details();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        void Save_Button_Check()
        {
            if (txtCr.Text == cr && txtCp.Text == cp && txtCa.Text == ca)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                if (txtCr.Text == "" || txtCp.Text == "" || txtCa.Text == "")
                {
                    btnSave.IsEnabled = false;
                }
                else
                {
                    btnSave.IsEnabled = true;
                }
            }
        }

        private void TxtCr_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Button_Check();
        }

        private void TxtCp_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Button_Check();
        }

        private void TxtCa_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Button_Check();
        }
    }
}
