using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using JR.Utils.GUI.Forms;
using System.Text;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Sub_List.xaml
    /// </summary>
    public partial class Sub_List : Window
    {
        DataTable chkdata = new DataTable();
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        public string sub_id = string.Empty;
        public string sub_code = string.Empty;
        public string sub_desc = string.Empty;

        DataTable dt;
        public Sub_List()
        {
            InitializeComponent();

            Fill_DataGrid_Subject();

            // Content
            this.DataContext = this;

            // Close and Minimize start
            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);

            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);

            txtCuraysem.Text = "ACADEMIC YEAR: " + Main.main_schoolyear + "   SEMESTER: " + Main.main_semester;
        }

        #region MAIN MENUBAR
        private void Logout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow win = new MainWindow();
                win.Owner = this.Owner;
                win.Show();
                this.Close();
            }
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
        #endregion

        // ========================= MAIN ===================================
        
        void Clear_Controls()
        {
            S_codeT.Text = "";
            S_descT.Text = "";
            CourseT.Text = "";
            YearT.Text = "";
            TunitsT.Text = "";
            CUlabT.Text = "";
            CUlecT.Text = "";
            CHlabT.Text = "";
            CHlecT.Text = "";
            ChoursT.Text = "";
            SemT.Text = "";
            dtSub.SelectedIndex = -1;
        }
        
        public void Fill_DataGrid_Subject()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `tblsubject` t1 LEFT JOIN `tblcourse` t2 USING (Course_id) WHERE t1.Course_id = t2.Course_id ORDER BY Year,Semester,Course_id", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dtSub.ItemsSource = dt.DefaultView;
        }

        private void DtSub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtSub.SelectedIndex > -1)
            {
                sub_id = ((DataRowView)dtSub.SelectedItem).Row["Sub_id"].ToString();
                sub_code = ((DataRowView)dtSub.SelectedItem).Row["Sub_code"].ToString();
                sub_desc = ((DataRowView)dtSub.SelectedItem).Row["Sub_desc"].ToString();
                int x1 = 0;
                int y = 0;
                S_codeT.Text = ((DataRowView)dtSub.SelectedItem).Row["Sub_code"].ToString();
                S_descT.Text = ((DataRowView)dtSub.SelectedItem).Row["Sub_desc"].ToString();
                TunitsT.Text = ((DataRowView)dtSub.SelectedItem).Row["T_units"].ToString();
                y = Int32.Parse(TunitsT.Text);
                if (y != 1)
                {
                    TunitsT.Text = y + " Units";
                }
                else
                {
                    TunitsT.Text = y + " Unit";
                }

                CUlecT.Text = ((DataRowView)dtSub.SelectedItem).Row["Cu_lec"].ToString();
                CUlabT.Text = ((DataRowView)dtSub.SelectedItem).Row["Cu_lab"].ToString();
                CHlecT.Text = ((DataRowView)dtSub.SelectedItem).Row["Ch_lec"].ToString();
                CHlabT.Text = ((DataRowView)dtSub.SelectedItem).Row["Ch_lab"].ToString();
                x1 = Int32.Parse(CHlecT.Text) + Int32.Parse(CHlabT.Text);
                if (x1 != 1)
                {
                    ChoursT.Text = x1 + " Hours";
                }
                else
                {
                    ChoursT.Text = x1 + " Hour";
                }
                YearT.Text = ((DataRowView)dtSub.SelectedItem).Row["Year"].ToString();
                CourseT.Text = ((DataRowView)dtSub.SelectedItem).Row["Course_name"].ToString();
                SemT.Text = ((DataRowView)dtSub.SelectedItem).Row["Semester"].ToString();
                btnUpdate.IsEnabled = true;
            }
            else
            {
                btnUpdate.IsEnabled = false;
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {   
            sub_id = string.Empty;
            mSubL UCobj = new mSubL();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Sub_List))
                {
                    (window as Sub_List).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            Clear_Controls();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            mSubL UCobj = new mSubL();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Sub_List))
                {
                    (window as Sub_List).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            Clear_Controls();
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
            dt.DefaultView.RowFilter = string.Format("Sub_code LIKE '{0}%' OR Sub_desc LIKE '%{0}%' OR Course_name LIKE '%{0}%'  OR Year LIKE '%{0}%' OR Semester LIKE '%{0}%'", txtSearch.Text);
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
        //END
    }
}
