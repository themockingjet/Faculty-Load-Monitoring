using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for vIns.xaml
    /// </summary>
    public partial class vIns : UserControl
    {
        DataTable dt;
        DataTable chkdata = new DataTable();
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";

        public vIns()
        {
            InitializeComponent();

           
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdSel = new MySqlCommand("Select * from tblins", con);
            dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dt.Columns.Add("Address", typeof(string), "Ins_reg + ', ' + Ins_prov + ', ' + Ins_cit + ', ' + Ins_scit + ' ' + Ins_brgy");
            dtIns.DataContext = dt;

            this.DataContext = this;

            chkdata.Columns.Add("Ins_id", typeof(string));

            chkdata.RowChanged += new DataRowChangeEventHandler(chkdata_RowChanged);

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
        // DataGrid CheckBox Check
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
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Instructors))
                    {
                        (window as Instructors).id_ins_update = ((DataRowView)dtIns.SelectedItem).Row["Ins_id"].ToString();
                    }
                }
                ins_id.Text = ((DataRowView)dtIns.SelectedItem).Row["Ins_id"].ToString();
                TitleT.Text = ((DataRowView)dtIns.SelectedItem).Row["title"].ToString();
                FnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["FName"].ToString();
                MnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["MName"].ToString();    
                LnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["LName"].ToString();
                AddressT.Text = ((DataRowView)dtIns.SelectedItem).Row["Address"].ToString();
                ContactT.Text = ((DataRowView)dtIns.SelectedItem).Row["Ins_con"].ToString();
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
                tblCon.Margin = new Thickness(50, 200, 0, 0);
                bdrContact.Margin = new Thickness(50, 225, 0, 0);
            }
            else
            {
                bdrStreet.Height = 41;
                tblCon.Margin = new Thickness(50, 180, 0, 0);
                bdrContact.Margin = new Thickness(50, 200, 0, 0);
            }
        }
    }
}
