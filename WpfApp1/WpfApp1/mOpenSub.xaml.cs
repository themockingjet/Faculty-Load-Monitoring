using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mOpenSub.xaml
    /// </summary>
    public partial class mOpenSub : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";
        DataTable dt;
         string sy = "";
       string sem = "";

        public mOpenSub()
        {
            InitializeComponent();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(OpenSub))
                {
                    sy = (window as OpenSub).get_sy;
                    sem = (window as OpenSub).get_sem;
                    Load_Grid();
                }
            }

            // Database
        }

        void Load_Grid()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("SELECT * FROM tblsubject WHERE tblsubject.Sub_code NOT IN (SELECT tblopen_sub.Sub_code FROM tblopen_sub WHERE tblopen_sub.semester='" + sem + "' AND tblopen_sub.sy='" + sy +"')", con);
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
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Sub_desc":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "T_units":
                    e.Column.CanUserSort = false;
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
    }
}
