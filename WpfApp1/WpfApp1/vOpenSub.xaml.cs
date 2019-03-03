using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for vOpenSub.xaml
    /// </summary>
    public partial class vOpenSub : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";
        string sy = "";
        string sem = "";
        DataTable dt;
        public vOpenSub()
        {
            InitializeComponent();
            Load_Sy();
            Load_Sem();
        }
        // Load Data Grid
        void Load_Grid()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("SELECT * FROM tblopensub JOIN tblsubject USING (Sub_code) WHERE tblsubject.Sub_code = tblopensub.Sub_code AND tblopensub.Sy = '" + sy + "'AND tblopensub.Sem = '" + sem + "'", con);
            dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dtIns.DataContext = dt;
        }
        // Data Grid Columns
        private void DtIns_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Sub_code":
                    e.Column.CanUserSort = false;
                    e.Column.CanUserReorder = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Sub_desc":
                    e.Column.CanUserSort = false;
                    e.Column.CanUserReorder = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "T_units":
                    e.Column.CanUserSort = false;
                    e.Column.CanUserReorder = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Year":
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Course":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "Semester":
                    e.Column.Visibility = Visibility.Visible;
                    break;
                default:
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        // Load ComboBox School Year
        void Load_Sy()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                MySqlCommand cmd = new MySqlCommand("select * from tblschoolyr", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string reg = dr.GetString(1) + " - " + dr.GetString(2);
                        cbSy.Items.Add(reg);
                    }
                }
            }
        }
        void Load_Sem()
        {
            cbSemester.Items.Add("FIRST");
            cbSemester.Items.Add("SECOND");
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(OpenSub))
                {
                    (window as OpenSub).get_sem = sem;
                    (window as OpenSub).get_sy = sy;
                }
            }


            mOpenSub UCobj = new mOpenSub();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(OpenSub))
                {
                    (window as OpenSub).grdcontent.Children.Add(UCobj);
                }
            }
        }

        private void CbSemester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sem = cbSemester.SelectedValue.ToString();
            if (cbSemester.SelectedIndex == -1)
            {

                tblSemester.Visibility = Visibility.Visible;
            }
            else
            {
                tblSemester.Visibility = Visibility.Collapsed;
                if (cbSy.SelectedValue != null)
                {
                    Load_Grid();
                }
            }

        }

        private void CbSy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sy = cbSy.SelectedValue.ToString();
            if (cbSy.SelectedIndex == -1)
            {
                tblSy.Visibility = Visibility.Visible;
            }
            else
            {
                tblSy.Visibility = Visibility.Collapsed;
                if (cbSemester.SelectedValue != null)
                {
                    Load_Grid();
                }
            }
        }
    }
}
