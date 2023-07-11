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
using JR.Utils.GUI.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Course.xaml
    /// </summary>
    public partial class Course : Window
    {
        private static string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        DataTable dt;

        public string courseid = string.Empty;
        public string ccode = string.Empty;
        string cdesc = string.Empty;

        public Course()
        {
            InitializeComponent();


            // Close and Minimize start
            this.DataContext = this;
            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);

            btn_Home.MouseEnter += new MouseEventHandler(btn_Home_MouseEnter);
            btn_Home.MouseLeave += new MouseEventHandler(btn_Home_MouseLeave);

            // Content
            Fill_DataGrid_Course();

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
        

        // =================================== MAIN ==================================

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
                btnManageSec.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                CcodeT.Text = string.Empty;
                CdescT.Text = string.Empty;
            }
            else if (x == 1)
            {
                btnUpdate.IsEnabled = true;
                btnManageSec.IsEnabled = true;
            }
            else if (x == 0)
            {
                Clear_Controls();
                dtCourse.SelectedIndex = -1;
            }

            if (x != xx)
            {
                chkAll.IsChecked = false;
            }
            else
            {
                chkAll.IsChecked = true;
            }
        }

        public void Fill_DataGrid_Course()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblcourse ORDER BY Course_name", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            var col = new DataColumn("Check", typeof(bool));
            col.DefaultValue = false;
            dt.Columns.Add(col);
            dtCourse.ItemsSource = dt.DefaultView;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dtCourse.SelectedItem != null)
            {
                var subject = dtCourse.SelectedItem as DataRowView;
                subject.Row["Check"] = true;
            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtCourse.SelectedItem != null)
            {
                var subject = dtCourse.SelectedItem as DataRowView;
                subject.Row["Check"] = false;
            }


        }

        private void ChkAll_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["Check"] = true;
                }
            }
            else if (checkBox.IsChecked == false)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["Check"] = false;
                }
            }
        }

        void Clear_Controls()
        {
            CcodeT.Text = "";
            CdescT.Text = "";
            CyearT.Text = "";
        }
        
        private void CdescT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CdescT.LineCount == 2)
            {
                bdrCdesc.Height = 60;
                tbYr.Margin = new Thickness(50, 230, 0, 0);
                bdrYr.Margin = new Thickness(50, 255, 0, 0);
                btnUpdate.Margin = new Thickness(50, 0, 12, 60);
                btnOpen.Margin = new Thickness(50, 0, 12, 120);
            }
            else if (CdescT.LineCount == 3)
            {
                bdrCdesc.Height = 80;
                tbYr.Margin = new Thickness(50, 250, 0, 0);
                bdrYr.Margin = new Thickness(50, 275, 0, 0);
                btnUpdate.Margin = new Thickness(50, 0, 12, 70);
                btnOpen.Margin = new Thickness(50, 0, 12, 130);
            }
            else
            {
                bdrCdesc.Height = 40;
            }
        }

        private void DtCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtCourse.SelectedIndex != -1)
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
                    btnUpdate.IsEnabled = false;
                }
                else if (x == 1)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((bool)row["Check"] == true)
                        {
                            btnUpdate.IsEnabled = true;
                            btnManageSec.IsEnabled = true;
                            courseid = row["Course_id"].ToString();

                            CcodeT.Text = row["Course_name"].ToString();
                            CdescT.Text = row["Course_desc"].ToString();
                            CyearT.Text = row["Max_yr"].ToString();
                            ccode = CcodeT.Text;
                            cdesc = CdescT.Text;
                        }
                    }
                    
                }
                else if (x == 0)
                {
                    btnUpdate.IsEnabled = true;
                    btnManageSec.IsEnabled = true;
                    courseid = ((DataRowView)dtCourse.SelectedItem).Row["Course_id"].ToString();

                    CcodeT.Text = ((DataRowView)dtCourse.SelectedItem).Row["Course_name"].ToString();
                    CdescT.Text = ((DataRowView)dtCourse.SelectedItem).Row["Course_desc"].ToString();
                    CyearT.Text = ((DataRowView)dtCourse.SelectedItem).Row["Max_yr"].ToString();
                    ccode = CcodeT.Text;
                    cdesc = CdescT.Text;
                }
            }
            else
            {
                btnUpdate.IsEnabled = false;
            }
        }
        
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            mCourse UCobj = new mCourse();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Course))
                {
                    (window as Course).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            Clear_Controls();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            courseid = string.Empty;
            mCourse UCobj = new mCourse();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Course))
                {
                    (window as Course).grdcontent.Children.Add(UCobj);
                    bdrContent.IsEnabled = false;
                }
            }
            Clear_Controls();
        }

        private void BtnManageSec_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            string xx = string.Empty;
            string xid = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    x += 1;
                    xx += row["Course"];
                    xid = row["Id"].ToString();
                }
            }
            if (x == 1)
            {
                mSection UCobj = new mSection();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Course))
                    {
                        (window as Course).grdcontent.Children.Add(UCobj);
                        ccode = xx;
                        bdrContent.IsEnabled = false;
                    }
                }
                Clear_Controls();
            }
            else if (x == 0)
            {
                mSection UCobj = new mSection();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Course))
                    {
                        (window as Course).grdcontent.Children.Add(UCobj);
                        bdrContent.IsEnabled = false;
                    }
                }
                Clear_Controls();
            }
            dtCourse.SelectedIndex = -1;
        }
    }
}
