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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for OpenSub.xaml
    /// </summary>
    public partial class OpenSub : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";

        public string get_sem = Main.semester_id;
        public string get_sy = Main.schoolyear_id;

        string scode = string.Empty;
        string sdesc = string.Empty;

        DataTable dt;
        public OpenSub()
        {
            InitializeComponent();
            
            // Close and Minimize start
            this.DataContext = this;
            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);
            

            Load_Grid();
            dt.RowChanged += new DataRowChangeEventHandler(dt_RowChanged);

            txtCuraysem.Text = "ACADEMIC YEAR: " + Main.main_schoolyear + "   SEMESTER: " + Main.main_semester;
        }

        void btn_Min_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_Min.Background = Brushes.Transparent;
        }

        void btn_Min_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_Min.Background = new SolidColorBrush(Color.FromRgb(79, 83, 91));
        }


        void btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_Close.Background = Brushes.Transparent;
        }

        void btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_Close.Background = new SolidColorBrush(Color.FromRgb(220, 20, 60));
        }


        void btn_Home_MouseLeave(object sender, MouseEventArgs e)
        {
            tblHome.TextDecorations = null;
            imgHome.Width = 50;
            imgHome.Height = 50;
            tblHome.FontSize = 14;
        }

        void btn_Home_MouseEnter(object sender, MouseEventArgs e)
        {
            imgHome.Width = 60;
            imgHome.Height = 60;
            tblHome.TextDecorations = TextDecorations.Underline;
            tblHome.FontSize = 16;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            Main win = new Main();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // ==================== MAIN ============================

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
            if (x > 1)
            {
                btnCloseSub.IsEnabled = true;
            }
            else if (x == 1)
            {
                btnCloseSub.IsEnabled = true;
            }
            else if (x == 0)
            {
                btnCloseSub.IsEnabled = true;
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

        public void Load_Grid()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("SELECT osub.Osub_id, sub.Sub_code, sub.Sub_desc, sub.T_units, sem.Sem_name, crs.Course_name, osub.Petition, sy.Sy_id FROM `tblopensub` osub LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblsem sem USING (Sem_id) JOIN tblcourse crs JOIN tblsy sy ON sy.Sy_id = osub.Sy_id AND crs.Course_id = osub.Course_id WHERE osub.Sy_id = '" + get_sy + "'AND osub.Sem_id = '" + get_sem + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            dt = new DataTable();
            da.Fill(dt);
            var col = new DataColumn("Check", typeof(bool));
            col.DefaultValue = false;
            dt.Columns.Add(col);
            dtOpenSub.DataContext = dt;
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            mOpenSub UCobj = new mOpenSub();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(OpenSub))
                {
                    (window as OpenSub).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            dtOpenSub.SelectedIndex = -1;
            chkAll.IsChecked = false;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    row["Check"] = false;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dtOpenSub.SelectedItem != null)
            {
                var subject = dtOpenSub.SelectedItem as DataRowView;
                subject.Row["Check"] = true;
            }
        }
        
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtOpenSub.SelectedItem != null)
            {
                var subject = dtOpenSub.SelectedItem as DataRowView;
                subject.Row["Check"] = false;
            }
        }

        private void BtnCloseSub_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            string xx = string.Empty;
            string xid = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                var chk = row["Check"];
                if ((bool)chk != false)
                {
                    x += 1;
                    xx += row["Sub_code"].ToString() + " - " + row["Sub_desc"].ToString() + "\n";
                    xid = row["Osub_id"].ToString();
                }
            }
            MySqlConnection con = new MySqlConnection(Connection);
            if (x > 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close these subjects?\n\n " + xx , "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var chk = row["Check"];
                        if ((bool)chk != false)
                        {
                            MySqlCommand cmd = new MySqlCommand("DELETE FROM `tblopensub` WHERE Osub_id='" + row["Osub_id"].ToString() + "'", con);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    Load_Grid();
                    MessageBox.Show("Successfully closed.");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    //code for Cancel
                }

            }
            else if (x == 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close this subject?\n\n " + xx, "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM `tblopensub` WHERE Osub_id='" + xid + "'", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Load_Grid();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    //code for Cancel
                }
            }
            else if (x == 0)
            {
                if (selsid != string.Empty)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to close this subject?\n\n " + scode + " - " + sdesc, "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM `tblopensub` WHERE Osub_id='" + selsid + "'", con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Load_Grid();
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        //code for Cancel
                    }
                }
            }
        }

        string selsid = string.Empty;

        private void DtOpenSub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtOpenSub.SelectedIndex > -1)
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
                            selsid = row["Osub_id"].ToString();
                            scode = row["Sub_code"].ToString();
                            sdesc = row["Sub_desc"].ToString();
                            btnCloseSub.IsEnabled = true;
                        }
                    }
                }
                else if (x == 0)
                {
                    selsid = ((DataRowView)dtOpenSub.SelectedItem).Row["Osub_id"].ToString();
                    scode = ((DataRowView)dtOpenSub.SelectedItem).Row["Sub_code"].ToString();
                    sdesc = ((DataRowView)dtOpenSub.SelectedItem).Row["Sub_desc"].ToString();
                    btnCloseSub.IsEnabled = true;
                }
            }
            else
            {
                selsid = string.Empty;
                btnCloseSub.IsEnabled = false;
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

        private void S_codeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.DefaultView.RowFilter = filter;
        }

        private string filter
        {
            get
            {
                string fil = string.Format("Sub_code LIKE '{0}%' OR Sub_desc LIKE '%{0}%' OR Course_name LIKE '%{0}%' OR Petition LIKE '%{0}%'", S_codeT.Text);
                return fil;
            }
        }
    }
}
