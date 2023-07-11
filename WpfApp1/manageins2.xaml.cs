using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for manageins2.xaml
    /// </summary>
    public partial class manageins2 : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        DataTable dt;
        string syid = Main.schoolyear_id;
        string semid = Main.semester_id;

        string scode = string.Empty;
        string sid = string.Empty;
        public manageins2()
        {
            InitializeComponent();

            Load_Grid();
            dt.RowChanged += new DataRowChangeEventHandler(dt_RowChanged);
        }

        private void dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            int x = 0;
            int xx = dt.Rows.Count;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    x += 1;
                }
            }

            if (x == xx)
            {
                chkAll.IsChecked = true;
            }
            else
            {
                chkAll.IsChecked = false;
            }
        }

        void Load_Grid()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("SELECT Ins_id,CONCAT(LName,', ',FName) as FlName, FName, LName FROM tblins WHERE Ins_id NOT IN (SELECT Ins_id FROM tblopenins WHERE tblopenins.Sem_id='" + semid + "' AND tblopenins.Sy_id='" + syid + "') ORDER BY LName", con);
            dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            var col = new DataColumn("Check", typeof(bool));
            col.DefaultValue = false;
            dt.Columns.Add(col);
            dtMopen.DataContext = dt;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dtMopen.SelectedItem != null)
            {
                var subject = dtMopen.SelectedItem as DataRowView;
                subject.Row["Check"] = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtMopen.SelectedItem != null)
            {
                var subject = dtMopen.SelectedItem as DataRowView;
                subject.Row["Check"] = false;
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(manageins))
                {
                    (window as manageins).grdcontent.Children.Remove(this);
                    (window as manageins).bdrContent.IsEnabled = true;
                }
            }
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            string xx = string.Empty;
            string xx1 = string.Empty;
            string xid = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    x += 1;
                    xx +=  row["FName"].ToString() + "\n";
                    xx1 = row["FName"].ToString();
                    xid = row["Ins_id"].ToString();
                }
            }
            MySqlConnection con = new MySqlConnection(Connection);
            if (x > 1)
            {
                MessageBoxResult result = MessageBox.Show("Set these Instructors as active?\n\n " + xx, "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((bool)row["Check"] == true)
                        { 
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblopenins`(`Ins_id`, `Sem_id`, `Sy_id`) VALUES (@iid,@sem,@sy)", con);
                            cmd.Parameters.AddWithValue("@iid", row["Ins_id"].ToString());
                            cmd.Parameters.AddWithValue("@sem", semid);
                            cmd.Parameters.AddWithValue("@sy", syid);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    Load_Grid();
                    MessageBox.Show(xx + "\n\nAre now active.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(manageins))
                        {
                            (window as manageins).grdcontent.Children.Remove(this);
                            (window as manageins).Load_Grid();
                            (window as manageins).bdrContent.IsEnabled = true;
                        }
                    }
                }

            }
            else if (x == 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to set " + xx1 + " as active?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var chk = row["Check"];
                        if ((bool)chk != false)
                        {
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblopenins`(`Ins_id`, `Sem_id`, `Sy_id`) VALUES (@iid,@sem,@sy)", con);
                            cmd.Parameters.AddWithValue("@iid", row["Ins_id"].ToString());
                            cmd.Parameters.AddWithValue("@sem", semid);
                            cmd.Parameters.AddWithValue("@sy", syid);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    Load_Grid();
                    MessageBox.Show(xx + " is now active.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(manageins))
                        {
                            (window as manageins).grdcontent.Children.Remove(this);
                            (window as manageins).Load_Grid();
                            (window as manageins).bdrContent.IsEnabled = true;
                        }
                    }
                }
            }
            else if (x == 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to set " + scode + " as active?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblopenins`(`Ins_id`, `Sem_id`, `Sy_id`) VALUES (@iid,@sem,@sy)", con);
                    cmd.Parameters.AddWithValue("@iid", sid);
                    cmd.Parameters.AddWithValue("@sem", semid);
                    cmd.Parameters.AddWithValue("@sy", syid);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show(scode + " is now active.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(manageins))
                        {
                            (window as manageins).grdcontent.Children.Remove(this);
                            (window as manageins).Load_Grid();
                            (window as manageins).bdrContent.IsEnabled = true;
                        }
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    //code for Cancel
                }

            }
        }

        private void DtMopen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtMopen.SelectedIndex > -1)
            {
                int x = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if ((bool)row["Check"] == true)
                    {
                        x += 1;
                    }
                }
                if (x > 1)
                {
                    btnOpen.IsEnabled = true;
                }
                else if (x == 1)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((bool)row["Check"] == true)
                        {
                            sid = row["Ins_id"].ToString();
                            scode = row["FlName"].ToString();
                            btnOpen.IsEnabled = true;
                        }
                    }
                }
                else if (x == 0)
                {
                    sid = ((DataRowView)dtMopen.SelectedItem).Row["Ins_id"].ToString();
                    scode = ((DataRowView)dtMopen.SelectedItem).Row["FlName"].ToString();
                    btnOpen.IsEnabled = true;
                }
            }
            else
            {
                btnOpen.IsEnabled = false;
            }
        }

        private void ChkAll_Click(object sender, RoutedEventArgs e)
        {
            if (S_codeT.Text == "")
            {
                if (chkAll.IsChecked == true)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        row["Check"] = true;
                    }
                }
                else if (chkAll.IsChecked == false)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        row["Check"] = false;
                    }
                }
            }
            else
            {
                if (chkAll.IsChecked == true)
                {
                    DataRow[] dataRows = dt.Select("FName LIKE '" + S_codeT.Text + "%' OR LName LIKE '" + S_codeT.Text + "%'");
                    for (int i = 0; i < dataRows.Length; i++)
                    {
                        dataRows[i]["Check"] = true;
                    }
                }
                else if (chkAll.IsChecked == false)
                {
                    DataRow[] dataRows = dt.Select("FName LIKE '" + S_codeT.Text + "%' OR LName LIKE '" + S_codeT.Text + "%'");
                    for (int i = 0; i < dataRows.Length; i++)
                    {
                        dataRows[i]["Check"] = false;
                    }
                }
            }
        }

        private void S_codeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("LName LIKE '{0}%' OR FName LIKE '{0}%'", S_codeT.Text);

            int x = 0;
            int xx = dt.Rows.Count;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    x += 1;
                }
            }

            if (S_codeT.Text == "")
            {
                if (x == xx)
                {
                    chkAll.IsChecked = true;
                }
                else
                {
                    chkAll.IsChecked = false;
                }
            }
            else
            {
                DataRow[] dataRows = dt.Select("FName LIKE '" + S_codeT.Text + "%' OR LName LIKE '" + S_codeT.Text + "%'");
                if (x == dataRows.Length)
                {
                    chkAll.IsChecked = true;
                }
                else
                {
                    chkAll.IsChecked = false;
                }
            }
        }
    }
}
