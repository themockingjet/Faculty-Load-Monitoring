using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mS.xaml
    /// </summary>
    public partial class mS : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        string nl = System.Environment.NewLine;
        DataTable dtcklst;
        DataTable exsubj;
        DataTable exother;

        MySqlCommand cmd;
        MySqlDataReader reader;
        MySqlDataAdapter da;

        public string semester = string.Empty;
        public string schoolyear = string.Empty;

        public mS()
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

            // Main
            schoolyear = Main.schoolyear_id;
            semester = Main.semester_id;

            ComboBox_Manage_Instructors();
            ComboBox_Manage_Rooms();
            ComboBox_Manage_Section();
            ComboBox_Day();

            Scrollbar_min();
            ComboBox_Manage_Stime();
            Load_Footer();

            txtCuraysem.Text = "ACADEMIC YEAR: " + Main.main_schoolyear + "   SEMESTER: " + Main.main_semester;
        }


        #region Main Panel Buttons

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

        // ================================== Main ================================

        string ldday = string.Empty;
        Border bdr;
        ContextMenu context;
        TextBlock txt;
        string comcourse = string.Empty;
        string year = string.Empty;
        string pet = string.Empty;

        void contextmenu()
        {
            context = new ContextMenu();

            MenuItem delete = new MenuItem();
            delete.Header = "Remove";
            delete.Click += ContextMenuDelete_Click;

            context.Items.Add(delete);
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            var bdr = sender as Border;
            if (mnu != null)
            {
                bdr = ((ContextMenu)mnu.Parent).PlacementTarget as Border;
                MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this schedule?.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection))
                    {
                        cmd = new MySqlCommand("DELETE FROM tblsched WHERE Sched_id='" + bdr.Tag + "'", con);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        con.Close();
                        MessageBox.Show("Schedule has been removed successfully.");
                        Fill_Sched();
                        Reset_control();
                    }
                }
            }
        }

        public void Get_Time()
        {
            foreach (UIElement ele in grdSchedule.Children)
            {
                if (ele.GetType() == typeof(Border))
                {
                    //StackPanel pnl = null;
                    Border bdrx = (Border)ele;
                    int c = Grid.GetColumn(bdrx);
                    int r = Grid.GetRow(bdrx);
                    int rs = Grid.GetRowSpan(bdrx);

                    string x = bdrx.Name.Substring(1);
                    TimeSpan ts = DateTime.ParseExact(x, "h_mm_tt", CultureInfo.InvariantCulture).TimeOfDay;
                    DateTime time = DateTime.Today.Add(ts);
                    cbhour2.Items.Add(time.ToString("h:mm tt"));
                    for (int i = 0; i <= (rs - 1); i++)
                    {
                        DateTime time1 = DateTime.Today.Add(ts);
                        cbhour.Items.Remove(time1.ToString("h:mm tt"));
                        cbhour1.Items.Remove(time1.ToString("h:mm tt"));

                        ts = ts + TimeSpan.FromMinutes(30);
                    }
                }
            }
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
                        cbhour.Items.Remove(x);
                    }
                }
            }
        }

        // =============== COMBOBOX ITEMS =============

        void ComboBox_Manage_Instructors()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(LName,', ',FName) as Ins_name, Ins_id FROM tblopenins LEFT JOIN tblins USING (Ins_id) WHERE Sem_id ='" + Main.semester_id + "' AND Sy_id ='" + Main.schoolyear_id + "' ORDER BY LName", con))
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
                using (MySqlCommand cmd = new MySqlCommand("SELECT Room_id,Room_name FROM `tblopenrooms` LEFT JOIN `tblroom` USING (Room_id) WHERE Sem_id ='" + Main.semester_id + "' AND Sy_id ='" + Main.schoolyear_id + "'", con))
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

        void ComboBox_Manage_Section()
        {
            cbYrSection.ItemsSource = null;
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(Course_id,'/',Sec_id) as Sec_id,CONCAT(Course_name,' - ',CONCAT(IF(Sec_year = '0','',Sec_year),Sec_name)) as Section FROM tblopensec LEFT JOIN tblsection USING (Sec_id) LEFT JOIN tblcourse USING (Course_id) WHERE Sem_id ='" + Main.semester_id + "' AND Sy_id ='" + Main.schoolyear_id + "'ORDER BY Sec_year,Sec_name", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbYrSection.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        private string SelSecYear
        {
            get
            {
                var selitem = cbYrSection.SelectedItem as DataRowView;
                string secname = selitem.Row["Section"].ToString();
                string[] secyear = secname.Split('-', ' ');
                return secyear[3];
            }
        }

        private string CourseId
        {
            get
            {
                var selitem = cbYrSection.SelectedItem as DataRowView;
                string secname = selitem.Row["Sec_id"].ToString();
                string[] secyear = secname.Split('/');
                return secyear[0];
            }
        }

        private string SecId
        {
            get
            {
                var selitem = cbYrSection.SelectedItem as DataRowView;
                string secname = selitem.Row["Sec_id"].ToString();
                string[] secyear = secname.Split('/');
                return secyear[1];
            }
        }

        void ComboBox_Manage_Subjects()
        {
            if (cbYrSection.SelectedIndex != -1)
            {
                Fill_Checklist();
                if (pet != string.Empty)
                {
                    if (dtcklst != null)
                    {
                        DataTable dt = new DataTable();
                        dt = dtcklst.Copy();
                        dt.DefaultView.RowFilter = "Check <> TRUE";
                        cbSubject.ItemsSource = dt.DefaultView;
                    }
                }
                else
                {
                    if (dtcklst != null)
                    {
                        DataTable dt = new DataTable();
                        dt = dtcklst.Copy();
                        dt.DefaultView.RowFilter = "Check <> TRUE";
                        cbSubject.ItemsSource = dt.DefaultView;
                    }
                }
                cbSubject.IsEnabled = true;
            }
        }

        void ComboBox_Manage_SubType()
        {
            cbmType.Items.Clear();
            cbmType.Items.Add("LEC");
            cbmType.Items.Add("LAB");

            if (dtcklst != null)
            {
                foreach (DataRow row in dtcklst.Rows)
                {
                    if (row["Sub_id"].ToString() == cbSubject.SelectedValue.ToString())
                    {
                        if (row["Ch_lec"].ToString() == "0" && row["Ch_lab"].ToString() != "0")
                        {
                            cbmType.IsHitTestVisible = false;
                            cbmType.SelectedIndex = 1;
                        }
                        else if (row["Ch_lec"].ToString() != "0" && row["Ch_lab"].ToString() == "0")
                        {
                            cbmType.SelectedIndex = 0;
                            cbmType.IsHitTestVisible = false;
                        }
                    }
                }
            }
        }

        void ComboBox_Day()
        {
            cbDay.Items.Add("MONDAY");
            cbDay.Items.Add("TUESDAY");
            cbDay.Items.Add("WEDNESDAY");
            cbDay.Items.Add("THURSDAY");
            cbDay.Items.Add("FRIDAY");
            cbDay.Items.Add("SATURDAY");
        }

        string chlec = string.Empty;
        string chlab = string.Empty;
        string othermaxtime = string.Empty;

        void Scrollbar_hour()
        {
            scrollhour.Minimum = 0;
            scrollhour.SmallChange = 1;
            grdDur.IsEnabled = true;
            if (chkOther.IsChecked == true)
            {
                grdMin.IsEnabled = false;
                if (cbOther.SelectedIndex != -1)
                {
                    if (dtcklst != null)
                    {
                        foreach (DataRow row in dtcklst.Rows)
                        {
                            if (cbOther.SelectedValue.ToString() == "CONSULTATION HOUR" && row["Other"].ToString() == "CONSULTATION HOUR")
                            {
                                othermaxtime = row["THours"].ToString();
                                if (double.Parse(row["UHours"].ToString()) == double.Parse(row["THours"].ToString()) / 2)
                                {
                                    grdDur.IsEnabled = false;
                                    scrollhour.Value = double.Parse(othermaxtime) / 2;
                                }
                                else if (double.Parse(row["UHours"].ToString()) == 0)
                                {
                                    scrollhour.Maximum = 2;
                                    grdMin.IsEnabled = false;
                                }
                            }
                            if (cbOther.SelectedValue.ToString() == "RESEARCH" && row["Other"].ToString() == "RESEARCH")
                            {
                                othermaxtime = row["THours"].ToString();
                                if (double.Parse(row["UHours"].ToString()) == double.Parse(row["THours"].ToString()) / 2)
                                {
                                    grdDur.IsEnabled = false;
                                    double mod = double.Parse(othermaxtime) % 2;
                                    if (mod == 1)
                                    {
                                        grdDur.IsEnabled = false;
                                        scrollhour.Value = int.Parse(othermaxtime) / 2;
                                        scrollmin.Value = 30;
                                    }
                                    else if (mod == 0)
                                    {
                                        grdDur.IsEnabled = false;
                                        scrollhour.Value = int.Parse(othermaxtime) / 2;
                                    }
                                }
                                else if (double.Parse(row["UHours"].ToString()) == 0)
                                {
                                    grdMin.IsEnabled = true;
                                    scrollhour.Maximum = 3;
                                }
                            }
                            if (cbOther.SelectedValue.ToString() == "EXTENSION" && row["Other"].ToString() == "EXTENSION")
                            {
                                othermaxtime = row["THours"].ToString();
                                if (double.Parse(row["UHours"].ToString()) == double.Parse(row["THours"].ToString()) / 2)
                                {
                                    grdDur.IsEnabled = false;
                                    double mod = double.Parse(othermaxtime) % 2;
                                    if (mod == 1)
                                    {
                                        grdDur.IsEnabled = false;
                                        scrollhour.Value = int.Parse(othermaxtime) / 2;
                                        scrollmin.Value = 30;
                                    }
                                    else if (mod == 0)
                                    {
                                        grdDur.IsEnabled = false;
                                        scrollhour.Value = int.Parse(othermaxtime) / 2;
                                    }
                                }
                                else if (double.Parse(row["UHours"].ToString()) == 0)
                                {
                                    grdMin.IsEnabled = true;
                                    scrollhour.Maximum = 3;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (dtcklst != null)
                {
                    if (cbmType.SelectedValue.ToString() == "LEC")
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection))
                        {
                            con.Open();
                            cmd = new MySqlCommand("SELECT * FROM `tblsubject` t1 LEFT JOIN `tblcourse` t2 USING (Course_id) WHERE t1.Course_id = t2.Course_id AND Sub_id = '" + cbSubject.SelectedValue.ToString() + "'", con);
                            using (reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        chlec = reader[6].ToString();
                                        foreach (DataRow row in dtcklst.Rows)
                                        {
                                            if (row["Sub_id"].ToString() == cbSubject.SelectedValue.ToString())
                                            {
                                                if (row["Ch_lec"].ToString() == chlec)
                                                {
                                                    scrollhour.Maximum = double.Parse(chlec);
                                                }
                                                else if (row["Ch_lec"].ToString() == (double.Parse(chlec) / 2).ToString() && row["Ch_lec"].ToString() != chlec)
                                                {
                                                    double mod = double.Parse(chlec) % 2;
                                                    if (mod == 1)
                                                    {
                                                        grdDur.IsEnabled = false;
                                                        scrollhour.Value = int.Parse(chlec) / 2;
                                                        scrollmin.Value = 30;
                                                    }
                                                    else if (mod == 0)
                                                    {
                                                        grdDur.IsEnabled = false;
                                                        scrollhour.Value = int.Parse(chlec) / 2;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (cbmType.SelectedValue.ToString() == "LAB")
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection))
                        {
                            con.Open();
                            cmd = new MySqlCommand("SELECT * FROM `tblsubject` t1 LEFT JOIN `tblcourse` t2 USING (Course_id) WHERE t1.Course_id = t2.Course_id AND Sub_id = '" + cbSubject.SelectedValue.ToString() + "'", con);
                            using (reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        chlab = reader[7].ToString();
                                        foreach (DataRow row in dtcklst.Rows)
                                        {
                                            if (row["Sub_id"].ToString() == cbSubject.SelectedValue.ToString())
                                            {
                                                if (row["Ch_lab"].ToString() == chlab)
                                                {
                                                    scrollhour.Maximum = double.Parse(chlab);
                                                    grdMin.IsEnabled = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void Scrollbar_min()
        {
            scrollmin.Minimum = 0;
            scrollmin.Maximum = 30;
            scrollmin.SmallChange = 30;
        }

        void Start_Time_Enable()
        {
            if (txtHour.Text != "0")
            {
                cbStime.IsEnabled = true;
            }
            else if (txtHour.Text == "0" && txtMin.Text != "0")
            {
                cbStime.IsEnabled = false;
            }
            else if (txtHour.Text == "0" && txtMin.Text == "0")
            {
                cbStime.IsEnabled = false;
                if (cbStime.SelectedIndex != -1)
                {
                    cbStime.SelectedIndex = -1;
                }
            }
        }

        void ComboBox_Manage_Stime()
        {
            cbStime.Items.Add("7:00 AM");
            cbStime.Items.Add("7:30 AM");
            cbStime.Items.Add("8:00 AM");
            cbStime.Items.Add("8:30 AM");
            cbStime.Items.Add("9:00 AM");
            cbStime.Items.Add("9:30 AM");
            cbStime.Items.Add("10:00 AM");
            cbStime.Items.Add("10:30 AM");
            cbStime.Items.Add("11:00 AM");
            cbStime.Items.Add("11:30 AM");
            cbStime.Items.Add("12:00 PM");
            cbStime.Items.Add("12:30 PM");
            cbStime.Items.Add("1:00 PM");
            cbStime.Items.Add("1:30 PM");
            cbStime.Items.Add("2:00 PM");
            cbStime.Items.Add("2:30 PM");
            cbStime.Items.Add("3:00 PM");
            cbStime.Items.Add("3:30 PM");
            cbStime.Items.Add("4:00 PM");
            cbStime.Items.Add("4:30 PM");
            cbStime.Items.Add("5:00 PM");
            cbStime.Items.Add("5:30 PM");
            cbStime.Items.Add("6:00 PM");
            cbStime.Items.Add("6:30 PM");
            cbStime.Items.Add("7:00 PM");
            cbStime.Items.Add("7:30 PM");
            cbStime.Items.Add("8:00 PM");
        }

        // =============== COMBOBOX ITEMS ============= 
        void Selected_Day()
        {
            if (cbDay.SelectedIndex != -1)
            {
                if (cbDay.SelectedValue.ToString() == "MONDAY")
                {
                    Grid.SetColumn(bdr, 0);
                }
                else if (cbDay.SelectedValue.ToString() == "TUESDAY")
                {
                    Grid.SetColumn(bdr, 1);
                }
                else if (cbDay.SelectedValue.ToString() == "WEDNESDAY")
                {
                    Grid.SetColumn(bdr, 2);
                }
                else if (cbDay.SelectedValue.ToString() == "THURSDAY")
                {
                    Grid.SetColumn(bdr, 3);
                }
                else if (cbDay.SelectedValue.ToString() == "FRIDAY")
                {
                    Grid.SetColumn(bdr, 4);
                }
                else if (cbDay.SelectedValue.ToString() == "SATURDAY")
                {
                    Grid.SetColumn(bdr, 5);
                }
            }
        }

        void Border_Init()
        {
            contextmenu();
            bdr = new Border();
            bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            bdr.BorderThickness = new Thickness(2);
            bdr.Margin = new Thickness(-1);
            bdr.Background = new SolidColorBrush(Colors.White);
            bdr.ContextMenu = context;
        }

        void Day_Init()
        {
            if (ldday != string.Empty)
            {
                if (ldday == "MONDAY")
                {
                    Grid.SetColumn(bdr, 0);
                }
                else if (ldday == "TUESDAY")
                {
                    Grid.SetColumn(bdr, 1);
                }
                else if (ldday == "WEDNESDAY")
                {
                    Grid.SetColumn(bdr, 2);
                }
                else if (ldday == "THURSDAY")
                {
                    Grid.SetColumn(bdr, 3);
                }
                else if (ldday == "FRIDAY")
                {
                    Grid.SetColumn(bdr, 4);
                }
                else if (ldday == "SATURDAY")
                {
                    Grid.SetColumn(bdr, 5);
                }
            }
        }

        void Fill_Sched()
        {
            grdSchedule.Children.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                con.Open();
                if (cbInstructor.SelectedIndex != -1 && cbRoom.SelectedIndex == -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, sched.Sched_id FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE room.Room_id IN (SELECT Room_id FROM tblopenrooms) AND sched.Ins_id ='" + cbInstructor.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);

                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                Instructor_Not_Empty();
                            }
                        }
                    }

                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, sched.Sched_id FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Sub_Other != '' AND sched.Ins_id ='" + cbInstructor.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);
                    MySqlDataAdapter dta = new MySqlDataAdapter(cmd);

                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Instructor_Not_Empty();
                            }
                        }
                    }
                }
                else if (cbInstructor.SelectedIndex == -1 && cbRoom.SelectedIndex != -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND sched.Room_id ='" + cbRoom.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);

                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                Room_Not_Empty();
                            }
                        }
                    }
                }
                else if (cbInstructor.SelectedIndex != -1 && cbRoom.SelectedIndex != -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Ins_id, sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND sched.Room_id ='" + cbRoom.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);

                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                Instructor_Room_Not_Empty();
                            }
                        }
                    }
                }

            }
        }

        void Instructor_Not_Empty()
        {
            string ldins = reader[5].ToString(); // INS
            string ldcou = reader[6].ToString(); // COURSE
            string ldys = reader[7].ToString(); // SECTION
            string ldsub = reader[8].ToString(); // SUBJECT
            string ldtype = reader[1].ToString(); // TYPE
            string ldother = reader[2].ToString(); //
            string ldroom = reader[9].ToString(); // ROOM
            ldday = reader[0].ToString(); // DAY
            string ldrow = reader[3].ToString(); // ROW
            string lddur = reader[4].ToString(); // DUR
            string ldid = reader[10].ToString();

            if (ldys == "PETITION")
            {
                Border_Init();
                Day_Init();
                Grid.SetRow(bdr, Int32.Parse(ldrow));
                Grid.SetRowSpan(bdr, Int32.Parse(lddur));
                bdr.Tag = ldid;
                if (Int32.Parse(lddur) <= 4)
                {
                    txt = new TextBlock();
                    txt.Text = ldsub + " : " + ldtype + nl + ldcou + " - " + ldys + nl + ldroom;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);

                    string bdrXaml = XamlWriter.Save(bdr);
                    StringReader stringReader = new StringReader(bdrXaml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    Border newBdr = (Border)XamlReader.Load(xmlReader);
                    grdvSchedule.Children.Add(newBdr);
                }
                else
                {
                    txt = new TextBlock();
                    txt.Text = ldys + nl + nl + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys + nl + ldroom;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);

                    string bdrXaml = XamlWriter.Save(bdr);
                    StringReader stringReader = new StringReader(bdrXaml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    Border newBdr = (Border)XamlReader.Load(xmlReader);
                    grdvSchedule.Children.Add(newBdr);
                }
            }
            else
            {
                Border_Init();
                Day_Init();
                Grid.SetRow(bdr, Int32.Parse(ldrow));
                Grid.SetRowSpan(bdr, Int32.Parse(lddur));
                bdr.Tag = ldid;

                if (ldother == string.Empty)
                {
                    txt = new TextBlock();
                    txt.Text = ldsub + " : " + ldtype + nl + ldcou + " - " + ldys + nl + ldroom;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);

                    string bdrXaml = XamlWriter.Save(bdr);
                    StringReader stringReader = new StringReader(bdrXaml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    Border newBdr = (Border)XamlReader.Load(xmlReader);
                    grdvSchedule.Children.Add(newBdr);
                }
                else
                {
                    txt = new TextBlock();
                    txt.TextWrapping = TextWrapping.Wrap;
                    string x = ldother.Replace(' ', '\n');
                    txt.Text = x;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);

                    string bdrXaml = XamlWriter.Save(bdr);
                    StringReader stringReader = new StringReader(bdrXaml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    Border newBdr = (Border)XamlReader.Load(xmlReader);
                    grdvSchedule.Children.Add(newBdr);
                }
            }
        }

        void Room_Not_Empty()
        {
            string ldins = reader[5].ToString(); // INS
            string ldcou = reader[6].ToString(); // COURSE
            string ldys = reader[7].ToString(); // SECTION
            string ldsub = reader[8].ToString(); // SUBJECT
            string ldtype = reader[1].ToString(); // TYPE
            string ldother = reader[2].ToString(); //
            string ldroom = reader[9].ToString(); // ROOM
            ldday = reader[0].ToString(); // DAY
            string ldrow = reader[3].ToString(); // ROW
            string lddur = reader[4].ToString(); // DUR


            if (ldys == "PETITION")
            {
                Border_Init();
                Day_Init();
                Grid.SetRow(bdr, Int32.Parse(ldrow));
                Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                txt = new TextBlock();
                txt.Text = ldins + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys;
                txt.TextAlignment = TextAlignment.Center;
                txt.VerticalAlignment = VerticalAlignment.Center;
                bdr.Child = txt;
                grdSchedule.Children.Add(bdr);
            }
            else
            {
                Border_Init();
                Day_Init();
                Grid.SetRow(bdr, Int32.Parse(ldrow));
                Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                txt = new TextBlock();
                txt.Text = ldins + nl + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys;
                txt.TextAlignment = TextAlignment.Center;
                txt.VerticalAlignment = VerticalAlignment.Center;
                bdr.Child = txt;
                grdSchedule.Children.Add(bdr);
            }
        }

        void Instructor_Room_Not_Empty()
        {
            string selins = cbInstructor.SelectedValue.ToString();
            string insid = reader[0].ToString(); // INS ID
            ldday = reader[1].ToString(); // DAY
            string ldtype = reader[2].ToString(); // TYPE
            string ldother = reader[3].ToString(); //
            string ldrow = reader[4].ToString(); // ROW
            string lddur = reader[5].ToString(); // DUR
            string ldins = reader[6].ToString(); // INS NAME
            string ldcou = reader[7].ToString(); // COURSE
            string ldys = reader[8].ToString(); // SECTION
            string ldsub = reader[9].ToString(); // SUBJECT
            string ldroom = reader[10].ToString(); // ROOM

            if (insid != selins)
            {
                if (ldys == "PETITION")
                {
                    Border_Init();
                    Day_Init();
                    Grid.SetRow(bdr, Int32.Parse(ldrow));
                    Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                    txt = new TextBlock();
                    txt.Text = ldins + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    txt.Foreground = Brushes.Gray;
                    bdr.Child = txt;
                    bdr.IsEnabled = false;
                    grdSchedule.Children.Add(bdr);
                }
                else
                {
                    Border_Init();
                    Day_Init();
                    Grid.SetRow(bdr, Int32.Parse(ldrow));
                    Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                    txt = new TextBlock();
                    txt.Text = ldins + nl + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    txt.Foreground = Brushes.Gray;
                    bdr.Child = txt;
                    bdr.IsEnabled = false;
                    grdSchedule.Children.Add(bdr);
                }
            }
            else
            {
                if (ldys == "PETITION")
                {
                    Border_Init();
                    Day_Init();
                    Grid.SetRow(bdr, Int32.Parse(ldrow));
                    Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                    txt = new TextBlock();
                    txt.Text = ldins + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
                else
                {
                    Border_Init();
                    Day_Init();
                    Grid.SetRow(bdr, Int32.Parse(ldrow));
                    Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                    txt = new TextBlock();
                    txt.Text = ldins + nl + ldsub +" : " + ldtype + nl + ldcou + " - " + ldys + nl;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
            }
        }

        void Create_Schedule()
        {
            Border_Init();
            Grid.SetRow(bdr, 0);
            Grid.SetRowSpan(bdr, 4);
            Selected_Day();

            txt = new TextBlock();
            txt.Text = cbInstructor.SelectedValue.ToString();
            txt.TextAlignment = TextAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Center;

            bdr.Child = txt;
            grdSchedule.Children.Add(bdr);
        }

        private string Selins
        {
            get
            {
                var subject = cbInstructor.SelectedItem as DataRowView;
                string sub = subject.Row["Ins_name"].ToString();
                return sub;
            }
        }

        private string SelRoom
        {
            get
            {
                var subject = cbRoom.SelectedItem as DataRowView;
                string sub = subject.Row["Room_name"].ToString();
                return sub;
            }
        }

        void Title_Init()
        {
            if (cbInstructor.SelectedIndex != -1 && cbRoom.SelectedIndex == -1)
            {
                tbltwo.Text = Selins;
                grdschedviewer.Width = new GridLength(0, GridUnitType.Pixel);
                grdschedscrollviewer.Width = 822;
                grdschedfooter.Width = 822;
                tblvSched.Text = "";
            }
            else if (cbInstructor.SelectedIndex == -1 && cbRoom.SelectedIndex != -1)
            {
                tbltwo.Text = SelRoom;
            }
            else if (cbInstructor.SelectedIndex != -1 && cbRoom.SelectedIndex != -1)
            {
                tbltwo.Text = SelRoom;

                grdschedviewer.Width = new GridLength(1, GridUnitType.Auto);
                grdschedscrollviewer.Width = 1500;
                grdschedfooter.Width = 801;
                tblvSched.Text = Selins;
            }
            if (cbInstructor.SelectedIndex == -1)
            {
                grdschedviewer.Width = new GridLength(0, GridUnitType.Pixel);
                grdschedscrollviewer.Width = 822;
                grdschedfooter.Width = 822;
                tblvSched.Text = "";
                tbltwo.Text = string.Empty;
            }
        }

        void Button_Enable()
        {
            if (chkOther.IsChecked == true)
            {
                if (cbInstructor.SelectedIndex == -1 || cbDay.SelectedIndex == -1 || (double.Parse(txtHour.Text) + double.Parse(txtMin.Text) == 0) || cbStime.SelectedIndex == -1 || cbOther.SelectedIndex == -1)
                {
                    btnAddSched.IsEnabled = false;
                }
                else
                {
                    btnAddSched.IsEnabled = true;
                }
            }
            else
            {
                if (cbInstructor.SelectedIndex == -1 || cbRoom.SelectedIndex == -1 || cbYrSection.SelectedIndex == -1 || cbSubject.SelectedIndex == -1 || cbmType.SelectedIndex == -1 ||
                cbDay.SelectedIndex == -1 || (Int32.Parse(txtHour.Text) + Int32.Parse(txtMin.Text) == 0) || cbStime.SelectedIndex == -1)
                {
                    btnAddSched.IsEnabled = false;
                }
                else
                {
                    btnAddSched.IsEnabled = true;
                }
            }
        }

        private void CbInstructor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Title_Init();
            Button_Enable();
            Fill_Sched();
            grdOther.IsEnabled = true;
            if (cbInstructor.SelectedIndex != -1)
            {
                cbYrSection.IsEnabled = true;
            }
            else
            {
                cbYrSection.SelectedIndex = -1;
                cbYrSection.IsEnabled = true;
            }
        }

        private void CbRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Title_Init();
            Button_Enable();
            Fill_Sched();
            
            if (cbRoom.SelectedIndex != -1)
            {
                cbDay.IsEnabled = true;
            }
            else
            {
                cbDay.SelectedIndex = -1;
                cbDay.IsEnabled = false;
            }
        }

        private void CbYrSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbYrSection.SelectedIndex != -1)
            {
                grdchklist.Height = new GridLength(1, GridUnitType.Auto);
                bdrChecklist.Visibility = Visibility.Visible;
                dtChecklist.Visibility = Visibility.Visible;
                string x = SelSecYear;
                if (x != "PETITION")
                {
                    int yrs = Int32.Parse(x.Substring(0, 1));
                    if (yrs == 1)
                    {
                        year = "1ST";
                    }
                    else if (yrs == 2)
                    {
                        year = "2ND";
                    }
                    else if (yrs == 3)
                    {
                        year = "3RD";
                    }
                    else if (yrs == 4)
                    {
                        year = "4TH";
                    }
                    else if (yrs == 5)
                    {
                        year = "5TH";
                    }
                    else if (yrs == 6)
                    {
                        year = "6TH";
                    }
                    else if (yrs == 7)
                    {
                        year = "7TH";
                    }
                    else if (yrs == 8)
                    {
                        year = "8TH";
                    }
                    pet = string.Empty;
                }
                else
                {
                    pet = x;
                }
                ComboBox_Manage_Subjects();
                cbSubject.IsEnabled = true;
            }
            else
            {
                if (cbSubject.SelectedIndex != -1 || cbSubject.IsEnabled == true)
                {
                    cbSubject.IsEnabled = false;
                    cbSubject.SelectedIndex = -1;
                }
                if (cbmType.SelectedIndex != -1)
                {
                    cbmType.IsEnabled = false;
                    cbmType.SelectedIndex = -1;
                }
                if (Int32.Parse(txtHour.Text) != 0 || Int32.Parse(txtMin.Text) != 0)
                {
                    txtHour.Text = "0";
                    scrollhour.Value = 0;
                    txtMin.Text = "0";
                    scrollmin.Value = 0;
                }
                grdchklist.Height = new GridLength(0);
                bdrChecklist.Visibility = Visibility.Collapsed;
                if (exsubj != null)
                {
                    exsubj.Clear();
                }
                if (dtcklst != null)
                {
                    dtcklst.Clear();
                }
            }
            Button_Enable();
        }

        private void CbSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSubject.SelectedIndex != -1)
            {
                cbmType.SelectedIndex = -1;
                cbmType.IsEnabled = true;
                ComboBox_Manage_SubType();
            }
            else
            {
                if (cbmType.SelectedIndex != -1)
                {
                    cbmType.IsEnabled = false;
                    cbmType.SelectedIndex = -1;
                }
                cbmType.IsEnabled = false;
            }
            Button_Enable();
        }

        private void CbmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbmType.SelectedIndex != -1)
            {
                cbRoom.IsEnabled = true;
            }
            else
            {
                cbRoom.SelectedIndex = -1;
                cbRoom.IsEnabled = false;
            }
            Button_Enable();
        }

        private void CbDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDay.SelectedIndex != -1)
            {
                Scrollbar_hour();
            }
            else
            {
                grdDur.IsEnabled = false;
                txtHour.Text = "0";
                txtMin.Text = "0";
            }
            Button_Enable();
        }

        private void Scrollhour_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtHour.Text = scrollhour.Value.ToString();
            if (cbmType.SelectedIndex != -1)
            {
                if (cbmType.SelectedValue.ToString() == "LEC")
                {
                    if (scrollhour.Value == double.Parse(chlec))
                    {
                        grdMin.IsEnabled = false;
                    }
                    else
                    {
                        if (grdDur.IsEnabled == true)
                        {
                            grdMin.IsEnabled = true;
                        }
                    }
                }
                else if (cbmType.SelectedValue.ToString() == "LAB")
                {
                    if (scrollhour.Value == double.Parse(chlab))
                    {
                        grdMin.IsEnabled = false;
                    }
                    else
                    {
                        if (grdDur.IsEnabled == true)
                        {
                            grdMin.IsEnabled = true;
                        }
                    }
                }
            }
            else if (chkOther.IsChecked == true)
            {
                if (cbOther.SelectedValue.ToString() == "RESEARCH" || cbOther.SelectedValue.ToString() == "EXTENSION")
                {
                    if (scrollhour.Value == double.Parse(othermaxtime))
                    {
                        grdMin.IsEnabled = false;
                    }
                    else
                    {
                        if (grdDur.IsEnabled == true)
                        {
                            grdMin.IsEnabled = true;
                        }
                    }
                }
            }
            Start_Time_Enable();
            Button_Enable();
            if (s_time != string.Empty)
            {
                ETime();
            }
        }

        private void Scrollmin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtMin.Text = scrollmin.Value.ToString();
            Start_Time_Enable();
            Button_Enable();
            if (s_time != string.Empty)
            {
                ETime();
            }
        }

        string s_time = string.Empty;
        string e_time = string.Empty;

        void ETime()
        {
            DateTime time1 = DateTime.Today;
            TimeSpan ts = DateTime.ParseExact(s_time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            ts = ts + TimeSpan.FromMinutes(30);
            int hour = Int32.Parse(txtHour.Text);
            int min = Int32.Parse(txtMin.Text);
            int convertedmin = 0;
            if (min == 30)
            {
                convertedmin = 1;
            }
            int rs = (hour * 2) + (convertedmin);
            for (int i = 0; i <= (rs - 1); i++)
            {
                time1 = DateTime.Today.Add(ts);
                ts = ts + TimeSpan.FromMinutes(30);
            }
            e_time = time1.ToString("h:mm tt");
        }

        int duration = 0;

        void Duration_Calc()
        {
            int hour = Int32.Parse(txtHour.Text);
            int min = Int32.Parse(txtMin.Text);
            int convertedmin = 0;
            if (min == 30)
            {
                convertedmin = 1;
            }
            int rs = (hour * 2) + (convertedmin);
            duration = rs;
        }

        private void CbStime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Button_Enable();
            if (cbStime.SelectedIndex != -1)
            {
                s_time = cbStime.SelectedValue.ToString();
            }
            ETime();
        }

        void Reset_control()
        {
            if (chkOther.IsChecked == true)
            {
                chkOther.IsChecked = false;
            }
            else
            {
                cbSubject.SelectedIndex = -1;
            }
        }

        void Conflict_Checker()
        {
            double hh = double.Parse(txtHour.Text);
            double m = 0;
            if (double.Parse(txtMin.Text) == 30)
            {
                m = 0.5;
            }
            double dur = hh + m;

            if (chkOther.IsChecked == true)
            {
                if (dur == double.Parse(othermaxtime))
                {
                    using (MySqlConnection con = new MySqlConnection(Connection))
                    {// INSTRUCTOR
                        cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString()
                                             + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            string schedid = string.Empty;
                            while (reader.Read())
                            {
                                schedid = reader[0].ToString();
                            }
                            reader.Close();
                            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                            using (reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if (reader[1] != DBNull.Value)
                                        {
                                            string a = reader.GetString(0); // Day
                                            string e = reader.GetString(1); // Type
                                            string f = reader.GetString(2); // Other
                                            string b = reader.GetString(6); // Course
                                            string c = reader.GetString(7); // Section
                                            string d = reader.GetString(8); // Subject
                                            string g = reader.GetString(10); // Stime
                                            string h = reader.GetString(11); // Etime

                                            if (f != string.Empty)
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                        else
                                        {
                                            string a = reader.GetString(0); // Day
                                            string f = reader.GetString(2); // Other
                                            string g = reader.GetString(10); // Stime
                                            string h = reader.GetString(11); // Etime

                                            if (f != string.Empty)
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            reader.Close();
                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString() + "'AND Sched_Sti < STR_TO_DATE('" + e_time + "', '%l:%i %p') and Sched_Eti >= STR_TO_DATE('" + e_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                string schedid = string.Empty;
                                while (reader.Read())
                                {
                                    schedid = reader[0].ToString();
                                }
                                reader.Close();
                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                using (reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader[1] != DBNull.Value)
                                            {
                                                string a = reader.GetString(0); // Day
                                                string e = reader.GetString(1); // Type
                                                string f = reader.GetString(2); // Other
                                                string b = reader.GetString(6); // Course
                                                string c = reader.GetString(7); // Section
                                                string d = reader.GetString(8); // Subject
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                            else
                                            {
                                                string a = reader.GetString(0); // Day
                                                string f = reader.GetString(2); // Other
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {//SECTION
                                reader.Close();
                                TimeSpan tl = DateTime.ParseExact("8:00 PM", "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                TimeSpan teT = DateTime.ParseExact(e_time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                DateTime endtime = DateTime.Today.Add(teT);
                                DateTime timelimit = DateTime.Today.Add(tl);
                                if (endtime > timelimit)
                                {
                                    MessageBox.Show("Schedule overlaps the limit.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                }
                                else
                                {
                                    con.Dispose();
                                    string svins = cbInstructor.SelectedValue.ToString();
                                    string svother = cbOther.SelectedValue.ToString();
                                    string svday = cbDay.SelectedValue.ToString();
                                    DateTime svSt = DateTime.Parse(s_time);
                                    DateTime svEt = DateTime.Parse(e_time);
                                    int svrow = Row(cbStime.SelectedIndex);
                                    Duration_Calc();
                                    try
                                    {
                                        cmd = new MySqlCommand("INSERT INTO `tblsched`(`Ins_id`, `Sub_Other`, `Sched_Day`, `Sched_Sti`, `Sched_Eti`, `Sched_row`, `Sched_Dur`, `Sem_id`, `Sy_id`)"
                                                             + "VALUES (@ins,@other,@day,@sti,@eti,@row,@dur,@sem,@sy)", con);
                                        cmd.Parameters.AddWithValue("@ins", svins);
                                        cmd.Parameters.AddWithValue("@other", svother);
                                        cmd.Parameters.AddWithValue("@day", svday);
                                        cmd.Parameters.AddWithValue("@sti", svSt);
                                        cmd.Parameters.AddWithValue("@eti", svEt);
                                        cmd.Parameters.AddWithValue("@row", svrow);
                                        cmd.Parameters.AddWithValue("@dur", duration);
                                        cmd.Parameters.AddWithValue("@sem", semester);
                                        cmd.Parameters.AddWithValue("@sy", schoolyear);

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Dispose();

                                        MessageBox.Show("The Schedule has been added successfully.");
                                        Fill_Sched();
                                        Reset_control();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (dur == (double.Parse(othermaxtime) / 2))
                {
                    using (MySqlConnection con = new MySqlConnection(Connection))
                    {// INSTRUCTOR
                        cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString()
                                             + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            string schedid = string.Empty;
                            while (reader.Read())
                            {
                                schedid = reader[0].ToString();
                            }
                            reader.Close();
                            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                            using (reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if (reader[1] != DBNull.Value)
                                        {
                                            string a = reader.GetString(0); // Day
                                            string e = reader.GetString(1); // Type
                                            string f = reader.GetString(2); // Other
                                            string b = reader.GetString(6); // Course
                                            string c = reader.GetString(7); // Section
                                            string d = reader.GetString(8); // Subject
                                            string g = reader.GetString(10); // Stime
                                            string h = reader.GetString(11); // Etime

                                            if (f != string.Empty)
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                        else
                                        {
                                            string a = reader.GetString(0); // Day
                                            string f = reader.GetString(2); // Other
                                            string g = reader.GetString(10); // Stime
                                            string h = reader.GetString(11); // Etime

                                            if (f != string.Empty)
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            reader.Close();
                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString() + "'AND Sched_Sti < STR_TO_DATE('" + e_time + "', '%l:%i %p') and Sched_Eti >= STR_TO_DATE('" + e_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                string schedid = string.Empty;
                                while (reader.Read())
                                {
                                    schedid = reader[0].ToString();
                                }
                                reader.Close();
                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                using (reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader[1] != DBNull.Value)
                                            {
                                                string a = reader.GetString(0); // Day
                                                string e = reader.GetString(1); // Type
                                                string f = reader.GetString(2); // Other
                                                string b = reader.GetString(6); // Course
                                                string c = reader.GetString(7); // Section
                                                string d = reader.GetString(8); // Subject
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                            else
                                            {
                                                string a = reader.GetString(0); // Day
                                                string f = reader.GetString(2); // Other
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {//SECTION
                                reader.Close();
                                TimeSpan tl = DateTime.ParseExact("8:00 PM", "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                TimeSpan teT = DateTime.ParseExact(e_time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                DateTime endtime = DateTime.Today.Add(teT);
                                DateTime timelimit = DateTime.Today.Add(tl);
                                if (endtime > timelimit)
                                {
                                    MessageBox.Show("Schedule overlaps the limit.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                }
                                else
                                {
                                    con.Dispose();
                                    string svins = cbInstructor.SelectedValue.ToString();
                                    string svother = cbOther.SelectedValue.ToString();
                                    string svday = cbDay.SelectedValue.ToString();
                                    DateTime svSt = DateTime.Parse(s_time);
                                    DateTime svEt = DateTime.Parse(e_time);
                                    int svrow = Row(cbStime.SelectedIndex);
                                    Duration_Calc();
                                    try
                                    {
                                        cmd = new MySqlCommand("INSERT INTO `tblsched`(`Ins_id`, `Sub_Other`, `Sched_Day`, `Sched_Sti`, `Sched_Eti`, `Sched_row`, `Sched_Dur`, `Sem_id`, `Sy_id`)"
                                                             + "VALUES (@ins,@other,@day,@sti,@eti,@row,@dur,@sem,@sy)", con);
                                        cmd.Parameters.AddWithValue("@ins", svins);
                                        cmd.Parameters.AddWithValue("@other", svother);
                                        cmd.Parameters.AddWithValue("@day", svday);
                                        cmd.Parameters.AddWithValue("@sti", svSt);
                                        cmd.Parameters.AddWithValue("@eti", svEt);
                                        cmd.Parameters.AddWithValue("@row", svrow);
                                        cmd.Parameters.AddWithValue("@dur", duration);
                                        cmd.Parameters.AddWithValue("@sem", semester);
                                        cmd.Parameters.AddWithValue("@sy", schoolyear);

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Dispose();

                                        MessageBox.Show("The Schedule has been added successfully.");
                                        Fill_Sched();
                                        Reset_control();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (dur != (double.Parse(othermaxtime) / 2))
                {
                    MessageBox.Show("Duration cannot be longer or shorter than the half of the designated appointment's full duration.");
                }
            }
            else
            {
                if (cbmType.SelectedValue.ToString() == "LEC")
                {
                    if (dur == double.Parse(chlec))
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection))
                        {// INSTRUCTOR
                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString()
                                                 + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                            con.Open();
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                string schedid = string.Empty;
                                while (reader.Read())
                                {
                                    schedid = reader[0].ToString();
                                }
                                reader.Close();
                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                using (reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader[1] != DBNull.Value)
                                            {
                                                string a = reader.GetString(0); // Day
                                                string e = reader.GetString(1); // Type
                                                string f = reader.GetString(2); // Other
                                                string b = reader.GetString(6); // Course
                                                string c = reader.GetString(7); // Section
                                                string d = reader.GetString(8); // Subject
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                            else
                                            {
                                                string a = reader.GetString(0); // Day
                                                string f = reader.GetString(2); // Other
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                reader.Close();
                                cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString() + "'AND Sched_Sti < STR_TO_DATE('" + e_time + "', '%l:%i %p') and Sched_Eti >= STR_TO_DATE('" + e_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    string schedid = string.Empty;
                                    while (reader.Read())
                                    {
                                        schedid = reader[0].ToString();
                                    }
                                    reader.Close();
                                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                    using (reader = cmd.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            while (reader.Read())
                                            {
                                                if (reader[1] != DBNull.Value)
                                                {
                                                    string a = reader.GetString(0); // Day
                                                    string e = reader.GetString(1); // Type
                                                    string f = reader.GetString(2); // Other
                                                    string b = reader.GetString(6); // Course
                                                    string c = reader.GetString(7); // Section
                                                    string d = reader.GetString(8); // Subject
                                                    string g = reader.GetString(10); // Stime
                                                    string h = reader.GetString(11); // Etime

                                                    if (f != string.Empty)
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                                else
                                                {
                                                    string a = reader.GetString(0); // Day
                                                    string f = reader.GetString(2); // Other
                                                    string g = reader.GetString(10); // Stime
                                                    string h = reader.GetString(11); // Etime

                                                    if (f != string.Empty)
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {//SECTION
                                    reader.Close();
                                    cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Course_id='" + CourseId + "'And Sec_id='" + cbYrSection.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                    reader = cmd.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        string schedid = string.Empty;
                                        while (reader.Read())
                                        {
                                            schedid = reader[0].ToString();
                                        }
                                        reader.Close();
                                        cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                        using (reader = cmd.ExecuteReader())
                                        {
                                            if (reader.HasRows)
                                            {
                                                while (reader.Read())
                                                {
                                                    if (reader[1] != DBNull.Value)
                                                    {
                                                        string a = reader.GetString(0); // Day
                                                        string e = reader.GetString(1); // Type
                                                        string f = reader.GetString(2); // Other
                                                        string b = reader.GetString(6); // Course
                                                        string c = reader.GetString(7); // Section
                                                        string d = reader.GetString(8); // Subject
                                                        string g = reader.GetString(10); // Stime
                                                        string h = reader.GetString(11); // Etime

                                                        if (f != string.Empty)
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string a = reader.GetString(0); // Day
                                                        string f = reader.GetString(2); // Other
                                                        string g = reader.GetString(10); // Stime
                                                        string h = reader.GetString(11); // Etime

                                                        if (f != string.Empty)
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        reader.Close();
                                        cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Course_id='" + CourseId + "'And Sec_id='" + cbYrSection.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                        reader = cmd.ExecuteReader();
                                        if (reader.HasRows)
                                        {
                                            string schedid = string.Empty;
                                            while (reader.Read())
                                            {
                                                schedid = reader[0].ToString();
                                            }
                                            reader.Close();
                                            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                            using (reader = cmd.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        if (reader[1] != DBNull.Value)
                                                        {
                                                            string a = reader.GetString(0); // Day
                                                            string e = reader.GetString(1); // Type
                                                            string f = reader.GetString(2); // Other
                                                            string b = reader.GetString(6); // Course
                                                            string c = reader.GetString(7); // Section
                                                            string d = reader.GetString(8); // Subject
                                                            string g = reader.GetString(10); // Stime
                                                            string h = reader.GetString(11); // Etime

                                                            if (f != string.Empty)
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string a = reader.GetString(0); // Day
                                                            string f = reader.GetString(2); // Other
                                                            string g = reader.GetString(10); // Stime
                                                            string h = reader.GetString(11); // Etime

                                                            if (f != string.Empty)
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {//Room
                                            reader.Close();
                                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Room_id='" + cbRoom.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                            reader = cmd.ExecuteReader();
                                            if (reader.HasRows)
                                            {
                                                string schedid = string.Empty;
                                                while (reader.Read())
                                                {
                                                    schedid = reader[0].ToString();
                                                }
                                                reader.Close();
                                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                                using (reader = cmd.ExecuteReader())
                                                {
                                                    if (reader.HasRows)
                                                    {
                                                        while (reader.Read())
                                                        {
                                                            if (reader[1] != DBNull.Value)
                                                            {
                                                                string a = reader.GetString(0); // Day
                                                                string e = reader.GetString(1); // Type
                                                                string f = reader.GetString(2); // Other
                                                                string b = reader.GetString(6); // Course
                                                                string c = reader.GetString(7); // Section
                                                                string d = reader.GetString(8); // Subject
                                                                string g = reader.GetString(10); // Stime
                                                                string h = reader.GetString(11); // Etime

                                                                if (f != string.Empty)
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string a = reader.GetString(0); // Day
                                                                string f = reader.GetString(2); // Other
                                                                string g = reader.GetString(10); // Stime
                                                                string h = reader.GetString(11); // Etime

                                                                if (f != string.Empty)
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                reader.Close();
                                                cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Room_id='" + cbRoom.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                                reader = cmd.ExecuteReader();
                                                if (reader.HasRows)
                                                {
                                                    string schedid = string.Empty;
                                                    while (reader.Read())
                                                    {
                                                        schedid = reader[0].ToString();
                                                    }
                                                    reader.Close();
                                                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                                    using (reader = cmd.ExecuteReader())
                                                    {
                                                        if (reader.HasRows)
                                                        {
                                                            while (reader.Read())
                                                            {
                                                                if (reader[1] != DBNull.Value)
                                                                {
                                                                    string a = reader.GetString(0); // Day
                                                                    string e = reader.GetString(1); // Type
                                                                    string f = reader.GetString(2); // Other
                                                                    string b = reader.GetString(6); // Course
                                                                    string c = reader.GetString(7); // Section
                                                                    string d = reader.GetString(8); // Subject
                                                                    string g = reader.GetString(10); // Stime
                                                                    string h = reader.GetString(11); // Etime

                                                                    if (f != string.Empty)
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                    else
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string a = reader.GetString(0); // Day
                                                                    string f = reader.GetString(2); // Other
                                                                    string g = reader.GetString(10); // Stime
                                                                    string h = reader.GetString(11); // Etime

                                                                    if (f != string.Empty)
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                    else
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {// SAVE
                                                    TimeSpan tl = DateTime.ParseExact("8:00 PM", "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                                    TimeSpan teT = DateTime.ParseExact(e_time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                                    DateTime endtime = DateTime.Today.Add(teT);
                                                    DateTime timelimit = DateTime.Today.Add(tl);
                                                    if (endtime > timelimit)
                                                    {
                                                        MessageBox.Show("Schedule overlaps the limit.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                                    }
                                                    else
                                                    {
                                                        if (chkOther.IsChecked == true)
                                                        {
                                                            con.Dispose();
                                                            string svins = cbInstructor.SelectedValue.ToString();
                                                            string svother = cbOther.SelectedValue.ToString();
                                                            string svroom = cbRoom.SelectedValue.ToString();
                                                            string svday = cbDay.SelectedValue.ToString();
                                                            DateTime svSt = DateTime.Parse(s_time);
                                                            DateTime svEt = DateTime.Parse(e_time);
                                                            int svrow = Row(cbStime.SelectedIndex);
                                                            Duration_Calc();
                                                            try
                                                            {
                                                                cmd = new MySqlCommand("INSERT INTO `tblsched`(`Ins_id`, `Sub_Other`, `Sched_Day`, `Sched_Sti`, `Sched_Eti`, `Sched_row`, `Sched_Dur`, `Sem_id`, `Sy_id`)"
                                                                                     + "VALUES (@ins,@other,@day,@sti,@eti,@row,@dur,@sem,@sy)", con);
                                                                cmd.Parameters.AddWithValue("@ins", svins);
                                                                cmd.Parameters.AddWithValue("@other", svother);
                                                                cmd.Parameters.AddWithValue("@day", svday);
                                                                cmd.Parameters.AddWithValue("@sti", svSt);
                                                                cmd.Parameters.AddWithValue("@eti", svEt);
                                                                cmd.Parameters.AddWithValue("@row", svrow);
                                                                cmd.Parameters.AddWithValue("@dur", duration);
                                                                cmd.Parameters.AddWithValue("@sem", semester);
                                                                cmd.Parameters.AddWithValue("@sy", schoolyear);

                                                                con.Open();
                                                                cmd.ExecuteNonQuery();
                                                                con.Dispose();

                                                                MessageBox.Show("The Schedule has been added successfully.");
                                                                Fill_Sched();
                                                                Reset_control();
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                MessageBox.Show(ex.ToString());
                                                                throw;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            con.Dispose();
                                                            string svins = cbInstructor.SelectedValue.ToString();
                                                            string svcou = CourseId;
                                                            string svys = SecId;
                                                            string svsub = cbSubject.SelectedValue.ToString();
                                                            string svtype = cbmType.SelectedValue.ToString();
                                                            string svroom = cbRoom.SelectedValue.ToString();
                                                            string svday = cbDay.SelectedValue.ToString();
                                                            DateTime svSt = DateTime.Parse(s_time);
                                                            DateTime svEt = DateTime.Parse(e_time);
                                                            int svrow = Row(cbStime.SelectedIndex);
                                                            Duration_Calc();
                                                            try
                                                            {
                                                                cmd = new MySqlCommand("INSERT INTO `tblsched`(`Ins_id`, `Course_id`, `Sec_id`, `Sub_id`, `Sub_type`, `Room_id`, `Sched_Day`, `Sched_Sti`, `Sched_Eti`, `Sched_row`, `Sched_Dur`, `Sem_id`, `Sy_id`)"
                                                                                   + "VALUES (@ins,@cou,@ys,@sub,@type,@room,@day,@sti,@eti,@row,@dur,@sem,@sy)", con);
                                                                cmd.Parameters.AddWithValue("@ins", svins);
                                                                cmd.Parameters.AddWithValue("@cou", svcou);
                                                                cmd.Parameters.AddWithValue("@ys", svys);
                                                                cmd.Parameters.AddWithValue("@sub", svsub);
                                                                cmd.Parameters.AddWithValue("@type", svtype);
                                                                cmd.Parameters.AddWithValue("@room", svroom);
                                                                cmd.Parameters.AddWithValue("@day", svday);
                                                                cmd.Parameters.AddWithValue("@sti", svSt.ToString("HH:mm"));
                                                                cmd.Parameters.AddWithValue("@eti", svEt.ToString("HH:mm"));
                                                                cmd.Parameters.AddWithValue("@row", svrow);
                                                                cmd.Parameters.AddWithValue("@dur", duration);
                                                                cmd.Parameters.AddWithValue("@sem", semester);
                                                                cmd.Parameters.AddWithValue("@sy", schoolyear);

                                                                con.Open();
                                                                cmd.ExecuteNonQuery();
                                                                con.Dispose();

                                                                MessageBox.Show("The Schedule has been added successfully.");
                                                                Fill_Sched();
                                                                Reset_control();
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                MessageBox.Show(ex.ToString());
                                                                throw;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (dur != (double.Parse(chlec) / 2))
                    {
                        MessageBox.Show("Duration cannot be longer or shorter than the half of the subject's full duration.");
                    }
                    else if (dur == (double.Parse(chlec) / 2))
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection))
                        {// INSTRUCTOR
                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString()
                                                 + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                            con.Open();
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                string schedid = string.Empty;
                                while (reader.Read())
                                {
                                    schedid = reader[0].ToString();
                                }
                                reader.Close();
                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                using (reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader[1] != DBNull.Value)
                                            {
                                                string a = reader.GetString(0); // Day
                                                string e = reader.GetString(1); // Type
                                                string f = reader.GetString(2); // Other
                                                string b = reader.GetString(6); // Course
                                                string c = reader.GetString(7); // Section
                                                string d = reader.GetString(8); // Subject
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                            else
                                            {
                                                string a = reader.GetString(0); // Day
                                                string f = reader.GetString(2); // Other
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                reader.Close();
                                cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString() + "'AND Sched_Sti < STR_TO_DATE('" + e_time + "', '%l:%i %p') and Sched_Eti >= STR_TO_DATE('" + e_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    string schedid = string.Empty;
                                    while (reader.Read())
                                    {
                                        schedid = reader[0].ToString();
                                    }
                                    reader.Close();
                                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                    using (reader = cmd.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            while (reader.Read())
                                            {
                                                if (reader[1] != DBNull.Value)
                                                {
                                                    string a = reader.GetString(0); // Day
                                                    string e = reader.GetString(1); // Type
                                                    string f = reader.GetString(2); // Other
                                                    string b = reader.GetString(6); // Course
                                                    string c = reader.GetString(7); // Section
                                                    string d = reader.GetString(8); // Subject
                                                    string g = reader.GetString(10); // Stime
                                                    string h = reader.GetString(11); // Etime

                                                    if (f != string.Empty)
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                                else
                                                {
                                                    string a = reader.GetString(0); // Day
                                                    string f = reader.GetString(2); // Other
                                                    string g = reader.GetString(10); // Stime
                                                    string h = reader.GetString(11); // Etime

                                                    if (f != string.Empty)
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {//SECTION
                                    reader.Close();
                                    cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Course_id='" + CourseId + "'And Sec_id='" + cbYrSection.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                    reader = cmd.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        string schedid = string.Empty;
                                        while (reader.Read())
                                        {
                                            schedid = reader[0].ToString();
                                        }
                                        reader.Close();
                                        cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                        using (reader = cmd.ExecuteReader())
                                        {
                                            if (reader.HasRows)
                                            {
                                                while (reader.Read())
                                                {
                                                    if (reader[1] != DBNull.Value)
                                                    {
                                                        string a = reader.GetString(0); // Day
                                                        string e = reader.GetString(1); // Type
                                                        string f = reader.GetString(2); // Other
                                                        string b = reader.GetString(6); // Course
                                                        string c = reader.GetString(7); // Section
                                                        string d = reader.GetString(8); // Subject
                                                        string g = reader.GetString(10); // Stime
                                                        string h = reader.GetString(11); // Etime

                                                        if (f != string.Empty)
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string a = reader.GetString(0); // Day
                                                        string f = reader.GetString(2); // Other
                                                        string g = reader.GetString(10); // Stime
                                                        string h = reader.GetString(11); // Etime

                                                        if (f != string.Empty)
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        reader.Close();
                                        cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Course_id='" + CourseId + "'And Sec_id='" + cbYrSection.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                        reader = cmd.ExecuteReader();
                                        if (reader.HasRows)
                                        {
                                            string schedid = string.Empty;
                                            while (reader.Read())
                                            {
                                                schedid = reader[0].ToString();
                                            }
                                            reader.Close();
                                            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                            using (reader = cmd.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        if (reader[1] != DBNull.Value)
                                                        {
                                                            string a = reader.GetString(0); // Day
                                                            string e = reader.GetString(1); // Type
                                                            string f = reader.GetString(2); // Other
                                                            string b = reader.GetString(6); // Course
                                                            string c = reader.GetString(7); // Section
                                                            string d = reader.GetString(8); // Subject
                                                            string g = reader.GetString(10); // Stime
                                                            string h = reader.GetString(11); // Etime

                                                            if (f != string.Empty)
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string a = reader.GetString(0); // Day
                                                            string f = reader.GetString(2); // Other
                                                            string g = reader.GetString(10); // Stime
                                                            string h = reader.GetString(11); // Etime

                                                            if (f != string.Empty)
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {//Room
                                            reader.Close();
                                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Room_id='" + cbRoom.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                            reader = cmd.ExecuteReader();
                                            if (reader.HasRows)
                                            {
                                                string schedid = string.Empty;
                                                while (reader.Read())
                                                {
                                                    schedid = reader[0].ToString();
                                                }
                                                reader.Close();
                                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                                using (reader = cmd.ExecuteReader())
                                                {
                                                    if (reader.HasRows)
                                                    {
                                                        while (reader.Read())
                                                        {
                                                            if (reader[1] != DBNull.Value)
                                                            {
                                                                string a = reader.GetString(0); // Day
                                                                string e = reader.GetString(1); // Type
                                                                string f = reader.GetString(2); // Other
                                                                string b = reader.GetString(6); // Course
                                                                string c = reader.GetString(7); // Section
                                                                string d = reader.GetString(8); // Subject
                                                                string g = reader.GetString(10); // Stime
                                                                string h = reader.GetString(11); // Etime

                                                                if (f != string.Empty)
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string a = reader.GetString(0); // Day
                                                                string f = reader.GetString(2); // Other
                                                                string g = reader.GetString(10); // Stime
                                                                string h = reader.GetString(11); // Etime

                                                                if (f != string.Empty)
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                reader.Close();
                                                cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Room_id='" + cbRoom.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                                reader = cmd.ExecuteReader();
                                                if (reader.HasRows)
                                                {
                                                    string schedid = string.Empty;
                                                    while (reader.Read())
                                                    {
                                                        schedid = reader[0].ToString();
                                                    }
                                                    reader.Close();
                                                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                                    using (reader = cmd.ExecuteReader())
                                                    {
                                                        if (reader.HasRows)
                                                        {
                                                            while (reader.Read())
                                                            {
                                                                if (reader[1] != DBNull.Value)
                                                                {
                                                                    string a = reader.GetString(0); // Day
                                                                    string e = reader.GetString(1); // Type
                                                                    string f = reader.GetString(2); // Other
                                                                    string b = reader.GetString(6); // Course
                                                                    string c = reader.GetString(7); // Section
                                                                    string d = reader.GetString(8); // Subject
                                                                    string g = reader.GetString(10); // Stime
                                                                    string h = reader.GetString(11); // Etime

                                                                    if (f != string.Empty)
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                    else
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string a = reader.GetString(0); // Day
                                                                    string f = reader.GetString(2); // Other
                                                                    string g = reader.GetString(10); // Stime
                                                                    string h = reader.GetString(11); // Etime

                                                                    if (f != string.Empty)
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                    else
                                                                    {
                                                                        MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {// SAVE
                                                    TimeSpan tl = DateTime.ParseExact("8:00 PM", "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                                    TimeSpan teT = DateTime.ParseExact(e_time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                                    DateTime endtime = DateTime.Today.Add(teT);
                                                    DateTime timelimit = DateTime.Today.Add(tl);
                                                    if (endtime > timelimit)
                                                    {
                                                        MessageBox.Show("Schedule overlaps the limit.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                                    }
                                                    else
                                                    {
                                                        con.Dispose();
                                                        string svins = cbInstructor.SelectedValue.ToString();
                                                        string svcou = CourseId;
                                                        string svys = SecId;
                                                        string svsub = cbSubject.SelectedValue.ToString();
                                                        string svtype = cbmType.SelectedValue.ToString();
                                                        string svroom = cbRoom.SelectedValue.ToString();
                                                        string svday = cbDay.SelectedValue.ToString();
                                                        DateTime svSt = DateTime.Parse(s_time);
                                                        DateTime svEt = DateTime.Parse(e_time);
                                                        int svrow = Row(cbStime.SelectedIndex);
                                                        Duration_Calc();

                                                        try
                                                        {
                                                            cmd = new MySqlCommand("INSERT INTO `tblsched`(`Ins_id`, `Course_id`, `Sec_id`, `Sub_id`, `Sub_type`, `Room_id`, `Sched_Day`, `Sched_Sti`, `Sched_Eti`, `Sched_row`, `Sched_Dur`, `Sem_id`, `Sy_id`)"
                                                                               + "VALUES (@ins,@cou,@ys,@sub,@type,@room,@day,@sti,@eti,@row,@dur,@sem,@sy)", con);
                                                            cmd.Parameters.AddWithValue("@ins", svins);
                                                            cmd.Parameters.AddWithValue("@cou", svcou);
                                                            cmd.Parameters.AddWithValue("@ys", svys);
                                                            cmd.Parameters.AddWithValue("@sub", svsub);
                                                            cmd.Parameters.AddWithValue("@type", svtype);
                                                            cmd.Parameters.AddWithValue("@room", svroom);
                                                            cmd.Parameters.AddWithValue("@day", svday);
                                                            cmd.Parameters.AddWithValue("@sti", svSt.ToString("HH:mm"));
                                                            cmd.Parameters.AddWithValue("@eti", svEt.ToString("HH:mm"));
                                                            cmd.Parameters.AddWithValue("@row", svrow);
                                                            cmd.Parameters.AddWithValue("@dur", duration);
                                                            cmd.Parameters.AddWithValue("@sem", semester);
                                                            cmd.Parameters.AddWithValue("@sy", schoolyear);

                                                            con.Open();
                                                            cmd.ExecuteNonQuery();
                                                            con.Dispose();

                                                            MessageBox.Show("The Schedule has been added successfully.");
                                                            Fill_Sched();
                                                            Reset_control();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            MessageBox.Show(ex.ToString());
                                                            throw;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (cbmType.SelectedValue.ToString() == "LAB")
                {
                    using (MySqlConnection con = new MySqlConnection(Connection))
                    {// INSTRUCTOR
                        cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString()
                                             + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            string schedid = string.Empty;
                            while (reader.Read())
                            {
                                schedid = reader[0].ToString();
                            }
                            reader.Close();
                            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                            using (reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if (reader[1] != DBNull.Value)
                                        {
                                            string a = reader.GetString(0); // Day
                                            string e = reader.GetString(1); // Type
                                            string f = reader.GetString(2); // Other
                                            string b = reader.GetString(6); // Course
                                            string c = reader.GetString(7); // Section
                                            string d = reader.GetString(8); // Subject
                                            string g = reader.GetString(10); // Stime
                                            string h = reader.GetString(11); // Etime

                                            if (f != string.Empty)
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                        else
                                        {
                                            string a = reader.GetString(0); // Day
                                            string f = reader.GetString(2); // Other
                                            string g = reader.GetString(10); // Stime
                                            string h = reader.GetString(11); // Etime

                                            if (f != string.Empty)
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            reader.Close();
                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Ins_id='" + cbInstructor.SelectedValue.ToString() + "'AND Sched_Sti < STR_TO_DATE('" + e_time + "', '%l:%i %p') and Sched_Eti >= STR_TO_DATE('" + e_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                string schedid = string.Empty;
                                while (reader.Read())
                                {
                                    schedid = reader[0].ToString();
                                }
                                reader.Close();
                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                using (reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader[1] != DBNull.Value)
                                            {
                                                string a = reader.GetString(0); // Day
                                                string e = reader.GetString(1); // Type
                                                string f = reader.GetString(2); // Other
                                                string b = reader.GetString(6); // Course
                                                string c = reader.GetString(7); // Section
                                                string d = reader.GetString(8); // Subject
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                            else
                                            {
                                                string a = reader.GetString(0); // Day
                                                string f = reader.GetString(2); // Other
                                                string g = reader.GetString(10); // Stime
                                                string h = reader.GetString(11); // Etime

                                                if (f != string.Empty)
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There's a conflict with the Instructor's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {//SECTION
                                reader.Close();
                                cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Course_id='" + CourseId + "'And Sec_id='" + cbYrSection.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    string schedid = string.Empty;
                                    while (reader.Read())
                                    {
                                        schedid = reader[0].ToString();
                                    }
                                    reader.Close();
                                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                    using (reader = cmd.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            while (reader.Read())
                                            {
                                                if (reader[1] != DBNull.Value)
                                                {
                                                    string a = reader.GetString(0); // Day
                                                    string e = reader.GetString(1); // Type
                                                    string f = reader.GetString(2); // Other
                                                    string b = reader.GetString(6); // Course
                                                    string c = reader.GetString(7); // Section
                                                    string d = reader.GetString(8); // Subject
                                                    string g = reader.GetString(10); // Stime
                                                    string h = reader.GetString(11); // Etime

                                                    if (f != string.Empty)
                                                    {
                                                        MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                                else
                                                {
                                                    string a = reader.GetString(0); // Day
                                                    string f = reader.GetString(2); // Other
                                                    string g = reader.GetString(10); // Stime
                                                    string h = reader.GetString(11); // Etime

                                                    if (f != string.Empty)
                                                    {
                                                        MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    reader.Close();
                                    cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Course_id='" + CourseId + "'And Sec_id='" + cbYrSection.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                    reader = cmd.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        string schedid = string.Empty;
                                        while (reader.Read())
                                        {
                                            schedid = reader[0].ToString();
                                        }
                                        reader.Close();
                                        cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                        using (reader = cmd.ExecuteReader())
                                        {
                                            if (reader.HasRows)
                                            {
                                                while (reader.Read())
                                                {
                                                    if (reader[1] != DBNull.Value)
                                                    {
                                                        string a = reader.GetString(0); // Day
                                                        string e = reader.GetString(1); // Type
                                                        string f = reader.GetString(2); // Other
                                                        string b = reader.GetString(6); // Course
                                                        string c = reader.GetString(7); // Section
                                                        string d = reader.GetString(8); // Subject
                                                        string g = reader.GetString(10); // Stime
                                                        string h = reader.GetString(11); // Etime

                                                        if (f != string.Empty)
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string a = reader.GetString(0); // Day
                                                        string f = reader.GetString(2); // Other
                                                        string g = reader.GetString(10); // Stime
                                                        string h = reader.GetString(11); // Etime

                                                        if (f != string.Empty)
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("There's a conflict with the Section's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {//Room
                                        reader.Close();
                                        cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Room_id='" + cbRoom.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                        reader = cmd.ExecuteReader();
                                        if (reader.HasRows)
                                        {
                                            string schedid = string.Empty;
                                            while (reader.Read())
                                            {
                                                schedid = reader[0].ToString();
                                            }
                                            reader.Close();
                                            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                            using (reader = cmd.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        if (reader[1] != DBNull.Value)
                                                        {
                                                            string a = reader.GetString(0); // Day
                                                            string e = reader.GetString(1); // Type
                                                            string f = reader.GetString(2); // Other
                                                            string b = reader.GetString(6); // Course
                                                            string c = reader.GetString(7); // Section
                                                            string d = reader.GetString(8); // Subject
                                                            string g = reader.GetString(10); // Stime
                                                            string h = reader.GetString(11); // Etime

                                                            if (f != string.Empty)
                                                            {
                                                                MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string a = reader.GetString(0); // Day
                                                            string f = reader.GetString(2); // Other
                                                            string g = reader.GetString(10); // Stime
                                                            string h = reader.GetString(11); // Etime

                                                            if (f != string.Empty)
                                                            {
                                                                MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            reader.Close();
                                            cmd = new MySqlCommand("SELECT *,TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM `tblsched` WHERE Sched_Day = '" + cbDay.SelectedValue.ToString() + "'AND Room_id='" + cbRoom.SelectedValue.ToString() + "'AND Sched_Sti <= STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sched_Eti > STR_TO_DATE('" + s_time + "', '%l:%i %p') AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'", con);
                                            reader = cmd.ExecuteReader();
                                            if (reader.HasRows)
                                            {
                                                string schedid = string.Empty;
                                                while (reader.Read())
                                                {
                                                    schedid = reader[0].ToString();
                                                }
                                                reader.Close();
                                                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name, TIME_FORMAT(Sched_Sti, '%h:%i %p') as 'STime',TIME_FORMAT(Sched_Eti, '%h:%i %p') as ETime FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sched_id='" + schedid + "'", con);
                                                using (reader = cmd.ExecuteReader())
                                                {
                                                    if (reader.HasRows)
                                                    {
                                                        while (reader.Read())
                                                        {
                                                            if (reader[1] != DBNull.Value)
                                                            {
                                                                string a = reader.GetString(0); // Day
                                                                string e = reader.GetString(1); // Type
                                                                string f = reader.GetString(2); // Other
                                                                string b = reader.GetString(6); // Course
                                                                string c = reader.GetString(7); // Section
                                                                string d = reader.GetString(8); // Subject
                                                                string g = reader.GetString(10); // Stime
                                                                string h = reader.GetString(11); // Etime

                                                                if (f != string.Empty)
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + b + " - " + c + "\n" + d + "\n" + e + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string a = reader.GetString(0); // Day
                                                                string f = reader.GetString(2); // Other
                                                                string g = reader.GetString(10); // Stime
                                                                string h = reader.GetString(11); // Etime

                                                                if (f != string.Empty)
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("There's a conflict with the Room's schedule:\n\n" + a + " - " + f + "\n" + g + " - " + h, "ADDING FAILED", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {// SAVE
                                                TimeSpan tl = DateTime.ParseExact("8:00 PM", "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                                TimeSpan teT = DateTime.ParseExact(e_time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                                DateTime endtime = DateTime.Today.Add(teT);
                                                DateTime timelimit = DateTime.Today.Add(tl);
                                                if (endtime > timelimit)
                                                {
                                                    MessageBox.Show("Schedule overlaps the limit.", "FAILED", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                                }
                                                else
                                                {
                                                    con.Dispose();
                                                    string svins = cbInstructor.SelectedValue.ToString();
                                                    string svcou = CourseId;
                                                    string svys = SecId;
                                                    string svsub = cbSubject.SelectedValue.ToString();
                                                    string svtype = cbmType.SelectedValue.ToString();
                                                    string svroom = cbRoom.SelectedValue.ToString();
                                                    string svday = cbDay.SelectedValue.ToString();
                                                    DateTime svSt = DateTime.Parse(s_time);
                                                    DateTime svEt = DateTime.Parse(e_time);
                                                    int svrow = Row(cbStime.SelectedIndex);
                                                    Duration_Calc();
                                                    try
                                                    {
                                                        cmd = new MySqlCommand("INSERT INTO `tblsched`(`Ins_id`, `Course_id`, `Sec_id`, `Sub_id`, `Sub_type`, `Room_id`, `Sched_Day`, `Sched_Sti`, `Sched_Eti`, `Sched_row`, `Sched_Dur`, `Sem_id`, `Sy_id`)"
                                                                           + "VALUES (@ins,@cou,@ys,@sub,@type,@room,@day,@sti,@eti,@row,@dur,@sem,@sy)", con);
                                                        cmd.Parameters.AddWithValue("@ins", svins);
                                                        cmd.Parameters.AddWithValue("@cou", svcou);
                                                        cmd.Parameters.AddWithValue("@ys", svys);
                                                        cmd.Parameters.AddWithValue("@sub", svsub);
                                                        cmd.Parameters.AddWithValue("@type", svtype);
                                                        cmd.Parameters.AddWithValue("@room", svroom);
                                                        cmd.Parameters.AddWithValue("@day", svday);
                                                        cmd.Parameters.AddWithValue("@sti", svSt.ToString("HH:mm"));
                                                        cmd.Parameters.AddWithValue("@eti", svEt.ToString("HH:mm"));
                                                        cmd.Parameters.AddWithValue("@row", svrow);
                                                        cmd.Parameters.AddWithValue("@dur", duration);
                                                        cmd.Parameters.AddWithValue("@sem", semester);
                                                        cmd.Parameters.AddWithValue("@sy", schoolyear);

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                        con.Dispose();

                                                        MessageBox.Show("The Schedule has been added successfully.");
                                                        Fill_Sched();
                                                        Reset_control();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show(ex.ToString());
                                                        throw;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void BtnAddSched_Click(object sender, RoutedEventArgs e)
        {
            Conflict_Checker();
        }

        void plotrow()
        {
            switch (cbStime.SelectedIndex)
            {
                case 0:
                    Grid.SetRow(bdr, 0);
                    break;
                case 1:
                    Grid.SetRow(bdr, 1);
                    break;
                case 2:
                    Grid.SetRow(bdr, 2);
                    break;
                case 3:
                    Grid.SetRow(bdr, 3);
                    break;
                case 4:
                    Grid.SetRow(bdr, 4);
                    break;
                case 5:
                    Grid.SetRow(bdr, 5);
                    break;
                case 6:
                    Grid.SetRow(bdr, 6);
                    break;
                case 7:
                    Grid.SetRow(bdr, 7);
                    break;
                case 8:
                    Grid.SetRow(bdr, 8);
                    break;
                case 9:
                    Grid.SetRow(bdr, 9);
                    break;
                case 10:
                    Grid.SetRow(bdr, 10);
                    break;
                case 11:
                    Grid.SetRow(bdr, 11);
                    break;
                case 12:
                    Grid.SetRow(bdr, 12);
                    break;
                case 13:
                    Grid.SetRow(bdr, 13);
                    break;
                case 14:
                    Grid.SetRow(bdr, 14);
                    break;
                case 15:
                    Grid.SetRow(bdr, 15);
                    break;
                case 16:
                    Grid.SetRow(bdr, 16);
                    break;
                case 17:
                    Grid.SetRow(bdr, 17);
                    break;
                case 18:
                    Grid.SetRow(bdr, 18);
                    break;
                case 19:
                    Grid.SetRow(bdr, 19);
                    break;
                case 20:
                    Grid.SetRow(bdr, 20);
                    break;
                case 21:
                    Grid.SetRow(bdr, 21);
                    break;
                case 22:
                    Grid.SetRow(bdr, 22);
                    break;
                case 23:
                    Grid.SetRow(bdr, 23);
                    break;
                case 24:
                    Grid.SetRow(bdr, 24);
                    break;
                case 25:
                    Grid.SetRow(bdr, 25);
                    break;
            }
        }

        void plotcol()
        {
            switch (cbDay.SelectedIndex)
            {
                case 0:
                    Grid.SetColumn(bdr, 0);
                    break;
                case 1:
                    Grid.SetColumn(bdr, 1);
                    break;
                case 2:
                    Grid.SetColumn(bdr, 2);
                    break;
                case 3:
                    Grid.SetColumn(bdr, 3);
                    break;
                case 4:
                    Grid.SetColumn(bdr, 4);
                    break;
                case 5:
                    Grid.SetColumn(bdr, 5);
                    break;
            }
        }

        void plotrowspan()
        {
            int hour = Int32.Parse(txtHour.Text);
            int min = Int32.Parse(txtMin.Text);
            int convertedmin = 0;
            if (min == 30)
            {
                convertedmin = 1;
            }
            int rowspn = (hour * 2) + (convertedmin);
            Grid.SetRowSpan(bdr, rowspn);
        }

        static int Row(int x)
        {
            int xrow = 0;
            switch (x)
            {
                case 0:
                    xrow = 0;
                    break;
                case 1:
                    xrow = 1;
                    break;
                case 2:
                    xrow = 2;
                    break;
                case 3:
                    xrow = 3;
                    break;
                case 4:
                    xrow = 4;
                    break;
                case 5:
                    xrow = 5;
                    break;
                case 6:
                    xrow = 6;
                    break;
                case 7:
                    xrow = 7;
                    break;
                case 8:
                    xrow = 8;
                    break;
                case 9:
                    xrow = 9;
                    break;
                case 10:
                    xrow = 10;
                    break;
                case 11:
                    xrow = 11;
                    break;
                case 12:
                    xrow = 12;
                    break;
                case 13:
                    xrow = 13;
                    break;
                case 14:
                    xrow = 14;
                    break;
                case 15:
                    xrow = 15;
                    break;
                case 16:
                    xrow = 16;
                    break;
                case 17:
                    xrow = 17;
                    break;
                case 18:
                    xrow = 18;
                    break;
                case 19:
                    xrow = 19;
                    break;
                case 20:
                    xrow = 20;
                    break;
                case 21:
                    xrow = 21;
                    break;
                case 22:
                    xrow = 22;
                    break;
                case 23:
                    xrow = 23;
                    break;
                case 24:
                    xrow = 24;
                    break;
                case 25:
                    xrow = 25;
                    break;
            }
            return xrow;
        }

        void ComboBox_Manage_Other()
        {
            cbOther.Items.Clear();
            foreach (DataRow row in dtcklst.Rows)
            {
                if ((bool)row["Check"] == false)
                {
                    cbOther.Items.Add(row["Other"].ToString());
                }
            }
        }

        void Other_Check()
        {
            if (chkOther.IsChecked == true)
            {
                txtCourse.Text = "";
                cbYrSection.SelectedIndex = -1;
                cbYrSection.Visibility = Visibility.Collapsed;

                rowdefmain.Height = new GridLength(0,GridUnitType.Pixel);

                cbOther.Visibility = Visibility.Visible;
                cbOther.IsEnabled = true;

                grdchklist.Height = new GridLength(1, GridUnitType.Auto);
                bdrChecklist.Visibility = Visibility.Visible;
                dtOther.Visibility = Visibility.Visible;
                Fill_Checklist_Other();
                ComboBox_Manage_Other();
            }
            else
            {
                txtCourse.Text = "Course/YrSec";
                cbYrSection.Visibility = Visibility.Visible;

                rowdefmain.Height = new GridLength(230, GridUnitType.Pixel);

                cbOther.Visibility = Visibility.Collapsed;
                cbOther.IsEnabled = false;
                cbOther.SelectedIndex = -1;
                if (cbDay.SelectedIndex != -1)
                {
                    cbDay.SelectedIndex = -1;
                }
                if (Int32.Parse(txtHour.Text) != 0 || Int32.Parse(txtMin.Text) != 0)
                {
                    txtHour.Text = "0";
                    scrollhour.Value = 0;
                    txtMin.Text = "0";
                    scrollmin.Value = 0;
                }
                grdchklist.Height = new GridLength(0, GridUnitType.Auto);
                bdrChecklist.Visibility = Visibility.Collapsed;
                dtOther.Visibility = Visibility.Collapsed;
            }
        }

        private void ChkOther_Click(object sender, RoutedEventArgs e)
        {
            Other_Check();
        }

        private void CbOther_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbOther.SelectedIndex != -1)
            {
                cbDay.SelectedIndex = -1;
                cbDay.IsEnabled = true;
            }
            else
            {
                cbDay.SelectedIndex = -1;
                cbDay.IsEnabled = false;
            }
        }

        #region Subject Checklist
        public void Fill_Checklist()
        {
            // get plotted subjects 
            if (exsubj != null)
            {
                exsubj.Clear();
            }
            MySqlConnection con = new MySqlConnection(Connection);
            cmd = new MySqlCommand("SELECT sched.Sub_id, sched.Sub_type, sched.Sched_Dur, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sec_id='" + SecId + "' AND sched.Sem_id = '" + semester + "' AND sched.Sy_id='" + schoolyear + "'", con);
            da = new MySqlDataAdapter(cmd);
            exsubj = new DataTable();
            da.Fill(exsubj);

            // checklist table
            if (dtcklst != null)
            {
                dtcklst.Clear();
            }
            if (pet != string.Empty)
            {
                cmd = new MySqlCommand("SELECT *,(Ch_lec + Ch_lab) as T_hours FROM tblopensub JOIN tblsubject USING(Sub_id) where tblopensub.Sem_id = '" + semester + "' AND tblopensub.Petition = 'PETITION' AND tblsubject.Course_id = '" + CourseId + "' AND tblopensub.Sy_id = '" + schoolyear + "'", con);
                da = new MySqlDataAdapter(cmd);
                dtcklst = new DataTable();
                da.Fill(dtcklst);
                var col = new DataColumn("Check", typeof(bool));
                col.DefaultValue = false;
                col.ReadOnly = false;
                dtcklst.Columns.Add(col);
            }
            else
            {
                cmd = new MySqlCommand("SELECT *,(Ch_lec + Ch_lab) as T_hours FROM tblopensub JOIN tblsubject USING (Sub_id) where tblopensub.Sem_id = '" + semester + "' AND tblsubject.Year='" + year + "'AND tblsubject.Course_id ='" + CourseId + "' AND tblopensub.Petition <> 'PETITION' AND tblopensub.Sy_id='" + schoolyear + "'", con);
                da = new MySqlDataAdapter(cmd);
                dtcklst = new DataTable();
                da.Fill(dtcklst);
                var col = new DataColumn("Check", typeof(bool));
                col.DefaultValue = false;
                col.ReadOnly = false;
                dtcklst.Columns.Add(col);
            }

            foreach (DataRow rowex in exsubj.Rows)
            {
                string exsubjid = rowex["Sub_id"].ToString();
                foreach (DataRow rowchk in dtcklst.Rows)
                {
                    string chklstsubid = rowchk["Sub_id"].ToString();

                    if (chklstsubid == exsubjid)
                    {
                        if (rowex["Sub_type"].ToString() == "LEC")
                        {
                            rowchk["Ch_lec"] = double.Parse(rowchk["Ch_lec"].ToString()) - (double.Parse(rowex["Sched_Dur"].ToString()) / 2);
                        }
                        else if (rowex["Sub_type"].ToString() == "LAB")
                        {
                            rowchk["Ch_lab"] = double.Parse(rowchk["Ch_lab"].ToString()) - (double.Parse(rowex["Sched_Dur"].ToString()) / 2);
                        }
                        if (rowchk["Ch_lec"].ToString() == "0" && rowchk["Ch_lab"].ToString() == "0")
                        {
                            rowchk["Check"] = true;
                        }
                    }
                }
            }
            dtChecklist.ItemsSource = dtcklst.DefaultView;
        }

        private void CbInstructor_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbInstructor.Template.FindName("PART_EditableTextBox", cbInstructor) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbInstructor_GotFocus(object sender, RoutedEventArgs e)
        {
            cbInstructor.IsDropDownOpen = true;
        }

        private void CbRoom_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbRoom.Template.FindName("PART_EditableTextBox", cbRoom) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbRoom_GotFocus(object sender, RoutedEventArgs e)
        {
            cbRoom.IsDropDownOpen = true;
        }
        

        private void CbYrSection_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbYrSection.Template.FindName("PART_EditableTextBox", cbYrSection) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbYrSection_GotFocus(object sender, RoutedEventArgs e)
        {
            cbYrSection.IsDropDownOpen = true;
        }

        private void CbSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbSubject.Template.FindName("PART_EditableTextBox", cbSubject) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbSubject_GotFocus(object sender, RoutedEventArgs e)
        {
            cbSubject.IsDropDownOpen = true;
        }

        #endregion
        void Fill_Checklist_Other()
        {
            // EXISTING OTHER
            MySqlConnection con = new MySqlConnection(Connection);
            cmd = new MySqlCommand("SELECT Sched_id,Sub_Other,SUM(Sched_Dur) as Sched_Dur FROM tblsched WHERE Sub_Other != '' AND Ins_id = '" + cbInstructor.SelectedValue.ToString() + "' AND Sy_id = '" + schoolyear + "' AND Sem_id ='" + semester + "' GROUP BY Sub_Other", con);
            da = new MySqlDataAdapter(cmd);
            exother = new DataTable();
            da.Fill(exother);


            // CHECKLIST
            dtcklst = new DataTable();
            var col = new DataColumn("Check", typeof(bool));
            col.DefaultValue = false;
            col.ReadOnly = false;
            dtcklst.Columns.Add(col);
            dtcklst.Columns.Add("Other", typeof(string));
            dtcklst.Columns.Add("THours", typeof(double));
            dtcklst.Columns.Add("UHours", typeof(double));

            // Create a DataRow, add Name and Age data, and add to the DataTable
            DataRow dr = dtcklst.NewRow();
            dr["Other"] = "CONSULTATION HOUR"; // or dr[0]="Mohammad";
            dr["THours"] = 2; // or dr[1]=24;
            dr["UHours"] = 0;
            dtcklst.Rows.Add(dr);

            // Create another DataRow, add Name and Age data, and add to the DataTable
            dr = dtcklst.NewRow();
            dr["Other"] = "RESEARCH"; // or dr[0]="Shahnawaz";
            dr["THours"] = 3; // or dr[1]=24;
            dr["UHours"] = 0;
            dtcklst.Rows.Add(dr);

            // Create another DataRow, add Name and Age data, and add to the DataTable
            dr = dtcklst.NewRow();
            dr["Other"] = "EXTENSION"; // or dr[0]="Shahnawaz";
            dr["THours"] = 3; // or dr[1]=24;
            dr["UHours"] = 0;
            dtcklst.Rows.Add(dr);

            foreach (DataRow rowex in exother.Rows)
            {
                string exsubjid = rowex["Sub_Other"].ToString();
                foreach (DataRow rowchk in dtcklst.Rows)
                {
                    string chklstsubid = rowchk["Other"].ToString();

                    if (chklstsubid == exsubjid)
                    {
                        double usedhours = double.Parse(rowex["Sched_Dur"].ToString()) / 2;
                        rowchk["UHours"] = usedhours.ToString();
                        if (rowchk["THours"].ToString() == usedhours.ToString())
                        {
                            rowchk["Check"] = true;
                        }
                    }
                }
            }

            dtOther.ItemsSource = dtcklst.DefaultView;
        }
        
        void Load_Footer()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_id='" + semester + "'", con);
                con.Open();
                using (reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtCr.Text = reader[2].ToString();
                            txtCp.Text = reader[3].ToString();
                            txtCa.Text = reader[4].ToString();
                        }
                    }
                }
            }
        }
    }
}
