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
using JR.Utils.GUI.Forms;
using System.Text;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Instructors.xaml
    /// </summary>
    public partial class Instructors : Window
    {
        private static string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        
        public string id_ins_update = string.Empty;
        string ins_name = string.Empty;
        string acc_id = string.Empty;
        string acc_name = string.Empty;
        
        DataTable dtI;
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
            Fill_DataGrid_Instructor();

            txtCuraysem.Text = "ACADEMIC YEAR: " + Main.main_schoolyear + "   SEMESTER: " + Main.main_semester;
        }
        // Navigation

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

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // VIEW INSTRUCTOR
        void Clear_Controls()
        {
            TitleT.Text = "";
            FnameT.Text = "";
            MnameT.Text = "";
            LnameT.Text = "";
            AddressT.Text = "";
            ContactT.Text = "";
        }

        public void Fill_DataGrid_Instructor()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmdIns = new MySqlCommand("Select * from tblins order by LName", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmdIns);
            dtI = new DataTable();
            da.Fill(dtI);
            dtI.Columns.Add("Address", typeof(string), "Ins_reg + ', ' + Ins_prov + ', ' + Ins_cit + ', ' + Ins_scit + ' ' + Ins_brgy");
            dtIns.ItemsSource = dtI.DefaultView;
        }

        private void DtIns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtIns.SelectedIndex != -1)
            {
                id_ins_update = ((DataRowView)dtIns.SelectedItem).Row["Ins_id"].ToString();
                ins_name = ((DataRowView)dtIns.SelectedItem).Row["FName"].ToString() + " " + ((DataRowView)dtIns.SelectedItem).Row["LName"].ToString();

                ins_id.Text = ((DataRowView)dtIns.SelectedItem).Row["Ins_id"].ToString();
                TitleT.Text = ((DataRowView)dtIns.SelectedItem).Row["title"].ToString();
                FnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["FName"].ToString();
                MnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["MName"].ToString();
                LnameT.Text = ((DataRowView)dtIns.SelectedItem).Row["LName"].ToString();
                AddressT.Text = ((DataRowView)dtIns.SelectedItem).Row["Address"].ToString();
                ContactT.Text = ((DataRowView)dtIns.SelectedItem).Row["Ins_con"].ToString();
                btnUpdate.IsEnabled = true;
            }
            else
            {
                id_ins_update = string.Empty;
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
                    bdrContent.IsEnabled = false;
                }
            }
            Clear_Controls();
            btnUpdate.IsEnabled = false;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            id_ins_update = string.Empty;
            mIns UCobj = new mIns();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Instructors))
                {
                    (window as Instructors).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            Clear_Controls();
            btnUpdate.IsEnabled = false;
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
        
        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearchW.Visibility = Visibility.Collapsed;
            if (txtSearch.Text.Length != 0)
            {
                txtSearch.CaretIndex = txtSearch.Text.Length;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            dtI.DefaultView.RowFilter = string.Format("LName LIKE '{0}%' OR FName LIKE '{0}%'", txtSearch.Text);
        }

        private void TxtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            string x = txtSearch.Text;
            string xx = x.Replace("\\s", "");
            if (xx.Length == 0)
            {
                txtSearchW.Visibility = Visibility.Visible;
            }
        }
    }
}
