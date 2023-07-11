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
    /// Interaction logic for mCourse.xaml
    /// </summary>
    public partial class mCourse : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlDataReader reader;

        string cid = string.Empty;
        string ccode = string.Empty;
        string cdesc = string.Empty;
        string syr = string.Empty;

        bool coucode = false;
        bool coudesc = false;
        bool yr = false;
        bool hasrow = false;
        bool hasrow2 = false;

        bool clicked = false;
        ToolTip tt;
        public mCourse()
        {
            InitializeComponent();
            Get_Update_Course();
            Scroll_Yr();
        }

        void Scroll_Yr()
        {
            scrollyr.Minimum = 0;
            scrollyr.Maximum = 10;
            scrollyr.SmallChange = 1;
        }

        void Get_Update_Course()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Course))
                {
                    cid = (window as Course).courseid;
                }
            }
            if (cid != string.Empty)
            {
                Ccode_Update();
                btnSave.Content = "UPDATE";
                btnSave.IsEnabled = false;
            }
        }

        void Ccode_Update()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_id='" + cid + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CcodeT.Text = dr[1].ToString();
                            CdescT.Text = dr[2].ToString();
                            yrT.Text = dr[3].ToString();
                            ccode = dr[1].ToString();
                            cdesc = dr[2].ToString();
                            syr = dr[3].ToString();
                            scrollyr.Value = int.Parse(dr[3].ToString());
                        }
                    }
                }
            }
        }

        void Course_check()
        {
            if ( cid != string.Empty)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                con.Open();
                cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_id IN (SELECT Course_id FROM tblcourse WHERE Course_id <> '" + cid + "') AND Course_name='" + CcodeT.Text + "'", con);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bdrCcode.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course Code already exist." };
                    bdrCcode.ToolTip = tt;
                    coucode = false;
                    hasrow = true;
                }
                else
                {
                    bdrCcode.BorderBrush = Brushes.Black;
                    bdrCcode.ClearValue(Border.ToolTipProperty);
                    coucode = true;
                    hasrow = false;
                }
                con.Close();
            }
            else
            {
                MySqlConnection con = new MySqlConnection(Connection);
                con.Open();
                cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_name='" + CcodeT.Text + "'", con);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bdrCcode.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course Code already exist." };
                    bdrCcode.ToolTip = tt;
                    coucode = false;
                    hasrow = true;
                }
                else
                {
                    bdrCcode.BorderBrush = Brushes.Black;
                    bdrCcode.ClearValue(Border.ToolTipProperty);
                    coucode = true;
                    hasrow = false;
                }
                con.Close();
            }
            
            Desc_check();
        }

        void Desc_check()
        {
            if (cid != string.Empty)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                con.Open();
                cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_id IN (SELECT Course_id FROM tblcourse WHERE Course_id <> '" + cid + "') AND Course_desc='" + CdescT.Text + "'", con);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bdrCdesc.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course Description already exist." };
                    bdrCdesc.ToolTip = tt;
                    coudesc = false;
                    hasrow2 = true;
                }
                else
                {
                    bdrCdesc.BorderBrush = Brushes.Black;
                    bdrCdesc.ClearValue(Border.ToolTipProperty);
                    coudesc = true;
                    hasrow2 = false;
                }
                con.Close();
            }
            else
            {
                MySqlConnection con = new MySqlConnection(Connection);
                con.Open();
                cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_desc='" + CdescT.Text + "'", con);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bdrCdesc.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course Description already exist." };
                    bdrCdesc.ToolTip = tt;
                    coudesc = false;
                    hasrow2 = true;
                }
                else
                {
                    bdrCdesc.BorderBrush = Brushes.Black;
                    bdrCdesc.ClearValue(Border.ToolTipProperty);
                    coudesc = true;
                    hasrow2 = false;
                }
                con.Close();
            }
            Save_Check();
        }

        void Save_Check()
        {
            if (cid!= string.Empty)
            {
                if (CcodeT.Text == ccode && CdescT.Text == cdesc && yrT.Text == syr)
                {
                    btnSave.IsEnabled = false;
                }
                else
                {
                    if (hasrow != true)
                    {
                        if (hasrow2 != true)
                        {
                            btnSave.IsEnabled = true;
                        }
                        else
                        {
                            btnSave.IsEnabled = false;
                        }
                    }
                    else
                    {
                        btnSave.IsEnabled = false;
                    }
                }
            }
            else
            {
                if (hasrow != true)
                {
                    if (hasrow2 != true)
                    {
                        btnSave.IsEnabled = true;
                    }
                    else
                    {
                        btnSave.IsEnabled = false;
                    }
                }
                else
                {
                    btnSave.IsEnabled = false;
                }
            }
        }

        void LetterOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[A-Za-z]$"))
            {
                e.Handled = true;
            }
        }

        private void CdescT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CdescT.LineCount > 2)
            {
                bdrCdesc.Height = 80;
                btnSave.Margin = new Thickness(140, 210, 140, 10);
            }
            else
            {
                bdrCdesc.Height = 60;
            }
            if (clicked == true)
            {
                if (CdescT.Text.Length == 0)
                {
                    bdrCdesc.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course Description cannot be blank." };
                    bdrCdesc.ToolTip = tt;
                }
                else
                {
                    bdrCdesc.ClearValue(Border.ToolTipProperty);
                }
            }
            Course_check();
        }

        private void CcodeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (CcodeT.Text.Length == 0)
                {
                    bdrCcode.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Course Code cannot be blank." };
                    bdrCcode.ToolTip = tt;
                }
                else
                {
                    bdrCcode.ClearValue(Border.ToolTipProperty);
                }
            }
            Course_check();
        }

        private void CcodeT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrCcode.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CcodeT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (hasrow != true)
            {
                bdrCcode.BorderBrush = Brushes.Black;
            }
        }

        private void CdescT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrCdesc.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CdescT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (hasrow2 != true)
            {
                bdrCdesc.BorderBrush = Brushes.Black;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.IsHitTestVisible = false;
            clicked = true;
            if (CcodeT.Text == "")
            {
                bdrCcode.BorderBrush = Brushes.Red;
                tt = new ToolTip { Content = "Course Code cannot be blank." };
                bdrCcode.ToolTip = tt;
            }
            if (CdescT.Text == "")
            {
                bdrCdesc.BorderBrush = Brushes.Red;
                tt = new ToolTip { Content = "Course Description cannot be blank." };
                bdrCdesc.ToolTip = tt;
            }
            if (Int32.Parse(yrT.Text) == 0)
            {
                bdrYr.BorderBrush = Brushes.Red;
                tt = new ToolTip { Content = "Maximum Year level for this course cannot be zero." };
                bdrYr.ToolTip = tt;
            }
            if (cid != string.Empty)
            {
                if (coudesc == true && coucode == true)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        cmd = new MySqlCommand("UPDATE `tblcourse` SET `Course_name`=@Course,`Course_desc`=@Description, `Max_yr`=@yr WHERE Course_id ='" + cid + "'", con);
                        cmd.Parameters.AddWithValue("@Course", CcodeT.Text);
                        cmd.Parameters.AddWithValue("@Description", CdescT.Text);
                        cmd.Parameters.AddWithValue("@yr", yrT.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();
                        MessageBox.Show("The data has been updated successfully.");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Course))
                            {
                                (window as Course).grdcontent.Children.Remove(this);
                                (window as Course).Fill_DataGrid_Course();
                                (window as Course).bdrContent.IsEnabled = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }
                }
            }
            else
            {
                if (coudesc == true && coucode == true)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        cmd = new MySqlCommand("INSERT INTO `tblcourse`(`Course_name`, `Course_desc`,`Max_yr`) VALUES (@Course,@Description,@yr);", con);
                        cmd.Parameters.AddWithValue("@Course", CcodeT.Text);
                        cmd.Parameters.AddWithValue("@Description", CdescT.Text);
                        cmd.Parameters.AddWithValue("@yr", yrT.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        string liicrs = cmd.LastInsertedId.ToString();
                        con.Dispose();

                        int crsmaxyr = 0;
                        using (con = new MySqlConnection(Connection))
                        {
                            cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_id='" + liicrs + "'", con);
                            con.Open();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        crsmaxyr = int.Parse(dr[3].ToString());
                                    }
                                }
                            }
                        }
                        for (int x = 1; x <= crsmaxyr; x++)
                        {
                            for (int y = 1; y <= 26; y++)
                            {
                                string sectionname = string.Empty;
                                switch (y)
                                {
                                    case 1:
                                        sectionname = "A";
                                        break;
                                    case 2:
                                        sectionname = "B";
                                        break;
                                    case 3:
                                        sectionname = "C";
                                        break;
                                    case 4:
                                        sectionname = "D";
                                        break;
                                    case 5:
                                        sectionname = "E";
                                        break;
                                    case 6:
                                        sectionname = "F";
                                        break;
                                    case 7:
                                        sectionname = "G";
                                        break;
                                    case 8:
                                        sectionname = "H";
                                        break;
                                    case 9:
                                        sectionname = "I";
                                        break;
                                    case 10:
                                        sectionname = "J";
                                        break;
                                    case 11:
                                        sectionname = "K";
                                        break;
                                    case 12:
                                        sectionname = "L";
                                        break;
                                    case 13:
                                        sectionname = "M";
                                        break;
                                    case 14:
                                        sectionname = "N";
                                        break;
                                    case 15:
                                        sectionname = "O";
                                        break;
                                    case 16:
                                        sectionname = "P";
                                        break;
                                    case 17:
                                        sectionname = "Q";
                                        break;
                                    case 18:
                                        sectionname = "R";
                                        break;
                                    case 19:
                                        sectionname = "S";
                                        break;
                                    case 20:
                                        sectionname = "T";
                                        break;
                                    case 21:
                                        sectionname = "U";
                                        break;
                                    case 22:
                                        sectionname = "V";
                                        break;
                                    case 23:
                                        sectionname = "W";
                                        break;
                                    case 24:
                                        sectionname = "X";
                                        break;
                                    case 25:
                                        sectionname = "Y";
                                        break;
                                    case 26:
                                        sectionname = "Z";
                                        break;
                                }
                                cmd = new MySqlCommand("INSERT INTO `tblsection`(`Sec_year`, `Sec_name`,`Course_id`) VALUES (@year,@name,@cid)", con);
                                cmd.Parameters.AddWithValue("@year", x);
                                cmd.Parameters.AddWithValue("@name", sectionname);
                                cmd.Parameters.AddWithValue("@cid", liicrs);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Dispose();
                            }
                        }

                        cmd = new MySqlCommand("INSERT INTO `tblsection`(`Sec_year`, `Sec_name`,`Course_id`) VALUES (0,'PETITION',@cid)", con);
                        cmd.Parameters.AddWithValue("@cid", liicrs);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();

                        MessageBox.Show("The data has been updated successfully.");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Course))
                            {
                                (window as Course).grdcontent.Children.Remove(this);
                                (window as Course).Fill_DataGrid_Course();
                                (window as Course).bdrContent.IsEnabled = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }
                }
            }
            IsHitTestVisible = true;
            Mouse.OverrideCursor = null;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (cid != string.Empty)
            {
                if (CcodeT.Text == cid && CdescT.Text == cdesc)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to cancel updating this course?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Course))
                            {
                                (window as Course).grdcontent.Children.Remove(this);
                                (window as Course).bdrContent.IsEnabled = true;

                            }
                        }
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        //code for Cancel
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Any unsaved changes will be lost. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Course))
                            {
                                (window as Course).grdcontent.Children.Remove(this);
                                (window as Course).bdrContent.IsEnabled = true;

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
                if (CcodeT.Text != "" || CdescT.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("Any unsaved changes will be lost. Do you want to continue?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.OK)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Course))
                            {
                                (window as Course).grdcontent.Children.Remove(this);
                                (window as Course).bdrContent.IsEnabled = true;

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
                        if (window.GetType() == typeof(Course))
                        {
                            (window as Course).grdcontent.Children.Remove(this);
                            (window as Course).bdrContent.IsEnabled = true;

                        }
                    }
                }
            }
        }

        private void Scrollyr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            yrT.Text = scrollyr.Value.ToString();
            if (clicked == true)
            {
                if (Int32.Parse(yrT.Text) == 0)
                {
                    bdrYr.BorderBrush = Brushes.Red;
                    tt = new ToolTip { Content = "Maximum Year level for this course cannot be zero." };
                    bdrYr.ToolTip = tt;
                }
                else
                {
                    bdrYr.BorderBrush = Brushes.Black;
                    bdrYr.ClearValue(Border.ToolTipProperty);
                }
            }
            Save_Check();
        }
    }
}
