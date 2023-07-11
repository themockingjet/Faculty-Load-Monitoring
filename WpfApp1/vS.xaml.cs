﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for vS.xaml
    /// </summary>
    public partial class vS : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        string nl = System.Environment.NewLine;
        MySqlCommand cmd;
        MySqlDataReader reader;
        MySqlDataAdapter da;

        string semester = Main.semester_id;
        string schoolyear = Main.schoolyear_id;

        public vS()
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

            ComboBox_Category();
            ComboBox_Ins();
            ComboBox_Room();
            ComboBox_Course();

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

        private void BtnManage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnManage.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void BtnManage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnManage.Foreground = new SolidColorBrush(Colors.White);
        }

        private void BtnManage_Click(object sender, RoutedEventArgs e)
        {
            mS win = new mS();
            win.Show();
            this.Close();
        }

        // ================== Main ==============================
        string day = string.Empty;
        Border bdr;
        TextBlock txt;
        DataTable dt;
        string ldday = string.Empty;
        string comcourse = string.Empty;
        string year = string.Empty;
        string pet = string.Empty;

        void Border_Init()
        {
            bdr = new Border();
            bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            bdr.BorderThickness = new Thickness(2);
            bdr.Margin = new Thickness(-1);
            bdr.Background = new SolidColorBrush(Colors.White);

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
                if (cbIns.SelectedIndex != -1 && cbRm.SelectedIndex == -1 && cbCourse.SelectedIndex == -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE room.Room_id IN (SELECT Room_id FROM tblopenrooms) AND sched.Ins_id ='" + cbIns.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Instructor_Sched();
                            }
                        }
                    }
                }
                else if (cbIns.SelectedIndex == -1 && cbRm.SelectedIndex != -1 && cbCourse.SelectedIndex == -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND Room_id ='" + cbRm.SelectedValue.ToString() + "'AND Sem_id ='" + semester + "'AND Sy_id='" + schoolyear + "'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Room_Sched();
                            }
                        }
                    }
                }
                else if (cbIns.SelectedIndex == -1 && cbRm.SelectedIndex == -1 && cbCourse.SelectedIndex != -1 && cbYsec.SelectedIndex != -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND sched.Course_id ='" + cbCourse.SelectedValue.ToString() + "'AND sched.Sec_id ='" + cbYsec.SelectedValue.ToString() + "'AND Sem_id ='" + semester + "'AND Sy_id='" + schoolyear + "'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Section_Sched();
                            }
                        }
                    }
                }

            }
        }

        void Instructor_Sched()
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

                if (Int32.Parse(lddur) <= 4)
                {
                    txt = new TextBlock();
                    txt.Text = ldsub + " : " + ldtype + nl + ldcou + " - " + ldys + nl + ldroom;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
                else
                {
                    txt = new TextBlock();
                    txt.Text = ldys + nl + nl + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys + nl + ldroom;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
            }
            else
            {
                if (ldother == string.Empty)
                {
                    Border_Init();
                    Day_Init();
                    Grid.SetRow(bdr, Int32.Parse(ldrow));
                    Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                    txt = new TextBlock();
                    txt.Text = ldsub + " : " + ldtype + nl + ldcou + " - " + ldys + nl + ldroom;
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
                    txt.Text = ldother;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
            }
        }

        void Room_Sched()
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
                txt.Text = ldins + nl + ldsub + " : " + ldtype + nl + ldcou + " - " + ldys;
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
        
        void Section_Sched()
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

                if (Int32.Parse(lddur) <= 4)
                {
                    txt = new TextBlock();
                    txt.Text = ldins + nl + ldsub + " : " + ldtype + nl +  ldroom;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
                else
                {
                    txt = new TextBlock();
                    txt.Text = ldins + nl + ldys + nl + nl + ldsub + " : " + ldtype + nl + ldroom;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
            }
            else
            {
                if (ldother == string.Empty)
                {
                    Border_Init();
                    Day_Init();
                    Grid.SetRow(bdr, Int32.Parse(ldrow));
                    Grid.SetRowSpan(bdr, Int32.Parse(lddur));

                    txt = new TextBlock();
                    txt.Text = ldins + nl + ldsub + " : " + ldtype + nl + ldroom;
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
                    txt.Text = ldother;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.TextAlignment = TextAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    bdr.Child = txt;
                    grdSchedule.Children.Add(bdr);
                }
            }
        }

        void ComboBox_Category()
        {
            cbType.Items.Add("Instructors");
            cbType.Items.Add("Rooms");
            cbType.Items.Add("Sections");
        }

        void ComboBox_Ins()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT CONCAT(FName,' ',LName) as Ins_name,Ins_id from tblins LEFT JOIN tblsched USING (Ins_id) where tblins.Ins_id = tblsched.Ins_id AND tblsched.Sem_id = '" + semester + "'AND tblsched.Sy_id = '" + schoolyear + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbIns.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }
        
        void ComboBox_Room()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Room_name, Room_id FROM tblroom LEFT JOIN tblsched USING (Room_id) where tblroom.Room_id = tblsched.Room_id AND tblsched.Sem_id = '" + semester + "'AND tblsched.Sy_id = '" + schoolyear + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbRm.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        void ComboBox_Course()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Course_name,Course_id FROM tblcourse LEFT JOIN tblsched USING (Course_id) where tblcourse.Course_id = tblsched.Course_id AND tblsched.Sem_id = '" + semester + "'AND tblsched.Sy_id = '" + schoolyear + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbCourse.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        void ComboBox_Section()
        {
            cbYsec.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(IF(Sec_year ='0','',Sec_year),Sec_name) as Section FROM tblsection WHERE Course_id='" + cbCourse.SelectedValue.ToString() + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbYsec.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        bool ins = false;
        bool room = false;
        bool section = false;

        void section_bool()
        {
            if (cbCourse.SelectedIndex != -1 && cbYsec.SelectedIndex != -1)
            {
                 section = true;
                tblvSched.Text = Selcourse + " - " + selsection;
            }
            else if ( cbCourse.SelectedIndex == -1 || cbYsec.SelectedIndex == -1 )
            {
                 section = false;
                tblvSched.Text = "";
            }
        }
        
        private string Selins
        {
            get
            {
                var subject = cbIns.SelectedItem as DataRowView;
                string sub = subject.Row["Ins_name"].ToString();
                return sub;
            }
        }

        private string Selroom
        {
            get
            {
                var subject = cbRm.SelectedItem as DataRowView;
                string sub = subject.Row["Room_name"].ToString();
                return sub;
            }
        }

        private string Selcourse
        {
            get
            {
                var subject = cbCourse.SelectedItem as DataRowView;
                string sub = subject.Row["Course_name"].ToString();
                return sub;
            }
        }

        private string selsection
        {
            get
            {
                var subject = cbYsec.SelectedItem as DataRowView;
                string sub = subject.Row["Section"].ToString();
                return sub;
            }
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedIndex != -1)
            {
                if (cbType.SelectedValue.ToString() == "Instructors")
                {
                    cbYsec.SelectedIndex = -1;
                    cbCourse.SelectedIndex = -1;
                    cbRm.SelectedIndex = -1;
                    grdSchedule.Children.Clear();
                    bdrIns.Visibility = Visibility.Visible;
                    bdrRoom.Visibility = Visibility.Collapsed;
                    bdrSection.Visibility = Visibility.Collapsed;
                }
                else if (cbType.SelectedValue.ToString() == "Rooms")
                {
                    cbYsec.SelectedIndex = -1;
                    cbCourse.SelectedIndex = -1;
                    cbIns.SelectedIndex = -1;
                    grdSchedule.Children.Clear();
                    bdrIns.Visibility = Visibility.Collapsed;
                    bdrRoom.Visibility = Visibility.Visible;
                    bdrSection.Visibility = Visibility.Collapsed;
                }
                else if (cbType.SelectedValue.ToString() == "Sections")
                {
                    cbRm.SelectedIndex = -1;
                    cbIns.SelectedIndex = -1;
                    grdSchedule.Children.Clear();
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
                Fill_Sched();
                btnExcel.IsEnabled = true;
                ins = true;
                tblvSched.Text = Selins;
            }
            else
            {
                txtIns.Visibility = Visibility.Visible;
                btnExcel.IsEnabled = false;
                ins = false;
                tblvSched.Text = "";
            }
        }

        private void CbRm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRm.SelectedIndex != -1)
            {
                txtRoom.Visibility = Visibility.Collapsed;
                Fill_Sched();
                btnExcel.IsEnabled = true;
                room = true;
                tblvSched.Text = Selroom;
            }
            else
            {
                txtRoom.Visibility = Visibility.Visible;
                btnExcel.IsEnabled = false;
                room = false;
                tblvSched.Text = "";
            }
        }

        private void CbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCourse.SelectedIndex != -1)
            {
                txtCourse.Visibility = Visibility.Collapsed;
                cbYsec.SelectedIndex = -1;
                ComboBox_Section();
            }
            else
            {
                txtCourse.Visibility = Visibility.Visible;
            }
            section_bool();
        }

        private void CbYsec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbYsec.SelectedIndex != -1)
            {
                txtSection.Visibility = Visibility.Collapsed;
                Fill_Sched();
                btnExcel.IsEnabled = true;
            }
            else
            {
                txtSection.Visibility = Visibility.Visible;
                btnExcel.IsEnabled = false;
            }
            section_bool();
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Excel.Application oXL = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;

            if (ins == true)
            {
                int prep = 0;
                double contacthours = 0;
                int units = 0;
                string consultationday = string.Empty;

                MySqlConnection con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("select * from tblsched join tblsubject WHERE tblsubject.Sub_id = tblsched.Sub_id AND Ins_id ='" + cbIns.SelectedValue.ToString() + "'AND Sem_id ='" + semester + "'AND Sy_id='" + schoolyear + "'", con);
                
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Sub_type"].ToString() == "LAB")
                    {
                        units += int.Parse(row["Cu_lab"].ToString());
                    }
                    else if (row["Sub_type"].ToString() == "LEC")
                    {
                        units += int.Parse(row["Cu_lec"].ToString());
                    }

                }

                cmd = new MySqlCommand("SELECT GROUP_CONCAT( CONCAT (IF(sched.Sched_Day = 'THURSDAY', SUBSTRING(sched.Sched_Day,2,1), SUBSTRING(sched.Sched_Day,1,1)),' ',CONCAT(TIME_FORMAT(sched.Sched_Sti, '%l'),'-',CONCAT(TIME_FORMAT(sched.Sched_Eti, '%l'),TIME_FORMAT(sched.Sched_Eti, '%p')))) ORDER BY FIELD(sched.Sched_Day, 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY')) as Day from tblsched sched WHERE Ins_id ='" + cbIns.SelectedValue.ToString() + "'AND Sem_id ='" + semester + "'AND Sy_id ='" + schoolyear + "'AND Sub_Other='CONSULTATION HOUR'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string xcon = row["Day"].ToString();
                    if (xcon.Contains(",") == true)
                    {
                        consultationday = xcon.Replace(',', '/');
                    }
                    else
                    {
                        consultationday = xcon;
                    }
                }
                cmd = new MySqlCommand("select DISTINCT Sub_code,Sub_type from tblsched join tblsubject WHERE tblsched.Sub_id = tblsubject.Sub_id AND Ins_id ='" + cbIns.SelectedValue.ToString() + "'AND Sem_id ='" + semester + "'AND Sy_id='" + schoolyear + "'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    prep += 1;
                }

                object misvalue = System.Reflection.Missing.Value;

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = false;

                //Get a new workbook.
                oWB = oXL.Workbooks.Open("TEMPLATE");
                ((Excel.Worksheet)oWB.Sheets[1]).Select();
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[9, 2] = Selins;
                oSheet.Cells[10, 2] = prep.ToString();
                oSheet.Cells[11, 2] = contacthours/2;
                oSheet.Cells[10, 6] = units;
                oSheet.Cells[11, 6] = consultationday;



                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Ins_id ='" + cbIns.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    switch (int.Parse(row["Sched_row"].ToString()))
                    {
                        case 0:
                            printrow = 14;
                            break;
                        case 1:
                            printrow = 15;
                            break;
                        case 2:
                            printrow = 16;
                            break;
                        case 3:
                            printrow = 17;
                            break;
                        case 4:
                            printrow = 18;
                            break;
                        case 5:
                            printrow = 19;
                            break;
                        case 6:
                            printrow = 20;
                            break;
                        case 7:
                            printrow = 21;
                            break;
                        case 8:
                            printrow = 22;
                            break;
                        case 9:
                            printrow = 23;
                            break;
                        case 10:
                            printrow = 24;
                            break;
                        case 11:
                            printrow = 25;
                            break;
                        case 12:
                            printrow = 26;
                            break;
                        case 13:
                            printrow = 27;
                            break;
                        case 14:
                            printrow = 28;
                            break;
                        case 15:
                            printrow = 29;
                            break;
                        case 16:
                            printrow = 30;
                            break;
                        case 17:
                            printrow = 31;
                            break;
                        case 18:
                            printrow = 32;
                            break;
                        case 19:
                            printrow = 33;
                            break;
                        case 20:
                            printrow = 34;
                            break;
                        case 21:
                            printrow = 35;
                            break;
                        case 22:
                            printrow = 36;
                            break;
                        case 23:
                            printrow = 37;
                            break;
                        case 24:
                            printrow = 38;
                            break;
                        case 25:
                            printrow = 39;
                            break;
                    }
                    switch (row["Sched_Day"].ToString())
                    {
                        case "MONDAY":
                            printcol = "B";
                            break;
                        case "TUESDAY":
                            printcol = "C";
                            break;
                        case "WEDNESDAY":
                            printcol = "D";
                            break;
                        case "THURSDAY":
                            printcol = "E";
                            break;
                        case "FRIDAY":
                            printcol = "F";
                            break;
                        case "SATURDAY":
                            printcol = "G";
                            break;
                    }

                    int dur = int.Parse(row["Sched_Dur"].ToString()) -1;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Style = "Normal 2";
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Merge();
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Font.Size = 10;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).WrapText = true;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).BorderAround2(Weight:Excel.XlBorderWeight.xlMedium);
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                    if (row["Sub_Other"].ToString() != string.Empty)
                    {
                        oSheet.Cells[printrow, printcol] = row["Sub_Other"].ToString();
                    }
                    else
                    {
                        oSheet.Cells[printrow, printcol] = row["Sub_code"].ToString() + " : " + row["Sub_type"].ToString() + "\n" + row["Course_name"].ToString() + " - " + row["Section"].ToString() + "\n" + row["Room_name"].ToString();
                    }

                }

                cmd = new MySqlCommand("WITH CTE AS (SELECT sched.Sched_id, sub.Sub_code, sub.Sub_desc, CONCAT(cou.Course_name,'-',CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name)) AS Section, GROUP_CONCAT( CONCAT (IF(sched.Sched_Day = 'THURSDAY', SUBSTRING(sched.Sched_Day,1,2), SUBSTRING(sched.Sched_Day,1,1)),' ',CONCAT(TIME_FORMAT(sched.Sched_Sti, '%l'),'-',CONCAT(TIME_FORMAT(sched.Sched_Eti, '%l'),TIME_FORMAT(sched.Sched_Eti, '%p')))) ORDER BY FIELD(sched.Sched_Day, 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY')) as Day, sched.Sched_Sti, sched.Sched_Eti, ROW_NUMBER() OVER (PARTITION BY sub.Sub_code ORDER BY sched.Sec_id) rn FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Ins_id ='" + cbIns.SelectedValue.ToString() + "' AND sched.Sem_id ='" + semester + "' AND sched.Sy_id='" + schoolyear + "'AND sched.Sub_id != '' GROUP BY sub.Sub_code,sched.Sec_id)"
                                     + "SELECT CASE WHEN rn = 1 THEN Sub_code END AS Sub_code, CASE WHEN rn = 1 THEN Sub_desc END AS Sub_desc, Section, Day FROM CTE GROUP BY Section,rn", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                int rowfooter = 42;
                int colfooter = 1;
                foreach (DataRow row in dt.Rows)
                {
                    if(row["Section"] != null)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            string colname = col.ColumnName;
                            if (colname == "Sub_code")
                            {
                                oSheet.Cells[rowfooter, colfooter] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "Sub_desc")
                            {
                                Excel.Range r1 = oSheet.Cells[rowfooter, colfooter + 1];
                                Excel.Range r2 = oSheet.Cells[rowfooter, colfooter + 3];
                                oSheet.get_Range(r1, r2).Merge();
                                oSheet.Cells[rowfooter, colfooter + 1] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter + 1].ShrinkToFit = true;
                                oSheet.get_Range(r1, r2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "Section")
                            {
                                oSheet.Cells[rowfooter, colfooter + 4].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 4] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "Day")
                            {
                                string daytostring = row[colname].ToString();
                                daytostring = daytostring.Replace(',', '/');
                                oSheet.Cells[rowfooter, colfooter + 5] = daytostring;
                                oSheet.Cells[rowfooter, colfooter + 5].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                                oSheet.Cells[rowfooter, colfooter + 6].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                        }
                        rowfooter += 1;
                    }
                    else
                    {
                        rowfooter += 1;
                    }
                }
                cmd = new MySqlCommand("SELECT Sched_id, Sub_Other, GROUP_CONCAT( CONCAT (IF(Sched_Day = 'THURSDAY', SUBSTRING(Sched_Day,1,2), SUBSTRING(Sched_Day,1,1)),' ',CONCAT(TIME_FORMAT(Sched_Sti, '%l'),'-',CONCAT(TIME_FORMAT(Sched_Eti, '%l'),TIME_FORMAT(Sched_Eti, '%p')))) ORDER BY FIELD(Sched_Day, 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY')) as Day, ROW_NUMBER() OVER (PARTITION BY Sub_Other ORDER BY Sched_id) rn FROM tblsched WHERE Ins_id ='" + cbIns.SelectedValue.ToString() + "' AND Sem_id ='" + semester + "' AND Sy_id='" + schoolyear + "' AND Sub_Other != 'CONSULTATION HOUR' AND Sub_Other !='' GROUP BY Sub_Other", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        string colname = col.ColumnName;
                        if (colname == "Sub_Other")
                        {

                            oSheet.Cells[rowfooter, colfooter].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                            oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                            oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                            Excel.Range r1 = oSheet.Cells[rowfooter, colfooter + 1];
                            Excel.Range r2 = oSheet.Cells[rowfooter, colfooter + 3];
                            oSheet.get_Range(r1, r2).Merge();
                            oSheet.Cells[rowfooter, colfooter + 1] = row[colname].ToString();
                            oSheet.get_Range(r1, r2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                            oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                            oSheet.Cells[rowfooter, colfooter + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                            oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                        }
                        if (colname == "Day")
                        {
                            string daytostring = row[colname].ToString();
                            daytostring = daytostring.Replace(',', '/');
                            oSheet.Cells[rowfooter, colfooter + 5] = daytostring;
                            oSheet.Cells[rowfooter, colfooter + 5].ShrinkToFit = true;
                            oSheet.Cells[rowfooter, colfooter + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                            oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                            oSheet.Cells[rowfooter, colfooter + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                            oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                        }
                    }
                    rowfooter += 1;
                }
                Excel.Range c1 = oSheet.Cells[rowfooter, 1];
                Excel.Range c2 = oSheet.Cells[rowfooter, 7];

                oSheet.get_Range(c1, c2).Interior.Color = System.Drawing.Color.FromArgb(64,49,81);
                oSheet.get_Range(c1, c2).Merge();
                oSheet.get_Range(c1, c2).BorderAround2(Weight: Excel.XlBorderWeight.xlMedium);


                oSheet.Cells[rowfooter + 2, 1] = "Prepared by:";
                oSheet.Cells[rowfooter + 2, 3] = "Recommending Approval:";
                oSheet.Cells[rowfooter + 2, 6] = "Approved by:";
                

                oSheet.Cells[rowfooter + 4, 1].EntireRow.Font.Bold = true;
                cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_id='" + semester + "'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    oSheet.Cells[rowfooter + 4, 1] = row["Sem_cr"].ToString();
                    oSheet.Cells[rowfooter + 4, 3] = row["Sem_cp"].ToString();
                    oSheet.Cells[rowfooter + 4, 6] = row["Sem_ca"].ToString();
                }
                oSheet.Cells[rowfooter + 5, 1].EntireRow.Font.FontStyle = "Italic";
                oSheet.Cells[rowfooter + 5, 1] = "Campus Registrar"; // Italic
                oSheet.Cells[rowfooter + 5, 3] = "Chairperson, Information Technology Dept.";
                oSheet.Cells[rowfooter + 5, 6] = "Campus Administrator";

                int rowconforme = rowfooter + 8;
                Excel.Range rc1 = oSheet.Cells[rowconforme, 1];
                Excel.Range rc2 = oSheet.Cells[rowconforme +5, 3];
                oSheet.get_Range(rc1, rc2).BorderAround(Excel.XlLineStyle.xlDouble, Excel.XlBorderWeight.xlThin);


                Excel.Range rcc1 = oSheet.Cells[rowconforme + 2, 1];
                Excel.Range rcc2 = oSheet.Cells[rowconforme + 2, 3];
                oSheet.get_Range(rcc1, rcc2).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                oSheet.get_Range(rcc1, rcc2).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                oSheet.Cells[rowconforme, 1].EntireRow.Font.Bold = true;
                oSheet.Cells[rowconforme, 1] = "CONFORME:";

                oSheet.Cells[rowconforme + 3, 1].EntireRow.Font.Bold = true;
                oSheet.Cells[rowconforme + 3, 1] = Selins;

                oSheet.Cells[rowconforme + 4, 1].EntireRow.Font.FontStyle = "Italic";
                oSheet.Cells[rowconforme + 4, 1] = "Instructor 1"; // Italic

            }
            else if(room == true)
            {
                object misvalue = System.Reflection.Missing.Value;

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = false;

                //Get a new workbook.
                oWB = oXL.Workbooks.Open("TEMPLATE");
                ((Excel.Worksheet)oWB.Sheets[3]).Select();
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[10, 1] = Selroom;



                MySqlConnection con = new MySqlConnection(Connection);
                con.Open();
                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Room_id ='" + cbRm.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    switch (int.Parse(row["Sched_row"].ToString()))
                    {
                        case 0:
                            printrow = 16;
                            break;
                        case 1:
                            printrow = 17;
                            break;
                        case 2:
                            printrow = 18;
                            break;
                        case 3:
                            printrow = 19;
                            break;
                        case 4:
                            printrow = 20;
                            break;
                        case 5:
                            printrow = 21;
                            break;
                        case 6:
                            printrow = 22;
                            break;
                        case 7:
                            printrow = 23;
                            break;
                        case 8:
                            printrow = 24;
                            break;
                        case 9:
                            printrow = 25;
                            break;
                        case 10:
                            printrow = 26;
                            break;
                        case 11:
                            printrow = 27;
                            break;
                        case 12:
                            printrow = 28;
                            break;
                        case 13:
                            printrow = 29;
                            break;
                        case 14:
                            printrow = 30;
                            break;
                        case 15:
                            printrow = 31;
                            break;
                        case 16:
                            printrow = 32;
                            break;
                        case 17:
                            printrow = 33;
                            break;
                        case 18:
                            printrow = 34;
                            break;
                        case 19:
                            printrow = 35;
                            break;
                        case 20:
                            printrow = 36;
                            break;
                        case 21:
                            printrow = 37;
                            break;
                        case 22:
                            printrow = 38;
                            break;
                        case 23:
                            printrow = 39;
                            break;
                        case 24:
                            printrow = 40;
                            break;
                        case 25:
                            printrow = 41;
                            break;
                    }
                    switch (row["Sched_Day"].ToString())
                    {
                        case "MONDAY":
                            printcol = "B";
                            break;
                        case "TUESDAY":
                            printcol = "C";
                            break;
                        case "WEDNESDAY":
                            printcol = "D";
                            break;
                        case "THURSDAY":
                            printcol = "E";
                            break;
                        case "FRIDAY":
                            printcol = "F";
                            break;
                        case "SATURDAY":
                            printcol = "G";
                            break;
                    }

                    int dur = int.Parse(row["Sched_Dur"].ToString()) - 1;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Style = "Normal 2";
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Merge();
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Font.Size = 10;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).WrapText = true;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).BorderAround2(Weight: Excel.XlBorderWeight.xlMedium);
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                    if (row["Sub_Other"].ToString() != string.Empty)
                    {
                        oSheet.Cells[printrow, printcol] = row["Sub_Other"].ToString();
                    }
                    else
                    {
                        oSheet.Cells[printrow, printcol] = row["LName"].ToString() + "\n" + row["Sub_code"].ToString() + " : " + row["Sub_type"].ToString() + "\n" + row["Course_name"].ToString() + " - " + row["Section"].ToString().ToString();
                    }

                }
                oSheet.Cells[44, 1] = "Prepared by:";
                oSheet.Cells[44, 6] = "Approved by:";


                oSheet.Cells[47, 1].EntireRow.Font.Bold = true;
                cmd = new MySqlCommand("SELECT * FROM tblsem WHERE Sem_id='" + semester + "'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    oSheet.Cells[47, 1] = row["Sem_cr"].ToString();
                    oSheet.Cells[47, 6] = row["Sem_ca"].ToString();
                }
                oSheet.Cells[48, 1].EntireRow.Font.FontStyle = "Italic";
                oSheet.Cells[48, 1] = "Campus Registrar"; // Italic
                oSheet.Cells[48, 6] = "Campus Administrator";
                
            }
            else if(section == true)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                

                object misvalue = System.Reflection.Missing.Value;

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = false;

                //Get a new workbook.
                oWB = oXL.Workbooks.Open("TEMPLATE");
                ((Excel.Worksheet)oWB.Sheets[2]).Select();
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[4, 1] = Selcourse + "-" + selsection;



                cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sec_id='" + cbYsec.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    switch (int.Parse(row["Sched_row"].ToString()))
                    {
                        case 0:
                            printrow = 7;
                            break;
                        case 1:
                            printrow = 8;
                            break;
                        case 2:
                            printrow = 9;
                            break;
                        case 3:
                            printrow = 10;
                            break;
                        case 4:
                            printrow = 11;
                            break;
                        case 5:
                            printrow = 12;
                            break;
                        case 6:
                            printrow = 13;
                            break;
                        case 7:
                            printrow = 14;
                            break;
                        case 8:
                            printrow = 15;
                            break;
                        case 9:
                            printrow = 16;
                            break;
                        case 10:
                            printrow = 17;
                            break;
                        case 11:
                            printrow = 18;
                            break;
                        case 12:
                            printrow = 19;
                            break;
                        case 13:
                            printrow = 20;
                            break;
                        case 14:
                            printrow = 21;
                            break;
                        case 15:
                            printrow = 22;
                            break;
                        case 16:
                            printrow = 23;
                            break;
                        case 17:
                            printrow = 24;
                            break;
                        case 18:
                            printrow = 25;
                            break;
                        case 19:
                            printrow = 26;
                            break;
                        case 20:
                            printrow = 27;
                            break;
                        case 21:
                            printrow = 28;
                            break;
                        case 22:
                            printrow = 29;
                            break;
                        case 23:
                            printrow = 30;
                            break;
                        case 24:
                            printrow = 31;
                            break;
                        case 25:
                            printrow = 32;
                            break;
                    }
                    switch (row["Sched_Day"].ToString())
                    {
                        case "MONDAY":
                            printcol = "B";
                            break;
                        case "TUESDAY":
                            printcol = "C";
                            break;
                        case "WEDNESDAY":
                            printcol = "D";
                            break;
                        case "THURSDAY":
                            printcol = "E";
                            break;
                        case "FRIDAY":
                            printcol = "F";
                            break;
                        case "SATURDAY":
                            printcol = "G";
                            break;
                    }

                    int dur = int.Parse(row["Sched_Dur"].ToString()) - 1;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Style = "Normal 2";
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Merge();
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).Font.Size = 10;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).WrapText = true;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).BorderAround2(Weight: Excel.XlBorderWeight.xlMedium);
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    oSheet.get_Range(printcol + printrow.ToString(), printcol + (printrow + dur).ToString()).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                    if (row["Sub_Other"].ToString() != string.Empty)
                    {
                        oSheet.Cells[printrow, printcol] = row["Sub_Other"].ToString();
                    }
                    else
                    {
                        oSheet.Cells[printrow, printcol] = row["Sub_code"].ToString() + " : " + row["Sub_type"].ToString() + "\n" + row["Course_name"].ToString() + " - " + row["Section"].ToString() + "\n" + row["Room_name"].ToString();
                    }
                }

                cmd = new MySqlCommand("SELECT sub.Sub_code, sub.Sub_desc, CONCAT(cou.Course_name,'-',CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name)) AS Section, GROUP_CONCAT( CONCAT (IF(sched.Sched_Day = 'THURSDAY', SUBSTRING(sched.Sched_Day,1,2), SUBSTRING(sched.Sched_Day,1,1)),' ',CONCAT(TIME_FORMAT(sched.Sched_Sti, '%l'),'-',CONCAT(TIME_FORMAT(sched.Sched_Eti, '%l'),TIME_FORMAT(sched.Sched_Eti, '%p')))) ORDER BY FIELD(sched.Sched_Day, 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY')) as Day, ins.LName, room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE sched.Sec_id='" + cbYsec.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "' GROUP BY sub.Sub_code ORDER BY sched.Sched_id", con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                int rowfooter = 35;
                int colfooter = 1;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Section"] != null)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            string colname = col.ColumnName;
                            if (colname == "Sub_code")
                            {

                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                                oSheet.Cells[rowfooter, colfooter + 1] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter + 1].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 1].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 1].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter + 1].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 1].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter + 1].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 1].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "Sub_desc")
                            {
                                Excel.Range r1 = oSheet.Cells[rowfooter, colfooter + 2];
                                Excel.Range r2 = oSheet.Cells[rowfooter, colfooter + 3];
                                oSheet.get_Range(r1, r2).Merge();
                                oSheet.Cells[rowfooter, colfooter + 2] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter + 2].ShrinkToFit = true;
                                oSheet.get_Range(r1, r2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.get_Range(r1, r2).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "Day")
                            {
                                string daytostring = row[colname].ToString();
                                daytostring = daytostring.Replace(',', '/');
                                oSheet.Cells[rowfooter, colfooter + 4] = daytostring;
                                oSheet.Cells[rowfooter, colfooter + 4].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 4].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "Room_name")
                            {
                                oSheet.Cells[rowfooter, colfooter + 5] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter + 5].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 5].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                            if (colname == "LName")
                            {
                                oSheet.Cells[rowfooter, colfooter + 6] = row[colname].ToString();
                                oSheet.Cells[rowfooter, colfooter + 6].ShrinkToFit = true;
                                oSheet.Cells[rowfooter, colfooter + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                oSheet.Cells[rowfooter, colfooter + 6].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                            }
                        }
                        rowfooter += 1;
                    }
                    else
                    {
                        rowfooter += 1;
                    }
                }
            }

            try
            {
                bool dialogResult =
                  oXL.Dialogs[Excel.XlBuiltInDialog.xlDialogPrint].Show(
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.FinalReleaseComObject(oSheet);

                oWB.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject(oWB);

                oXL.Quit();
                Marshal.FinalReleaseComObject(oXL);

            }
            catch (Exception)
            {

                throw;
            }
            Mouse.OverrideCursor = null;
        }        
        
        int printrow = 0;
        string printcol = string.Empty;

        private void CbIns_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbIns.Template.FindName("PART_EditableTextBox", cbIns) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbIns_GotFocus(object sender, RoutedEventArgs e)
        {
            cbIns.IsDropDownOpen = true; 
        }

        private void CbRm_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbRm.Template.FindName("PART_EditableTextBox", cbRm) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbRm_GotFocus(object sender, RoutedEventArgs e)
        {
            cbRm.IsDropDownOpen = true;
        }

        private void CbYsec_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbYsec.Template.FindName("PART_EditableTextBox", cbYsec) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbYsec_GotFocus(object sender, RoutedEventArgs e)
        {
            cbYsec.IsDropDownOpen = true;
        }
    }
}
