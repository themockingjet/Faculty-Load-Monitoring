using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Media;
using System.Windows.Markup;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mScheduleSubject.xaml
    /// </summary>
    public partial class mScheduleSubject : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";
        string cu_lec = "";
        string cu_lab = "";
        string year = "";
        string DayLab = "";
        string DayLec = "";
        Border bdr;
        int xrow = 27;
        int xrow1 = 27;
        int lab;

        
        public mScheduleSubject()
        {
            InitializeComponent();
            Load_Names();
            Load_Course();

            ScrollbarLec_hour();
            ScrollbarLec_Min();

            ScrollbarLab_hour();
            ScrollbarLab_Min();

            
        }

        void Border_Init()
        {
            bdr = new Border();
            bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            bdr.BorderThickness = new Thickness(1);
            bdr.Background = new SolidColorBrush(Colors.White);
        }

        void Select_Day_Lec()
        {
            if(cbDayLec.SelectedIndex == 0)
            {
                DayLec = "grdMONDAY";
            }
            if (cbDayLec.SelectedIndex == 1)
            {
                DayLec = "grdTUESDAY";
            }
            if (cbDayLec.SelectedIndex == 2)
            {
                DayLec = "grdWEDNESDAY";
            }
            if (cbDayLec.SelectedIndex == 3)
            {
                DayLec = "grdTHURSDAY";
            }
            if (cbDayLec.SelectedIndex == 4)
            {
                DayLec = "grdFRIDAY";
            }
            if (cbDayLec.SelectedIndex == 5)
            {
                DayLec = "grdSATURDAY";
            }
        }

        void Select_Day_Lab()
        {
            if (cbDayLab.SelectedIndex == 0)
            {
                DayLab = "grdMONDAY";
            }
            if (cbDayLab.SelectedIndex == 1)
            {
                DayLab = "grdTUESDAY";
            }
            if (cbDayLab.SelectedIndex == 2)
            {
                DayLab = "grdWEDNESDAY";
            }
            if (cbDayLab.SelectedIndex == 3)
            {
                DayLab = "grdTHURSDAY";
            }
            if (cbDayLab.SelectedIndex == 4)
            {
                DayLab = "grdFRIDAY";
            }
            if (cbDayLab.SelectedIndex == 5)
            {
                DayLab = "grdSATURDAY";
            }
        }

        void Start_Time_Lec()
        {
            if (cbStimeLec.SelectedValue.ToString() == "7:00 AM")
            {
                xrow = 0;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "7:30 AM")
            {
                xrow = 1;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "8:00 AM")
            {
                xrow = 2;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "8:30 AM")
            {
                xrow = 3;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "9:00 AM")
            {
                xrow = 4;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "9:30 AM")
            {
                xrow = 5;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "10:00 AM")
            {
                xrow = 6;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "10:30 AM")
            {
                xrow = 7;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "11:00 AM")
            {
                xrow = 8;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "11:30 AM")
            {
                xrow = 9;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "12:00 NN")
            {
                xrow = 10;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "12:30 NN")
            {
                xrow = 11;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "1:00 PM")
            {
                xrow = 12;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "1:30 PM")
            {
                xrow = 13;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "2:00 PM")
            {
                xrow = 14;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "2:30 PM")
            {
                xrow = 15;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "3:00 PM")
            {
                xrow = 16;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "3:30 PM")
            {
                xrow = 17;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "4:00 PM")
            {
                xrow = 18;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "4:30 PM")
            {
                xrow = 19;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "5:00 PM")
            {
                xrow = 20;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "5:30 PM")
            {
                xrow = 21;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "6:00 PM")
            {
                xrow = 22;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "6:30 PM")
            {
                xrow = 23;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "7:00 PM")
            {
                xrow = 24;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "7:30 PM")
            {
                xrow = 25;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "8:00 PM")
            {
                xrow = 26;
            }
        }

        void Start_Time_Lab()
        {
            if (cbStimeLec.SelectedValue.ToString() == "7:00 AM")
            {
                xrow1 = 0;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "7:30 AM")
            {
                xrow1 = 1;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "8:00 AM")
            {
                xrow1 = 2;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "8:30 AM")
            {
                xrow1 = 3;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "9:00 AM")
            {
                xrow1 = 4;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "9:30 AM")
            {
                xrow1 = 5;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "10:00 AM")
            {
                xrow1 = 6;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "10:30 AM")
            {
                xrow1 = 7;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "11:00 AM")
            {
                xrow1 = 8;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "11:30 AM")
            {
                xrow1 = 9;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "12:00 NN")
            {
                xrow1 = 10;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "12:30 NN")
            {
                xrow1 = 11;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "1:00 PM")
            {
                xrow1 = 12;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "1:30 PM")
            {
                xrow1 = 13;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "2:00 PM")
            {
                xrow1 = 14;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "2:30 PM")
            {
                xrow1 = 15;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "3:00 PM")
            {
                xrow1 = 16;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "3:30 PM")
            {
                xrow1 = 17;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "4:00 PM")
            {
                xrow1 = 18;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "4:30 PM")
            {
                xrow1 = 19;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "5:00 PM")
            {
                xrow1 = 20;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "5:30 PM")
            {
                xrow1 = 21;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "6:00 PM")
            {
                xrow1 = 22;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "6:30 PM")
            {
                xrow1 = 23;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "7:00 PM")
            {
                xrow1 = 24;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "7:30 PM")
            {
                xrow1 = 25;
            }
            else if (cbStimeLec.SelectedValue.ToString() == "8:00 PM")
            {
                xrow1 = 26;
            }
        }

        void Load_Subjects()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblopensub JOIN tblsubject USING (Sub_code) where tblsubject.Year='" + year + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cbSubject.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        void Load_Course()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblcourse", con))
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

        void Load_Section()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblsection where Course='" + cbCourse.SelectedValue.ToString() + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string yr = dr.GetString(1);
                        string sec = dr.GetString(2);
                        cbYrSection.Items.Add(yr + sec);
                    }
                }
            }
        }

        void Load_Names()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    if ((window as Schedule).manage_instructor_name != null && (window as Schedule).manage_room_name != null)
                    {
                        tblIns_name.Text = (window as Schedule).manage_instructor_name;
                        tblRoom_name.Text = (window as Schedule).manage_room_name;
                    }
                }
            }
        }

        void ComboBox_Day()
        {
            if (lab == 0)
            {
                cbDayLec.Items.Add("MONDAY");
                cbDayLec.Items.Add("TUESDAY");
                cbDayLec.Items.Add("WEDNESDAY");
                cbDayLec.Items.Add("THURSDAY");
                cbDayLec.Items.Add("FRIDAY");
                cbDayLec.Items.Add("SATURDAY");
            }
            else
            {
                cbDayLec.Items.Add("MONDAY");
                cbDayLec.Items.Add("TUESDAY");
                cbDayLec.Items.Add("WEDNESDAY");
                cbDayLec.Items.Add("THURSDAY");
                cbDayLec.Items.Add("FRIDAY");
                cbDayLec.Items.Add("SATURDAY");

                cbDayLab.Items.Add("MONDAY");
                cbDayLab.Items.Add("TUESDAY");
                cbDayLab.Items.Add("WEDNESDAY");
                cbDayLab.Items.Add("THURSDAY");
                cbDayLab.Items.Add("FRIDAY");
                cbDayLab.Items.Add("SATURDAY");
            }
        }

        void ComboBox_Hour_S()
        {
            cbStimeLec.Items.Add("7:00 AM");
            cbStimeLec.Items.Add("7:30 AM");
            cbStimeLec.Items.Add("8:00 AM");
            cbStimeLec.Items.Add("8:30 AM");
            cbStimeLec.Items.Add("9:00 AM");
            cbStimeLec.Items.Add("9:30 AM");
            cbStimeLec.Items.Add("10:00 AM");
            cbStimeLec.Items.Add("10:30 AM");
            cbStimeLec.Items.Add("11:00 AM");
            cbStimeLec.Items.Add("11:30 AM");
            cbStimeLec.Items.Add("12:00 NN");
            cbStimeLec.Items.Add("12:30 NN");
            cbStimeLec.Items.Add("1:00 PM");
            cbStimeLec.Items.Add("1:30 PM");
            cbStimeLec.Items.Add("2:00 PM");
            cbStimeLec.Items.Add("2:30 PM");
            cbStimeLec.Items.Add("3:00 PM");
            cbStimeLec.Items.Add("3:30 PM");
            cbStimeLec.Items.Add("4:00 PM");
            cbStimeLec.Items.Add("4:30 PM");
            cbStimeLec.Items.Add("5:00 PM");
            cbStimeLec.Items.Add("5:30 PM");
            cbStimeLec.Items.Add("6:00 PM");
            cbStimeLec.Items.Add("6:30 PM");
            cbStimeLec.Items.Add("7:00 PM");
            cbStimeLec.Items.Add("7:30 PM");
            cbStimeLec.Items.Add("8:00 PM");
        }

        void ScrollbarLec_hour()
        {
            scrollhourLec.Minimum = 1;
            scrollhourLec.Maximum = 13;
            scrollhourLec.SmallChange = 1;
        }

        void ScrollbarLec_Min()
        {
            scrollminLec.Minimum = 0;
            scrollminLec.Maximum = 30;
            scrollminLec.SmallChange = 30;
        }

        void ScrollbarLab_hour()
        {
            scrollhourLab.Minimum = 1;
            scrollhourLab.Maximum = 13;
            scrollhourLab.SmallChange = 1;
        }

        void ScrollbarLab_Min()
        {
            scrollminLab.Minimum = 0;
            scrollminLab.Maximum = 30;
            scrollminLab.SmallChange = 30;
        }
        
        void Border_Single()
        {
            tblIns.Text = tblIns_name.Text;
            if (cbSubject.SelectedIndex != -1)
            {
                tblSub.Text = cbSubject.SelectedValue.ToString();
                tblType.Text = "LEC";
            }
            else
            {
                tblSub.Text = "";
                tblType.Text = "";
            }
            tblYrsec.Text = cbCourse.SelectedValue.ToString() + " - " + cbYrSection.SelectedValue.ToString();
            tblRm.Text = tblRoom_name.Text;
        }
        
        void Border_Double()
        {
            if(cbSubject.SelectedIndex != -1)
            {
                tblSubD1.Text = cbSubject.SelectedValue.ToString();
                tblTypeD1.Text = "LEC";

                tblSubD2.Text = cbSubject.SelectedValue.ToString();
                tblTypeD2.Text = "LAB";
            }
            else
            {
                tblSubD1.Text = "";
                tblTypeD1.Text = "";

                tblSubD2.Text = "";
                tblTypeD2.Text = "";
            }

            tblInsD1.Text = tblIns_name.Text;
            tblYrsecD1.Text = cbCourse.SelectedValue.ToString() + " - " + cbYrSection.SelectedValue.ToString();
            tblRmD1.Text = tblRoom_name.Text;

            tblInsD2.Text = tblIns_name.Text;
            tblYrsecD2.Text = cbCourse.SelectedValue.ToString() + " - " + cbYrSection.SelectedValue.ToString();
            tblRmD2.Text = tblRoom_name.Text;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    (window as Schedule).grdcontent.Children.Remove(this);
                }
            }
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            lab = new Int32();
            lab = Int32.Parse(cu_lab);
            if (lab == 0)
            {
                Border_Init();
            }
            else
            {
                Border_Init();
                //Grid grid = new Grid();
                //grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                //grid.VerticalAlignment = VerticalAlignment.Stretch;

                //TextBlock txt1 = new TextBlock();
                //txt1.Text = "2005";
                //txt1.FontSize = 20;
                //txt1.FontWeight = FontWeights.Bold;
                //grid.Children.Add(txt1);
                //bdr.Child = grid;
                //Border newControl = new Border { DataContext = bdrDouble1.DataContext };
                Border copy = XamlReader.Parse(XamlWriter.Save(bdrDouble1)) as Border;
                Grid.SetRow(copy, 10);
                Grid.SetRowSpan(copy, 8);


                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Schedule))
                    {
                        if ((window as Schedule).manage_instructor_name != null && (window as Schedule).manage_room_name != null)
                        {
                            (window as Schedule).grdMONDAY.Children.Add(copy);
                        }
                    }
                }
            }
        }

        private void ScrollhourLec_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtHourLec.Text = scrollhourLec.Value.ToString();
        }

        private void ScrollminLec_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtMinLec.Text = scrollminLec.Value.ToString();
        }

        private void ScrollminLab_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtMinLab.Text = scrollminLab.Value.ToString();
        }

        private void ScrollhourLab_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtHourLab.Text = scrollhourLab.Value.ToString();
        }

        private void CbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load_Section();
        }

        private void CbYrSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string x = cbYrSection.SelectedValue.ToString();
            int yrs = Int32.Parse(x.Substring(0,1));
            if (yrs == 1)
            {
                year = "FIRST";
            }
            else if (yrs == 2)
            {
                year = "SECOND";
            }
            else if (yrs == 3)
            {
                year = "THIRD";
            }
            else if (yrs == 4)
            {
                year = "FOURTH";
            }
            else if (yrs == 5)
            {
                year = "FIFTH";
            }
            cbSubject.SelectedIndex = -1;
            Load_Subjects();
        }

        private void CbSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSubject.SelectedValue != null)
            {
                using (MySqlConnection con = new MySqlConnection(Connection))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from tblsubject where Sub_code='" + cbSubject.SelectedValue.ToString() + "'", con);
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cu_lec = dr.GetString(3);
                            cu_lab = dr.GetString(4);
                        }
                    }
                }
            }
            lab = new Int32();
            lab = Int32.Parse(cu_lab);
            
            if(cbSubject.SelectedIndex != -1)
            {
                if (lab == 0)
                {
                    grdLec.Visibility = Visibility.Visible;
                    tblLec.Visibility = Visibility.Visible;
                    grdLab.Visibility = Visibility.Collapsed;
                    tblLab.Visibility = Visibility.Collapsed;
                    grdSingle.Visibility = Visibility.Visible;
                    grdDouble.Visibility = Visibility.Collapsed;
                    Border_Single();
                }
                else
                {
                    tblLec.Visibility = Visibility.Visible;
                    grdLec.Visibility = Visibility.Visible;
                    grdLab.Visibility = Visibility.Visible;
                    tblLab.Visibility = Visibility.Visible;
                    grdSingle.Visibility = Visibility.Collapsed;
                    grdDouble.Visibility = Visibility.Visible;
                    Border_Double();
                }
            }
            else
            {
                cbDayLec.SelectedIndex = -1;
                cbDayLab.SelectedIndex = -1;

                grdLec.Visibility = Visibility.Collapsed;
                tblLec.Visibility = Visibility.Collapsed;
                grdLab.Visibility = Visibility.Collapsed;
                tblLab.Visibility = Visibility.Collapsed;
                if(grdSingle.Visibility == Visibility.Visible & grdDouble.Visibility == Visibility.Collapsed)
                {
                    Border_Single();
                }
                else if (grdSingle.Visibility == Visibility.Collapsed & grdDouble.Visibility == Visibility.Visible)
                {
                    Border_Double();
                }
            }
            ComboBox_Day();
        }

        private void CbDayLec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDayLec.SelectedIndex != -1)
            {
                if (grdSingle.Visibility == Visibility.Visible & grdDouble.Visibility == Visibility.Collapsed)
                {
                    tblDay.Text = cbDayLec.SelectedValue.ToString();
                }
                else if (grdSingle.Visibility == Visibility.Collapsed & grdDouble.Visibility == Visibility.Visible)
                {
                    if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex == -1)
                    {
                        tblDayD1.Text = cbDayLec.SelectedValue.ToString();
                    }
                    else if (cbDayLec.SelectedIndex == -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD2.Text = cbDayLab.SelectedValue.ToString();
                    }
                    else if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD1.Text = cbDayLec.SelectedValue.ToString();
                        tblDayD2.Text = cbDayLab.SelectedValue.ToString();
                    }
                }
                Select_Day_Lec();
            }
            else
            {
                if (grdSingle.Visibility == Visibility.Visible & grdDouble.Visibility == Visibility.Collapsed)
                {
                    tblDay.Text = "";
                }
                else if (grdSingle.Visibility == Visibility.Collapsed & grdDouble.Visibility == Visibility.Visible)
                {
                    if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex == -1)
                    {
                        tblDayD1.Text = "";
                    }
                    else if (cbDayLec.SelectedIndex == -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD2.Text = "";
                    }
                    else if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD1.Text = "";
                        tblDayD2.Text = "";
                    }
                }
            }
            
        }

        private void CbDayLab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDayLab.SelectedIndex != -1)
            {
                if (grdSingle.Visibility == Visibility.Visible & grdDouble.Visibility == Visibility.Collapsed)
                {
                    tblDay.Text = cbDayLec.SelectedValue.ToString();
                }
                else if (grdSingle.Visibility == Visibility.Collapsed & grdDouble.Visibility == Visibility.Visible)
                {
                    if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex == -1)
                    {
                        tblDayD1.Text = cbDayLec.SelectedValue.ToString();
                    }
                    else if (cbDayLec.SelectedIndex == -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD2.Text = cbDayLab.SelectedValue.ToString();
                    }
                    else if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD1.Text = cbDayLec.SelectedValue.ToString();
                        tblDayD2.Text = cbDayLab.SelectedValue.ToString();
                    }
                }
                Select_Day_Lab();
            }
            else
            {
                if (grdSingle.Visibility == Visibility.Visible & grdDouble.Visibility == Visibility.Collapsed)
                {
                    tblDay.Text = "";
                }
                else if (grdSingle.Visibility == Visibility.Collapsed & grdDouble.Visibility == Visibility.Visible)
                {
                    if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex == -1)
                    {
                        tblDayD1.Text = "";
                    }
                    else if (cbDayLec.SelectedIndex == -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD2.Text = "";
                    }
                    else if (cbDayLec.SelectedIndex != -1 & cbDayLab.SelectedIndex != -1)
                    {
                        tblDayD1.Text = "";
                        tblDayD2.Text = "";
                    }
                }
            }
        }

        private void CbStimeLec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Start_Time_Lec();
        }

        private void CbStimeLab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Start_Time_Lab();
        }
    }
}
