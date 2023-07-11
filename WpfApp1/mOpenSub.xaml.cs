using MySql.Data.MySqlClient;
using System;
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
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        DataTable dt;
        string syid = Main.semester_id;
        string semid = Main.schoolyear_id;

        string scode = string.Empty;
        string sdesc = string.Empty;
        string sem_i = string.Empty;
        string sid = string.Empty;

        public mOpenSub()
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
            MySqlCommand cmdSel = new MySqlCommand("SELECT * FROM tblsubject LEFT JOIN tblcourse USING (Course_id) WHERE tblsubject.Sub_id NOT IN (SELECT tblopensub.Sub_id FROM tblopensub WHERE tblopensub.Sem_id='" + Schoolyear.cursemid + "' AND tblopensub.Sy_id='" + Schoolyear.cursyid +"') ORDER BY Year,Semester,Course_name", con);
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
            SearchW.Visibility = Visibility.Visible;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(OpenSub))
                {
                    (window as OpenSub).grdcontent.Children.Remove(this);
                    (window as OpenSub).bdrContent.IsEnabled = true;
                }
            }
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            string xx = string.Empty;
            string xid = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    x += 1;
                    xx += row["Sub_code"].ToString() + " - " + row["Sub_desc"].ToString() + "\n";
                    xid = row["Sub_id"].ToString();
                }
            }
            MySqlConnection con = new MySqlConnection(Connection);
            if (x > 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to open these subjects?\n\n " + xx, "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((bool)row["Check"] == true)
                        {
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblopensub`(`Sub_id`, `Sem_id`, `Course_id`, `Sy_id`, `Petition`) VALUES (@scode,@sem,@crsid,@sy,@pet)", con);
                            cmd.Parameters.AddWithValue("@scode", row["Sub_id"].ToString());
                            cmd.Parameters.AddWithValue("@sem", semid);
                            cmd.Parameters.AddWithValue("@crsid", row["Course_id"].ToString());
                            cmd.Parameters.AddWithValue("@sy", syid);
                            if (row["Semester"].ToString() != Main.main_semester)
                            {
                                cmd.Parameters.AddWithValue("@pet", "PETITION");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@pet", "");
                            }
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    Load_Grid();
                    MessageBox.Show(xx + "\n\nAre now open.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(OpenSub))
                        {
                            (window as OpenSub).grdcontent.Children.Remove(this);
                            (window as OpenSub).Load_Grid();
                            (window as OpenSub).bdrContent.IsEnabled = true;
                        }
                    }
                }

            }
            else if (x == 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to open these subjects?\n\n " + xx, "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var chk = row["Check"];
                        if ((bool)chk != false)
                        {
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblopensub`(`Sub_id`, `Sem_id`, `Course_id`, `Sy_id`, `Petition`) VALUES (@scode,@sem,@crsid,@sy,@pet)", con);
                            cmd.Parameters.AddWithValue("@scode", row["Sub_id"].ToString());
                            cmd.Parameters.AddWithValue("@sem", semid);
                            cmd.Parameters.AddWithValue("@crsid", row["Course_id"].ToString());
                            cmd.Parameters.AddWithValue("@sy", syid);
                            if (row["Semester"].ToString() != Main.main_semester)
                            {
                                cmd.Parameters.AddWithValue("@pet", "PETITION");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@pet", "");
                            }
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    Load_Grid();
                    MessageBox.Show(xx + "\n\nAre now open.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(OpenSub))
                        {
                            (window as OpenSub).grdcontent.Children.Remove(this);
                            (window as OpenSub).Load_Grid();
                            (window as OpenSub).bdrContent.IsEnabled = true;
                        }
                    }
                }
            }
            else if (x == 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to open this subject?\n\n " + scode + " - " + sdesc + " ?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblopensub`(`Sub_id`, `Sem_id`, `Course_id`, `Sy_id`, `Petition`) VALUES (@scode,@sem,@crsid,@sy,@pet)", con);
                    cmd.Parameters.AddWithValue("@scode", sid);
                    cmd.Parameters.AddWithValue("@sem", semid);
                    cmd.Parameters.AddWithValue("@sy", syid);
                    if (sem_i != Main.main_semester)
                    {
                        cmd.Parameters.AddWithValue("@pet", "PETITION");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@pet", "");
                    }
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show(scode + " - " + sdesc + "\n\nis now open.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(OpenSub))
                        {
                            (window as OpenSub).grdcontent.Children.Remove(this);
                            (window as OpenSub).Load_Grid();
                            (window as OpenSub).bdrContent.IsEnabled = true;
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

                            sid = row["Sub_id"].ToString();
                            scode = row["Sub_code"].ToString();
                            sdesc = row["Sub_desc"].ToString();
                            sem_i = row["Semester"].ToString();
                            btnOpen.IsEnabled = true;
                        }
                    }
                }
                else if (x == 0)
                {
                    sid = ((DataRowView)dtMopen.SelectedItem).Row["Sub_id"].ToString();
                    scode = ((DataRowView)dtMopen.SelectedItem).Row["Sub_code"].ToString();
                    sdesc = ((DataRowView)dtMopen.SelectedItem).Row["Sub_desc"].ToString();
                    sem_i = ((DataRowView)dtMopen.SelectedItem).Row["Semester"].ToString();
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

        private void S_codeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("Sub_code LIKE '{0}%' OR Sub_desc LIKE '%{0}%' OR Course_name LIKE '%{0}%' OR Year LIKE '%{0}%' OR Semester LIKE '%{0}%'", S_codeT.Text);
        }
    }
}
