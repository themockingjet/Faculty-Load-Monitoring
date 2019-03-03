using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Instructors.xaml
    /// </summary>
    public partial class Instructors : Window
    {
        DataTable dt;
        DataTable chkdata = new DataTable();
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";
        public string id_ins_update = string.Empty;
        string acc_id = string.Empty;
        string acc_name = string.Empty;
        public Instructors()
        {
            InitializeComponent();

            // Close and Minimize start
            this.DataContext = this;

            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            void btn_Min_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Min.Background = Brushes.Transparent;
            }

            void btn_Min_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Min.Background = new SolidColorBrush(Color.FromRgb(79, 83, 91));
            }

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);

            void btn_Close_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Close.Background = Brushes.Transparent;
            }

            void btn_Close_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Close.Background = new SolidColorBrush(Color.FromRgb(220, 20, 60));
            }
            // Close and Minimize end

            // Home Button
            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);

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

            // Instructor
            DataGrid_Instructor();

            this.DataContext = this;

            chkdata.Columns.Add("Ins_id", typeof(string));

            chkdata.RowChanged += new DataRowChangeEventHandler(chkdata_RowChanged);

            // Accounts
            DataGrid_Account();
        }

        //Close Button
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //Home Button
        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            Main win = new Main();
            win.Owner = this.Owner;
            win.Show();
            this.Close();
        }

        // VIEW INSTRUCTOR
        void DataGrid_Instructor()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("Select * from tblins", con);
            dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dt.Columns.Add("Address", typeof(string), "Ins_reg + ', ' + Ins_prov + ', ' + Ins_cit + ', ' + Ins_scit + ' ' + Ins_brgy");
            dtIns.DataContext = dt;
        }

        private void chkdata_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (chkdata.Rows.Count != 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DataRow adddata = chkdata.NewRow();
            var subject = dtIns.SelectedItem as DataRowView;
            adddata[0] = subject.Row["Ins_id"].ToString();
            chkdata.Rows.Add(adddata);
        }
        
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var subject = dtIns.SelectedItem as DataRowView;
            string sc = subject.Row["Ins_id"].ToString();

            for (int i = chkdata.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = chkdata.Rows[i];
                if (dr["Ins_id"].ToString() == sc)
                {
                    dr.Delete();
                }
            }
            chkdata.AcceptChanges();
        }

        private void DtIns_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "title":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "LName":
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "FName":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "MName":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Address":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Contact":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                default:
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void DtIns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtIns.SelectedIndex > -1)
            {
                id_ins_update = ((DataRowView)dtIns.SelectedItem).Row["Ins_id"].ToString();

                ins_id.Text = ((DataRowView)dtIns.SelectedItem).Row["Ins_id"].ToString();
                TitleT.Text = ((DataRowView)dtIns.SelectedItem).Row["title"].ToString();
                FnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["FName"].ToString();
                MnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["MName"].ToString();
                LnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["LName"].ToString();
                AddressT.Text = ((DataRowView)dtIns.SelectedItem).Row["Address"].ToString();
                ContactT.Text = ((DataRowView)dtIns.SelectedItem).Row["Ins_con"].ToString();
                btnUpdate.IsEnabled = true;
                btnUpdate.IsEnabled = true;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            mIns UCobj = new mIns();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Instructors))
                {
                    (window as Instructors).grdcontent.Children.Add(UCobj);
                }
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            mIns UCobj = new mIns();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Instructors))
                {
                    (window as Instructors).grdcontent.Children.Add(UCobj);
                }
            }
        }

        private void AddressT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddressT.LineCount != 1)
            {
                bdrStreet.Height = 60;
                tblCon.Margin = new Thickness(80, 200, 0, 0);
                bdrContact.Margin = new Thickness(80, 225, 0, 0);
            }
            else
            {
                bdrStreet.Height = 41;
                tblCon.Margin = new Thickness(80, 180, 0, 0);
                bdrContact.Margin = new Thickness(80, 200, 0, 0);
            }
        }

        // ACCOUNT INSTRUCTOR
        void DataGrid_Account()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("SELECT *, DATE_FORMAT(Last_log, '%W, %M %d, /%Y %h:%i %p') as Log_date FROM `tblaccounts` INNER JOIN tblins WHERE tblins.Ins_id = tblaccounts.User_info AND User_type <> 1", con);
            dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dt.Columns.Add("Name", typeof(string), "FName + ', ' + LName");
            dtAcc.DataContext = dt;
        }

        private void DtAcc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtAcc.SelectedIndex > -1)
            {
                acc_id = ((DataRowView)dtAcc.SelectedItem).Row["User_id"].ToString();
                acc_name = ((DataRowView)dtAcc.SelectedItem).Row["Name"].ToString();
                btnAccDelete.IsEnabled = true;
            }
            else
            {
                btnAccDelete.IsEnabled = false;
            }
        }

        private void BtnAccDelete_Click(object sender, RoutedEventArgs e)
        {
            if (acc_id != string.Empty)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + acc_name + "'s Account?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    MySqlCommand cmd = new MySqlCommand("DELETE  from [course] where User_id ='" + acc_id + "')", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show(acc_name + "'s Accoun has been delete successfully.");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    //code for Cancel
                }
            }
        }
    }
}
