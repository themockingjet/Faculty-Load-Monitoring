using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Media;
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
    /// Interaction logic for mSem.xaml
    /// </summary>
    public partial class mSem : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        
        public static string sy = string.Empty;
        public static string cursy = string.Empty;

        bool add = false;
        DataTable dt;
        public mSem()
        {
            InitializeComponent();

            // Load School Year
            
            Fill_Acad();
        }

        public void Fill_Acad()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdIns = new MySqlCommand("SELECT *,CONCAT(Sy_start,' - ',Sy_end) as Sy FROM tblsy WHERE Sy_prev != 0 AND Sy_id !='" + Schoolyear.cursyid + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmdIns);
            dt = new DataTable();
            da.Fill(dt);
            dtSy.ItemsSource = dt.DefaultView;
        }
        

        private void BtnClose_Click(object sender, RoutedEventArgs e)
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

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            Schoolyear.gobacksemid = Schoolyear.cursemid;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to set Academic Year: " + cursy + " ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (MySqlConnection con = new MySqlConnection(Connection))
                {
                    cmd = new MySqlCommand("UPDATE `tblsem` SET `Sem_cur`=0 WHERE `Sem_cur`=1 AND `Sy_id`='" + Schoolyear.cursyid + "';UPDATE `tblsy` SET `Sy_cur`=0 WHERE Sy_id='" + Schoolyear.cursyid + "';", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    cmd = new MySqlCommand("UPDATE `tblsy` SET `Sy_cur`=1 WHERE Sy_id='" + sy + "';UPDATE `tblsem` SET `Sem_cur`=1 WHERE `Sem_name`='FIRST' AND `Sem_cur`=0 AND `Sy_id`='" + sy + "';", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Successfully changed the current academic year.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Schoolyear))
                        {
                            (window as Schoolyear).grdcontent.Children.Remove(this);
                            (window as Schoolyear).load_acad_year();
                            (window as Schoolyear).load_acad_sem();
                            (window as Schoolyear).bdrContent.IsEnabled = true;
                        }
                    }
                }
            }
        }
        
        private void DtSy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtSy.SelectedIndex != -1)
            {
                sy = ((DataRowView)dtSy.SelectedItem).Row["Sy_id"].ToString();
                cursy = ((DataRowView)dtSy.SelectedItem).Row["Sy_start"].ToString() + " - " + ((DataRowView)dtSy.SelectedItem).Row["Sy_end"].ToString();
                btnSet.IsEnabled = true;
            }
            else
            {
                btnSet.IsEnabled = false;
                sy = string.Empty;
            }
        }
    }
}
