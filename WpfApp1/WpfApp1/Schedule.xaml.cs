using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";

        public string manage_instructor_name = "";
        public string manage_room_name = "";
        public string schedule_semester = "";
        public string schedule_school_year = "";
        string nl = System.Environment.NewLine;

        public Schedule()
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

            // Main

            Load_Sem();
            Load_Sy();

            // View

            ComboBox_Category();

            // Manage
            ComboBox_Manage_Instructors();
            ComboBox_Manage_Rooms();

            bdrSelect.Visibility = Visibility.Visible;
            tcSched.Visibility = Visibility.Collapsed;
        }

        // Main
        void Load_Sem()
        {
            cbSemester.Items.Add("FIRST");
            cbSemester.Items.Add("SECOND");
        }

        void Load_Sy()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                MySqlCommand cmd = new MySqlCommand("select * from tblschoolyr", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string reg = dr.GetString(1) + " - " + dr.GetString(2);
                        cbSy.Items.Add(reg);
                    }
                }
            }
        }

        private void CbSemester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSemester.SelectedIndex == -1)
            {

                tblSemester.Visibility = Visibility.Visible;
            }
            else
            {
                tblSemester.Visibility = Visibility.Collapsed;
            }

        }

        private void CbSy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSy.SelectedIndex == -1)
            {
                tblSy.Visibility = Visibility.Visible;
            }
            else
            {
                tblSy.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnProceed_Click(object sender, RoutedEventArgs e)
        {
            bdrSelect.Visibility = Visibility.Collapsed;
            tcSched.Visibility = Visibility.Visible;
            schedule_semester = cbSemester.SelectedValue.ToString();
            schedule_school_year = cbSy.SelectedValue.ToString();
        }
        
        
        // Navigation Buttons
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


        // vSchedule Start
        string day = "";
        Border bdr;

        void Border_Init()
        {
            bdr = new Border();
            bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            bdr.BorderThickness = new Thickness(1,2,1,2);
            bdr.Margin = new Thickness(0,-1,0,-1);
            bdr.Background = new SolidColorBrush(Colors.White);

        }

        void Dayyy()
        {
            if (day != "")
            {
                if (day == "MONDAY")
                {
                    grdvMONDAY.Children.Add(bdr);
                }
                else if (day == "TUESDAY")
                {
                    grdvTUESDAY.Children.Add(bdr);
                }
                else if (day == "WEDNESDAY")
                {
                    grdvWEDNESDAY.Children.Add(bdr);
                }
                else if (day == "THURSDAY")
                {
                    grdvTHURSDAY.Children.Add(bdr);
                }
                else if (day == "FRIDAY")
                {
                    grdvFRIDAY.Children.Add(bdr);
                }
                else if (day == "SATURDAY")
                {
                    grdvSATURDAY.Children.Add(bdr);
                }
            }
        }

        void ComboBox_Category()
        {
            cbType.Items.Add("Instructors");
            cbType.Items.Add("Rooms");
            cbType.Items.Add("Sections");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                MySqlCommand cmd = new MySqlCommand("select * from tbltemp", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        TextBlock txt = new TextBlock();
                        while (dr.Read())
                        {
                            string insname = dr.GetString(1);
                            string course = dr.GetString(2);
                            string ys = dr.GetString(3);
                            string Subj = dr.GetString(4);
                            string subt = "";
                            if(dr.GetString(5) != "")
                            {
                                subt = dr.GetString(5);
                            }
                            string stype = dr.GetString(6);
                            day = dr.GetString(7);
                            string bdrname = dr.GetString(8);
                            int row = dr.GetInt32(9);
                            int rowspan = dr.GetInt32(10);

                            //==================================//
                            bdrname = bdrname.Replace(":", "_");
                            bdrname = bdrname.Replace(" ", "_");
                            bdrname = "x" + bdrname;

                            Border_Init();
                            bdr.Name = bdrname;
                            Grid.SetRow(bdr, row);
                            Grid.SetRowSpan(bdr, rowspan);

                            txt.Text = insname + nl + course + " - " + ys;
                            txt.TextAlignment = TextAlignment.Center;
                            txt.VerticalAlignment = VerticalAlignment.Center;
                            bdr.Child = txt;
                            Dayyy();
                        }
                    }
                }
            }
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedIndex != -1)
            {
                if (cbType.SelectedValue.ToString() == "Instructors")
                {
                    bdrIns.Visibility = Visibility.Visible;
                    bdrRoom.Visibility = Visibility.Collapsed;
                    bdrSection.Visibility = Visibility.Collapsed;
                }
                else if (cbType.SelectedValue.ToString() == "Rooms")
                {
                    bdrIns.Visibility = Visibility.Collapsed;
                    bdrRoom.Visibility = Visibility.Visible;
                    bdrSection.Visibility = Visibility.Collapsed;
                }
                else if (cbType.SelectedValue.ToString() == "Sections")
                {
                    bdrIns.Visibility = Visibility.Collapsed;
                    bdrRoom.Visibility = Visibility.Collapsed;
                    bdrSection.Visibility = Visibility.Visible;
                }
                txtType.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtType.Visibility = Visibility.Visible;
            }
        }

        private void CbIns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIns.SelectedIndex != -1)
            {
                txtIns.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtIns.Visibility = Visibility.Visible;
            }
        }

        private void CbRm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRm.SelectedIndex != -1)
            {
                txtRoom.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtRoom.Visibility = Visibility.Visible;
            }
        }

        private void CbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCourse.SelectedIndex != -1)
            {
                txtCourse.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtCourse.Visibility = Visibility.Visible;
            }
        }

        private void CbYsec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbYsec.SelectedIndex != -1)
            {
                txtSection.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtSection.Visibility = Visibility.Visible;
            }
        }

        // mSchedule Start
        void ComboBox_Manage_Instructors()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblins", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbInstructor.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        void ComboBox_Manage_Rooms()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblroom", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbRoom.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        private void CbInstructor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                MySqlCommand cmd = new MySqlCommand("select * from tblins where I_ID='" + cbInstructor.SelectedValue.ToString() + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string lname = dr.GetString(2);
                        string fname = dr.GetString(3);
                        manage_instructor_name = fname + " " + lname;
                    }
                }
            }
            if (cbRoom.SelectedIndex == -1)
            {
                tbltwo.Text = manage_instructor_name;
                bdrOne.BorderThickness = new Thickness(0);
                bdrZero.BorderThickness = new Thickness(0);
            }
            else
            {
                bdrOne.BorderThickness = new Thickness(1);
                bdrZero.BorderThickness = new Thickness(1);
                tbltwo.Text = "";
                tblzero.Text = manage_instructor_name;
                tblone.Text = cbRoom.SelectedValue.ToString();
            }
        }

        private void CbRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbInstructor.SelectedIndex == -1)
            {
                tbltwo.Text = cbRoom.SelectedValue.ToString();
                bdrOne.BorderThickness = new Thickness(0);
                bdrZero.BorderThickness = new Thickness(0);
            }
            else
            {
                bdrOne.BorderThickness = new Thickness(1);
                bdrZero.BorderThickness = new Thickness(1);
                tbltwo.Text = "";
                tblzero.Text = manage_instructor_name;
                tblone.Text = cbRoom.SelectedValue.ToString();
            }
            manage_room_name = cbRoom.SelectedValue.ToString();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            mScheduleSubject UCobj = new mScheduleSubject();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    (window as Schedule).grdcontent.Children.Add(UCobj);
                }
            }
        }

        private void BtnX_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in cbhour1.Items)
            {
                string x = item.ToString();
                TimeSpan ts = DateTime.ParseExact(x, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                ts = ts + TimeSpan.FromHours(1);
                DateTime time1 = DateTime.Today.Add(ts);

                if (cbhour2.Items.Contains(time1.ToString("h:mm tt")))
                {

                }
                else
                {
                    if (cbhour1.Items.Contains(time1.ToString("h:mm tt")))
                    {

                    }
                    else
                    {
                        cbhour0.Items.Remove(x);
                    }
                }
            }
        }
    }
}
