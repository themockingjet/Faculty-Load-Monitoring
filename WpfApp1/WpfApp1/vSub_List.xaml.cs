using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for vSub_List.xaml
    /// </summary>
    /// 
    public partial class vSub_List : UserControl
    {
        DataTable dt;
        DataTable chkdata = new DataTable();
        public vSub_List()
        {
            InitializeComponent();
            
            // Database
            string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("Select * from tblsubject", con);
            dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dtIns.DataContext = dt;

            // Buttons
            btnDelete.IsEnabled = false;

            // Content
            this.DataContext = this;

            chkdata.Columns.Add("subcode", typeof(string));
            chkdata.Columns.Add("Subdesc", typeof(string));

            chkdata.RowChanged += new DataRowChangeEventHandler(chkdata_RowChanged);

        }

        private void chkdata_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if(chkdata.Rows.Count != 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
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

        // DataGrid CheckBox Check
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            DataRow adddata = chkdata.NewRow();
            var subject = dtIns.SelectedItem as DataRowView;
            adddata[0] = subject.Row["Sub_code"].ToString();
            adddata[1] = subject.Row["Sub_desc"].ToString();
            chkdata.Rows.Add(adddata);
        }
        // DataGrid CheckBox Check
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var subject = dtIns.SelectedItem as DataRowView;
            string sc = subject.Row["Sub_code"].ToString();
            string sd = subject.Row["Sub_desc"].ToString();

            for (int i = chkdata.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = chkdata.Rows[i];
                if (dr["subcode"].ToString() == sc && dr["subdesc"].ToString() == sd)
                {
                    dr.Delete();
                }
            }
            chkdata.AcceptChanges();
        }

        // Button Delete
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRow row in chkdata.Rows)
            {
                MessageBox.Show(row[0].ToString());
            }
        }
        
    }
    
}
