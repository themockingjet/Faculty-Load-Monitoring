using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mSubL.xaml
    /// </summary>
    public partial class mSubL : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlDataReader reader;
        string cid = string.Empty;
        string sid = string.Empty;


        bool bool_scode = false;
        bool bool_sdesc = false;
        bool bool_cou = false;
        bool bool_year = false;
        bool bool_unit = false;
        bool bool_hour = false;
        bool bool_sem = false;
        bool clicked = false;
        bool hasrow = false;

        string string_scode;
        string string_sdesc;
        string string_cou;
        string string_year;
        string string_unit;
        string string_clec;
        string string_clab;
        string string_hour;
        string string_hlec;
        string string_hlab;
        string string_sem = string.Empty;

        ToolTip tt;
        public mSubL()
        {
            InitializeComponent();

            ScrollbarLec_hour();
            ScrollbarLab_hour();
            ScrollbarUnit_Lec();
            ScrollbarUnit_Lab();
            ComboBox_Course();
            ComboBox_Semester();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Sub_List))
                {
                    sid = (window as Sub_List).sub_id;
                }
            }
            if (sid != string.Empty)
            {
                Scode_Update();
            }
        }

        void Scode_Update()
        {
            btnSave.Content = "UPDATE";
            btnSave.IsEnabled = false;
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblsubject JOIN tblcourse WHERE tblcourse.Course_id = tblsubject.Course_id AND Sub_id='" + sid + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            S_codeT.Text = dr[0].ToString();
                            string_scode = dr[0].ToString();

                            S_descT.Text = dr[1].ToString();
                            string_sdesc = dr[1].ToString();

                            TunitsT.Text = dr[2].ToString();
                            string_unit = dr[2].ToString();

                            CUlecT.Text = dr[3].ToString();
                            string_clec = dr[3].ToString();
                            scrollUnitLec.Value = Int32.Parse(string_clec);

                            CUlabT.Text = dr[4].ToString();
                            string_clab = dr[4].ToString();
                            scrollUnitLab.Value = Int32.Parse(string_clab);


                            CHlecT.Text = dr[5].ToString();
                            string_hlec = dr[5].ToString();
                            scrollhourLec.Value = Int32.Parse(string_hlec);

                            CHlabT.Text = dr[6].ToString();
                            string_hlab = dr[6].ToString();
                            scrollhourLab.Value = Int32.Parse(string_hlab);

                            string_hour = (Int32.Parse(string_hlec) + Int32.Parse(string_hlab)).ToString();
                            ChoursT.Text = string_hour;

                            YearT.Text = dr[7].ToString();
                            string_year = dr[7].ToString();
                            YearT.SelectedValue = dr[7].ToString();

                            CourseT.Text = dr[8].ToString();
                            string_cou = dr[8].ToString();
                            cid = dr[8].ToString();
                            CourseT.SelectedValue = dr[8].ToString();

                            SemT.Text = dr[9].ToString();
                            string_sem = SemT.Text;
                            SemT.SelectedValue = dr[9].ToString();

                        }
                    }
                }
            }
        }

        void LetterNumber(object sender, TextCompositionEventArgs e)
        {
            if(!Regex.IsMatch(e.Text, "^[A-Za-z0-9]$"))
            {
                e.Handled = true;
            }
        }

        void LetterOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[A-Za-z]$"))
            {
                e.Handled = true;
            }
        }

        void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]$"))
            {
                e.Handled = true;
            }
        }

        void ScrollbarLec_hour()
        {
            scrollhourLec.Minimum = 0;
            scrollhourLec.Maximum = 10;
            scrollhourLec.SmallChange = 1;
        }

        void ScrollbarLab_hour()
        {
            scrollhourLab.Minimum = 0;
            scrollhourLab.Maximum = 10;
            scrollhourLab.SmallChange = 1;
        }

        void ScrollbarUnit_Lec()
        {
            scrollUnitLec.Minimum = 0;
            scrollUnitLec.Maximum = 10;
            scrollUnitLec.SmallChange = 1;
        }

        void ScrollbarUnit_Lab()
        {
            scrollUnitLab.Minimum = 0;
            scrollUnitLab.Maximum = 10;
            scrollUnitLab.SmallChange = 1;
        }

        void ComboBox_Course()
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
                            CourseT.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
        }

        void ComboBox_Year()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblyear LIMIT " + limit, con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dty = new DataTable())
                        {
                            sda.Fill(dty);
                            YearT.ItemsSource = dty.DefaultView;
                        }
                    }
                }
            }
        }

        void ComboBox_Semester()
        {
            SemT.Items.Add("FIRST");
            SemT.Items.Add("SECOND");
            SemT.Items.Add("MID YEAR");
        }

        void Check_Scode()
        {
            if (sid != string.Empty)
            {
                if (S_codeT.Text != "" && CourseT.SelectedIndex != -1)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT * FROM `tblsubject` WHERE Sub_id IN (SELECT Sub_id FROM tblsubject where Sub_id <> '" + sid + "') AND Sub_code ='" + S_codeT.Text + "'AND Course_id='" + crsid + "'", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        con.Close();
                        reader.Close();
                        hasrow = true;
                        bool_scode = false;
                    }
                    else
                    {
                        con.Close();
                        reader.Close();
                        hasrow = false;
                        bool_scode = true;
                    }
                }
            }
            else
            {
                if (S_codeT.Text != "" && CourseT.SelectedIndex != -1)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT * FROM `tblsubject` LEFT JOIN tblcourse using (Course_id) WHERE tblsubject.Sub_code ='" + S_codeT.Text + "'AND Course_name ='" + crs + "'", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        con.Close();
                        reader.Close();
                        hasrow = true;
                        bool_scode = false;
                    }
                    else
                    {
                        con.Close();
                        reader.Close();
                        hasrow = false;
                        bool_scode = true;
                    }
                }
            }
            Save_Check();
        }

        void Check_Sdesc()
        {
            if (sid != string.Empty)
            {
                if (S_descT.Text != "" && CourseT.SelectedIndex != -1)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT * FROM `tblsubject` WHERE Sub_id IN (SELECT Sub_id FROM tblsubject where Sub_id <> '" + sid + "') AND Sub_desc ='" + S_descT.Text + "'AND Course_id='" + crsid + "'", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        con.Close();
                        reader.Close();
                        hasrow = true;
                        bool_sdesc = false;
                    }
                    else
                    {
                        hasrow = false;
                        bool_sdesc = true;
                    }
                }
            }
            else
            {
                if (S_descT.Text != "" && CourseT.SelectedIndex != -1)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT * FROM `tblsubject` LEFT JOIN tblcourse using (Course_id) WHERE tblsubject.Sub_desc ='" + S_descT.Text + "'AND Course_name ='" + crs + "'", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        con.Close();
                        reader.Close();
                        hasrow = true;
                        bool_sdesc = false;
                    }
                    else
                    {
                        con.Close();
                        reader.Close();
                        hasrow = false;
                        bool_sdesc = true;
                    }
                }
            }
            Save_Check();
        }

        void Save_Check()
        {
            if (sid != string.Empty)
            {
                if (YearT.SelectedIndex != -1 && SemT.SelectedIndex != -1 && string_sem != string.Empty)
                {
                    if (S_codeT.Text == string_scode && S_descT.Text == string_sdesc && crsid == string_cou && TunitsT.Text == string_unit && ChoursT.Text == string_hour && SemT.SelectedValue.ToString() == string_sem && year == string_year)
                    {
                        btnSave.IsEnabled = false;
                    }
                    else
                    {
                        if (hasrow != true)
                        {
                            btnSave.IsEnabled = true;
                        }
                        else
                        {
                            btnSave.IsEnabled = false;
                        }
                    }
                }
            }
            else
            {
                if (S_codeT.Text == "" || S_descT.Text == "" || CourseT.SelectedIndex == -1 || YearT.SelectedIndex == -1)
                {
                    if(btnSave != null)
                    {
                        btnSave.IsEnabled = false;
                    }
                }
                else
                {
                    if (hasrow != true)
                    {
                        btnSave.IsEnabled = true;
                    }
                    else
                    {
                        btnSave.IsEnabled = false;
                    }
                }
            }
        }

        private void ScrollUnitLec_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CUlecT.Text = scrollUnitLec.Value.ToString();
            if (CUlecT.Text != null && CUlabT.Text != null)
            {
                int x = 0;
                x = Int32.Parse(CUlecT.Text) + Int32.Parse(CUlabT.Text);
                TunitsT.Text = x.ToString();
            }
        }

        private void ScrollUnitLab_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CUlabT.Text = scrollUnitLab.Value.ToString();
            if (CUlecT.Text != null && CUlabT.Text != null)
            {
                int x = 0;
                x = Int32.Parse(CUlecT.Text) + Int32.Parse(CUlabT.Text);
                TunitsT.Text = x.ToString();
            }
        }

        private void ScrollhourLec_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CHlecT.Text = scrollhourLec.Value.ToString();
            if (CHlecT.Text != null && CHlabT.Text != null)
            {
                int x = 0;
                x = Int32.Parse(CHlecT.Text) + Int32.Parse(CHlabT.Text);
                ChoursT.Text = x.ToString();
            }
        }

        private void ScrollhourLab_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CHlabT.Text = scrollhourLab.Value.ToString();
            if (CHlecT.Text != null && CHlabT.Text != null)
            {
                int x = 0;
                x = Int32.Parse(CHlecT.Text) + Int32.Parse(CHlabT.Text);
                ChoursT.Text = x.ToString();
                if (clicked == true)
                {
                    if (x == 0)
                    {
                        bdrChours.BorderBrush = Brushes.Red;
                        tt = new ToolTip { Content = "Contact Hours can not be zero." };
                        bdrChours.ToolTip = tt;
                        bool_hour = false;
                    }
                    else
                    {
                        bdrChours.BorderBrush = Brushes.Black;
                        bdrChours.ClearValue(Border.ToolTipProperty);
                        bool_hour = true;
                    }
                }
                else
                {
                    if (x == 0)
                    {
                        bool_hour = false;
                    }
                    else
                    {
                        bool_hour = true;
                    }
                }
            }
        }

        private void S_codeT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrS_code.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            ScodeW.Visibility = Visibility.Collapsed;
            if (clicked == true)
            {
                if (ppScode.Visibility == Visibility.Visible)
                {
                    ppSdesc.Visibility = Visibility.Collapsed;
                }
                if (ppTunits.Visibility == Visibility.Visible)
                {
                    ppTunits.Visibility = Visibility.Collapsed;
                }
                if (ppChours.Visibility == Visibility.Visible)
                {
                    ppChours.Visibility = Visibility.Collapsed;
                }
                if (ppSem.Visibility == Visibility.Visible)
                {
                    ppSem.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void S_codeT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_scode == true)
            {
                bdrS_code.BorderBrush = Brushes.Black;
            }
            if (S_codeT.Text == "")
            {
                ScodeW.Visibility = Visibility.Visible;
            }
            else
            {
                ScodeW.Visibility = Visibility.Collapsed;
            }
            
        }

        private void S_codeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            Check_Scode();
            if (clicked == true)
            {
                if (S_codeT.Text.Length == 0)
                {
                    bdrS_code.BorderBrush = Brushes.Red;
                    ppScodetxt.Text = "Subject Code is required";
                    ppScode.Visibility = Visibility.Visible;
                    bool_scode = false;
                    if (ppScode.Visibility == Visibility.Visible)
                    {
                        ppSdesc.Visibility = Visibility.Collapsed;
                    }
                    if (ppTunits.Visibility == Visibility.Visible)
                    {
                        ppTunits.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    bdrS_code.BorderBrush = Brushes.Black;
                    ppScodetxt.Text = "Subject Code already exist for this course";
                    ppScode.Visibility = Visibility.Collapsed;
                    if (hasrow != true)
                    {
                        ppScode.Visibility = Visibility.Collapsed;
                        bdrS_code.BorderBrush = Brushes.Black;
                        bool_scode = true;
                        if (S_descT.Text != "")
                        {
                            Check_Sdesc();
                            if (hasrow != true)
                            {
                                ppSdesc.Visibility = Visibility.Collapsed;
                                bdrS_desc.BorderBrush = Brushes.Black;
                                bool_sdesc = true;
                                if (bool_unit == false)
                                {
                                    ppTunits.Visibility = Visibility.Visible;
                                }
                                if (bool_unit == true && bool_hour == false)
                                {
                                    ppChours.Visibility = Visibility.Visible;
                                }
                                if (bool_unit == true && bool_hour == true && bool_sem == false)
                                {
                                    ppSem.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                if (ppScode.Visibility == Visibility.Visible)
                                {
                                    ppSdesc.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    ppSdesc.Visibility = Visibility.Visible;
                                }
                                bdrS_desc.BorderBrush = Brushes.Black;
                                bool_sdesc = false;
                            }
                        }
                    }
                    else
                    {
                        if (ppSdesc.Visibility == Visibility.Visible)
                        {
                            ppSdesc.Visibility = Visibility.Collapsed;
                            ppScode.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ppScode.Visibility = Visibility.Visible;
                        }
                        bdrS_code.BorderBrush = Brushes.Red;
                        bool_scode = false;
                    }
                }
            }
            else
            {
                if (hasrow != true)
                {
                    ppScode.Visibility = Visibility.Collapsed;
                    bdrS_code.BorderBrush = Brushes.Black;
                    bool_scode = true;
                    if (S_descT.Text != "")
                    {
                        Check_Sdesc();
                        if (hasrow != true)
                        {
                            ppSdesc.Visibility = Visibility.Collapsed;
                            bdrS_desc.BorderBrush = Brushes.Black;
                            bool_sdesc = true;
                        }
                        else
                        {
                            if (ppScode.Visibility == Visibility.Visible)
                            {
                                ppSdesc.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                ppSdesc.Visibility = Visibility.Visible;
                            }
                            bdrS_desc.BorderBrush = Brushes.Black;
                            bool_sdesc = false;
                        }
                    }
                }
                else
                {
                    if (ppSdesc.Visibility == Visibility.Visible)
                    {
                        ppSdesc.Visibility = Visibility.Collapsed;
                        ppScode.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ppScode.Visibility = Visibility.Visible;
                    }
                    bdrS_code.BorderBrush = Brushes.Red;
                    bool_scode = false;
                }
            }
        }

        private void S_descT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrS_desc.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            if (S_codeT.Text.Length == 0)
            {
                bool_sdesc = false;
                SdescW.Visibility = Visibility.Visible;
            }
            else
            {
                bool_sdesc = true;
                SdescW.Visibility = Visibility.Collapsed;
            }
            if (clicked == true)
            {
                if (ppScode.Visibility == Visibility.Visible)
                {
                    ppSdesc.Visibility = Visibility.Collapsed;
                }
                if (ppTunits.Visibility == Visibility.Visible)
                {
                    ppTunits.Visibility = Visibility.Collapsed;
                }
                if (ppChours.Visibility == Visibility.Visible)
                {
                    ppChours.Visibility = Visibility.Collapsed;
                }
                if (ppSem.Visibility == Visibility.Visible)
                {
                    ppSem.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void S_descT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (hasrow != true)
            {
                bdrS_desc.BorderBrush = Brushes.Black;
            }
            else
            {
                bdrS_desc.BorderBrush = Brushes.Red;
            }
            if (S_codeT.Text == "")
            {
                SdescW.Visibility = Visibility.Visible;
            }
            else
            {
                SdescW.Visibility = Visibility.Collapsed;
            }
        }

        private void S_descT_TextChanged(object sender, TextChangedEventArgs e)
        {
            Check_Sdesc();
            if (clicked == true)
            {
                if (S_descT.Text.Length == 0)
                {
                    bdrS_desc.BorderBrush = Brushes.Red;
                    ppsdesctxt.Text = "Subject Description is required";
                    ppSdesc.Visibility = Visibility.Visible;
                    bool_sdesc = false;
                }
                else
                {
                    bdrS_desc.BorderBrush = Brushes.Black;
                    ppsdesctxt.Text = "Subject Code already exist for this course";
                    ppSdesc.Visibility = Visibility.Collapsed;
                    if (hasrow != true)
                    {
                        ppSdesc.Visibility = Visibility.Collapsed;
                        bdrS_desc.BorderBrush = Brushes.Black;
                        bool_sdesc = true;
                        if (bool_unit == false)
                        {
                            ppTunits.Visibility = Visibility.Visible;
                        }
                        if (bool_unit == true && bool_hour == false)
                        {
                            ppChours.Visibility = Visibility.Visible;
                        }
                        if (bool_unit == true && bool_hour == true && bool_sem == false)
                        {
                            ppSem.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (ppScode.Visibility == Visibility.Visible)
                        {
                            ppSdesc.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ppSdesc.Visibility = Visibility.Visible;
                        }

                        bdrS_desc.BorderBrush = Brushes.Red;
                        bool_sdesc = false;

                        if (ppTunits.Visibility == Visibility.Visible)
                        {
                            ppTunits.Visibility = Visibility.Collapsed;
                        }
                        if (ppChours.Visibility == Visibility.Visible)
                        {
                            ppChours.Visibility = Visibility.Collapsed;
                        }
                        if (ppSem.Visibility == Visibility.Visible)
                        {
                            ppSem.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            else
            {
                if (hasrow != true)
                {
                    ppSdesc.Visibility = Visibility.Collapsed;
                    bdrS_desc.BorderBrush = Brushes.Black;
                    bool_sdesc = true;
                }
                else
                {
                    if (ppScode.Visibility == Visibility.Visible)
                    {
                        ppSdesc.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ppSdesc.Visibility = Visibility.Visible;
                    }
                    bdrS_desc.BorderBrush = Brushes.Red;
                    bool_sdesc = false;
                }
            }
        }

        private void CourseT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrCourse.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CourseT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (bool_cou == true)
                {
                    bdrCourse.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    bdrCourse.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                bdrCourse.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        private string crs
        {
            get
            {
                var subject = CourseT.SelectedItem as DataRowView;
                string sub = subject.Row["Course_name"].ToString();
                return sub;
            }
        }

        private string crsid
        {
            get
            {
                var subject = CourseT.SelectedItem as DataRowView;
                string sub = subject.Row["Course_id"].ToString();
                return sub;
            }
        }

        private string year
        {
            get
            {
                var year = YearT.SelectedItem as DataRowView;
                string yr = year.Row["Year"].ToString();
                return yr;
            }
        }

        private string limit
        {
            get
            {
                var subject = CourseT.SelectedItem as DataRowView;
                string limit = subject.Row["Max_yr"].ToString();
                return limit;
            }
        }

        private void CourseT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (CourseT.SelectedIndex != -1)
                {
                    bdrCourse.BorderBrush = Brushes.Black;
                    bdrCourse.ClearValue(Border.ToolTipProperty);
                    bool_cou = true;
                    bdrYear.IsEnabled = true;
                    ComboBox_Year();
                }
                else
                {
                    bdrCourse.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course is required." };
                    bdrCourse.ToolTip = tt;
                    bool_cou = false;
                    bdrYear.IsEnabled = false;
                }
            }
            else
            {
                if (CourseT.SelectedIndex != -1)
                {
                    bool_cou = true;
                    Check_Scode();
                    bdrYear.IsEnabled = true;
                    ComboBox_Year();
                }
                else
                {
                    bool_cou = false;
                    bdrYear.IsEnabled = false;
                }
            }
            Check_Scode();
            Check_Sdesc();
        }

        private void YearT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrYear.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void YearT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (bool_year == true)
                {
                    bdrYear.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    bdrYear.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                bdrYear.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        private void YearT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (YearT.SelectedIndex != -1)
                {
                    bdrYear.BorderBrush = Brushes.Black;
                    bdrYear.ClearValue(Border.ToolTipProperty);
                    bool_year = true;
                }
                else
                {
                    bdrYear.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course is required." };
                    bdrYear.ToolTip = tt;
                    bool_year = false;
                }
            }
            else
            {
                if (YearT.SelectedIndex != -1)
                {
                    bool_year = true;
                }
                else
                {
                    bool_year = false;
                }
            }
            Save_Check();
        }

        private void TunitsT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (TunitsT.Text == "0")
                {
                    if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                    {
                        ppTunits.Visibility = Visibility.Visible;
                    }

                    bdrTunits.BorderBrush = Brushes.Red;
                    bool_unit = false;

                    if (ppChours.Visibility == Visibility.Visible)
                    {
                        ppChours.Visibility = Visibility.Collapsed;
                    }
                    if (ppSem.Visibility == Visibility.Visible)
                    {
                        ppSem.Visibility = Visibility.Collapsed;
                    }
                }
                else if (TunitsT.Text != "0")
                {
                    bdrTunits.BorderBrush = Brushes.Black;
                    ppTunits.Visibility = Visibility.Collapsed;
                    bool_unit = true;
                    
                    if (bool_hour == false)
                    {
                        if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                        {
                            ppChours.Visibility = Visibility.Visible;
                        }
                    }
                    if (bool_hour == true && bool_sem == false)
                    {
                        if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                        {
                            ppSem.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            else
            {
                if (TunitsT.Text == "0")
                {
                    bool_unit = false;
                }
                else
                {
                    bool_unit = true;
                }
            }
            Save_Check();
        }

        private void ChoursT_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x = int.Parse(ChoursT.Text);
            if (clicked == true)
            {
                if (x == 0)
                {
                    if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                    {
                        ppChours.Visibility = Visibility.Visible;
                    }

                    bdrChours.BorderBrush = Brushes.Red;
                    bool_hour = false;

                    if (ppSem.Visibility == Visibility.Visible)
                    {
                        ppSem.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    bdrChours.BorderBrush = Brushes.Black;
                    ppChours.Visibility = Visibility.Collapsed;
                    bool_hour = true;
                    if (bool_sem == false)
                    {
                        if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                        {
                            ppSem.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            else
            {
                if (x == 0)
                {
                    bool_hour = false;
                }
                else if (x != 0)
                {
                    bool_hour = true;
                }
            }
            Save_Check();
        }

        private void SemT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrSemester.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void SemT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (bool_sem == true)
                {
                    bdrSemester.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    bdrSemester.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                bdrSemester.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        private void SemT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (SemT.SelectedIndex != -1)
                {
                    bdrSemester.BorderBrush = Brushes.Black;
                    ppSem.Visibility = Visibility.Collapsed;
                    bool_sem = true;
                }
                else
                {
                    if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                    {
                        ppSem.Visibility = Visibility.Visible;
                    }
                    bdrSemester.BorderBrush = Brushes.Red;
                    bool_sem = false;
                }
            }
            else
            {
                if (CourseT.SelectedIndex != -1)
                {
                    bool_sem = true;
                }
                else
                {
                    bool_year = false;
                }
            }
            Save_Check();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (sid != string.Empty)
            {
                if (S_codeT.Text == string_scode && S_descT.Text == string_sdesc && CourseT.SelectedValue.ToString() == string_cou && year == string_year 
                    && TunitsT.Text == string_unit && CUlecT.Text == string_clec && CUlabT.Text == string_clab && ChoursT.Text == string_hour && CHlecT.Text == string_hlec
                    && CHlabT.Text == string_hlab && SemT.SelectedValue.ToString() == string_sem)
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Sub_List))
                        {
                            (window as Sub_List).grdcontent.Children.Remove(this);
                            (window as Sub_List).bdrContent.IsEnabled = true;

                        }
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Any changes will not be saved.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Sub_List))
                            {
                                (window as Sub_List).grdcontent.Children.Remove(this);
                                (window as Sub_List).bdrContent.IsEnabled = true;

                            }
                        }
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        //code for Cancel
                    }
                }
            }
            else
            {
                if (S_codeT.Text != "" || S_descT.Text != "" || CourseT.SelectedIndex != -1 || YearT.SelectedIndex != -1 || TunitsT.Text != "0" || CUlecT.Text != "0" || CUlabT.Text != "0" || ChoursT.Text != "0" || CHlecT.Text != "0" || CHlabT.Text != "0" || SemT.SelectedIndex != -1)
                {
                    MessageBoxResult result = MessageBox.Show("Any changes will not be saved.", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Sub_List))
                            {
                                (window as Sub_List).grdcontent.Children.Remove(this);
                                (window as Sub_List).bdrContent.IsEnabled = true;

                            }
                        }
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        //code for Cancel
                    }
                }
                else
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Sub_List))
                        {
                            (window as Sub_List).grdcontent.Children.Remove(this);
                            (window as Sub_List).bdrContent.IsEnabled = true;

                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            clicked = true;
            if (bool_unit == false)
            {
                bdrTunits.BorderBrush = Brushes.Red;
                if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible)
                {
                    ppTunits.Visibility = Visibility.Visible;
                }
            }
            if (bool_hour == false)
            {
                bdrChours.BorderBrush = Brushes.Red;
                if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible && ppTunits.Visibility != Visibility.Visible)
                {
                    ppChours.Visibility = Visibility.Visible;
                }
            }
            if (bool_sem == false)
            {
                bdrSemester.BorderBrush = Brushes.Red;
                if (ppScode.Visibility != Visibility.Visible && ppSdesc.Visibility != Visibility.Visible && ppTunits.Visibility != Visibility.Visible && ppChours.Visibility != Visibility.Visible)
                {
                    ppSem.Visibility = Visibility.Visible;
                }
            }

            if (sid != string.Empty)
            {
                try
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    MySqlCommand cmd = new MySqlCommand("UPDATE `tblsubject` SET `Sub_code`=@scode,`Sub_desc`=@sdesc,`T_units`=@tunits,`Cu_lec`=@ulec,`Cu_lab`=@ulab,`Ch_lec`=@hlec,`Ch_lab`=@hlab,`Year`=@year,`Course_id`=@course,`Semester`=@semester WHERE Sub_id ='" + sid + "'", con);
                    cmd.Parameters.AddWithValue("@scode", S_codeT.Text);
                    cmd.Parameters.AddWithValue("@sdesc", S_descT.Text);
                    cmd.Parameters.AddWithValue("@tunits", TunitsT.Text);
                    cmd.Parameters.AddWithValue("@ulec", CUlecT.Text);
                    cmd.Parameters.AddWithValue("@ulab", CUlabT.Text);
                    cmd.Parameters.AddWithValue("@hlec", CHlecT.Text);
                    cmd.Parameters.AddWithValue("@hlab", CHlabT.Text);
                    cmd.Parameters.AddWithValue("@year", YearT.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@course", crsid);
                    cmd.Parameters.AddWithValue("@semester", SemT.SelectedValue.ToString());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("The subject has been updated successfully.");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Sub_List))
                        {
                            (window as Sub_List).Fill_DataGrid_Subject();
                            (window as Sub_List).grdcontent.Children.Remove(this);
                            (window as Sub_List).bdrContent.IsEnabled = true;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                if (bool_scode == true && bool_sdesc == true && bool_cou == true && bool_year == true && bool_unit == true && bool_hour == true && bool_sem == true)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("INSERT INTO `tblsubject`(`Sub_code`, `Sub_desc`, `T_units`, `Cu_lec`, `Cu_lab`, `Ch_lec`, `Ch_lab`, `Year`, `Course_id`, `Semester`) VALUES"
                                         + "(?scode,?sdesc,?tunits,?ulec,?ulab,?hlec,?hlab,?year,?course,?semester)", con);
                    cmd.Parameters.AddWithValue("?scode", S_codeT.Text);
                    cmd.Parameters.AddWithValue("?sdesc", S_descT.Text);
                    cmd.Parameters.AddWithValue("?tunits", TunitsT.Text);
                    cmd.Parameters.AddWithValue("?ulec", CUlecT.Text);
                    cmd.Parameters.AddWithValue("?ulab", CUlabT.Text);
                    cmd.Parameters.AddWithValue("?hlec", CHlecT.Text);
                    cmd.Parameters.AddWithValue("?hlab", CHlabT.Text);
                    cmd.Parameters.AddWithValue("?year", YearT.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("?course", crsid);
                    cmd.Parameters.AddWithValue("?semester", SemT.SelectedValue.ToString());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Dispose();
                    MessageBox.Show("Subject added successfully.");

                    mSubL UCobj = new mSubL();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Sub_List))
                        {
                            (window as Sub_List).Fill_DataGrid_Subject();
                            (window as Sub_List).grdcontent.Children.Remove(this);
                            (window as Sub_List).bdrContent.IsEnabled = true;
                        }
                    }
                }
            }
        }
    }
}
