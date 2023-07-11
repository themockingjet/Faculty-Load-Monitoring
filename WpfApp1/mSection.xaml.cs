using JR.Utils.GUI.Forms;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mSection.xaml
    /// </summary>
    public partial class mSection : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlDataReader reader;
        DataTable dt;

        int maxyr = 0;
        string courseid = string.Empty;
        string ccode = string.Empty;
        string secid = string.Empty;

        bool update = false;
        bool add = false;
        bool yrsec = false;
        ToolTip tooltip;
        public mSection()
        {
            InitializeComponent();
            Get_Update_Section();
            Fill_Section();


            ScrollbarYear();
            ScrollbarSection();
            dt.RowChanged += new DataRowChangeEventHandler(dt_RowChanged);
        }

        void Get_Update_Section()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Course))
                {
                    courseid = (window as Course).courseid;
                    ccode = (window as Course).ccode;
                }
            }
            if (courseid != string.Empty)
            {
                Ccode_Update();
                CcodeT.Text = ccode;
            }
        }

        void Ccode_Update()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("SELECT * FROM tblcourse WHERE Course_id='" + courseid + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            maxyr = int.Parse(dr[3].ToString());
                        }
                    }
                }
            }
        }

        void Fill_Section()
        {
            MySqlConnection con = new MySqlConnection(Connection);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblsection WHERE Course_id ='" + courseid + "' ORDER BY Sec_year, Sec_name", con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            var col = new DataColumn("Check", typeof(bool));
            col.DefaultValue = false;
            dt.Columns.Add(col);
            dtSection.ItemsSource = dt.DefaultView;
        }

        void ScrollbarYear()
        {
            scrollYear.Minimum = 1;
            scrollYear.Maximum = maxyr;
            scrollYear.SmallChange = 1;
        }

        void ScrollbarSection()
        {
            scrollSection.Minimum = 1;
            scrollSection.Maximum = 26;
            scrollSection.SmallChange = 1;
        }


        private void ScrollYear_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (update == true || add == true)
            {
                YearT.Text = scrollYear.Value.ToString();
            }
        }

        string sec = string.Empty;
        string defyr = string.Empty;
        string defsec = string.Empty;

        void Section()
        {
            switch (scrollSection.Value)
            {
                case 1:
                    SectionT.Text = "A";
                    break;
                case 2:
                    SectionT.Text = "B";
                    break;
                case 3:
                    SectionT.Text = "C";
                    break;
                case 4:
                    SectionT.Text = "D";
                    break;
                case 5:
                    SectionT.Text = "E";
                    break;
                case 6:
                    SectionT.Text = "F";
                    break;
                case 7:
                    SectionT.Text = "G";
                    break;
                case 8:
                    SectionT.Text = "H";
                    break;
                case 9:
                    SectionT.Text = "I";
                    break;
                case 10:
                    SectionT.Text = "J";
                    break;
                case 11:
                    SectionT.Text = "K";
                    break;
                case 12:
                    SectionT.Text = "L";
                    break;
                case 13:
                    SectionT.Text = "M";
                    break;
                case 14:
                    SectionT.Text = "N";
                    break;
                case 15:
                    SectionT.Text = "O";
                    break;
                case 16:
                    SectionT.Text = "P";
                    break;
                case 17:
                    SectionT.Text = "Q";
                    break;
                case 18:
                    SectionT.Text = "R";
                    break;
                case 19:
                    SectionT.Text = "S";
                    break;
                case 20:
                    SectionT.Text = "T";
                    break;
                case 21:
                    SectionT.Text = "U";
                    break;
                case 22:
                    SectionT.Text = "V";
                    break;
                case 23:
                    SectionT.Text = "W";
                    break;
                case 24:
                    SectionT.Text = "X";
                    break;
                case 25:
                    SectionT.Text = "Y";
                    break;
                case 26:
                    SectionT.Text = "Z";
                    break;
                case 0:
                    SectionT.Text = "";
                    break;
            }
        }

        void Section_Value()
        {
            switch (SectionT.Text)
            {
                case "A":
                    scrollSection.Value = 1;
                    break;
                case "B":
                    scrollSection.Value = 2;
                    break;
                case "C":
                    scrollSection.Value = 3;
                    break;
                case "D":
                    scrollSection.Value = 4;
                    break;
                case "E":
                    scrollSection.Value = 5;
                    break;
                case "F":
                    scrollSection.Value = 6;
                    break;
                case "G":
                    scrollSection.Value = 7;
                    break;
                case "H":
                    scrollSection.Value = 8;
                    break;
                case "I":
                    scrollSection.Value = 9;
                    break;
                case "J":
                    scrollSection.Value = 10;
                    break;
                case "K":
                    scrollSection.Value = 11;
                    break;
                case "L":
                    scrollSection.Value = 12;
                    break;
                case "M":
                    scrollSection.Value = 13;
                    break;
                case "N":
                    scrollSection.Value = 14;
                    break;
                case "O":
                    scrollSection.Value = 15;
                    break;
                case "P":
                    scrollSection.Value = 16;
                    break;
                case "Q":
                    scrollSection.Value = 17;
                    break;
                case "R":
                    scrollSection.Value = 18;
                    break;
                case "S":
                    scrollSection.Value = 19;
                    break;
                case "T":
                    scrollSection.Value = 20;
                    break;
                case "U":
                    scrollSection.Value = 21;
                    break;
                case "V":
                    scrollSection.Value = 22;
                    break;
                case "W":
                    scrollSection.Value = 23;
                    break;
                case "X":
                    scrollSection.Value = 24;
                    break;
                case "Y":
                    scrollSection.Value = 25;
                    break;
                case "Z":
                    scrollSection.Value = 26;
                    break;
                default:
                    scrollSection.Value = 0;
                    break;
            }
        }

        void Year_Value()
        {
            switch(YearT.Text)
            {
                case "":
                    scrollYear.Value = 0;
                    break;
                case "1":
                    scrollYear.Value = 1;
                    break;
                case "2":
                    scrollYear.Value = 2;
                    break;
                case "3":
                    scrollYear.Value = 3;
                    break;
                case "4":
                    scrollYear.Value = 4;
                    break;
                case "5":
                    scrollYear.Value = 5;
                    break;
                case "6":
                    scrollYear.Value = 6;
                    break;
                case "7":
                    scrollYear.Value = 7;
                    break;
                case "8":
                    scrollYear.Value = 8;
                    break;
            }
        }

        private void ScrollSection_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (update == true || add == true)
            {
                Section();
            }
        }

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
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = true;
                CcodeT.Text = string.Empty;
            }
            else if (x == 1)
            {
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else if (x == 0)
            {
                dtSection.SelectedIndex = -1;
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

        private void DtSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtSection.SelectedIndex != -1)
            {
                btnDelete.IsEnabled = true;
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
                            secid = row["Sec_id"].ToString();
                            defyr = row["Sec_year"].ToString();
                            defsec = row["Sec_name"].ToString();

                            YearT.Text = row["Sec_year"].ToString();
                            SectionT.Text = row["Sec_name"].ToString();
                            if (row["Sec_name"].ToString() == "PETITION")
                            {
                                btnUpdate.IsEnabled = false;
                            }
                        }
                    }
                    
                }
                else if (x == 0)
                {
                    btnUpdate.IsEnabled = true;
                    secid = ((DataRowView)dtSection.SelectedItem).Row["Sec_id"].ToString();
                    defyr = ((DataRowView)dtSection.SelectedItem).Row["Sec_year"].ToString();
                    defsec = ((DataRowView)dtSection.SelectedItem).Row["Sec_name"].ToString();

                    YearT.Text = ((DataRowView)dtSection.SelectedItem).Row["Sec_year"].ToString();
                    SectionT.Text = ((DataRowView)dtSection.SelectedItem).Row["Sec_name"].ToString();
                    if (((DataRowView)dtSection.SelectedItem).Row["Sec_name"].ToString() == "PETITION")
                    {
                        btnUpdate.IsEnabled = false;
                    }
                }
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            string yrup = string.Empty;
            string secup = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Check"] == true)
                {
                    x += 1;
                    yrup = row["year"].ToString();
                    secup = row["section"].ToString();
                }
            }
            if (x == 1)
            {
                YearT.Text = yrup;
                SectionT.Text = secup;
            }
            update = true;
            btnAdd.Content = "SAVE";
            btnDelete.Content = "CANCEL";
            btnUpdate.Width = 130;
            btnUpdate.Margin = new Thickness(50, 0, 162, 280);
            btnDelete.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            scrollYear.Visibility = Visibility.Visible;
            scrollSection.Visibility = Visibility.Visible;
            dtSection.IsEnabled = false;
            tblYredit.Text = "EDIT MODE";
            tblsecedit.Text = "EDIT MODE";
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (update == true)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if ((bool)row["Check"] == true)
                    {
                        row["Check"] = false;
                    }
                }
                update = false;
                btnAdd.Content = "ADD NEW SECTION";
                btnDelete.Content = "DELETE";
                btnUpdate.Width = 280;
                btnUpdate.Margin = new Thickness(50, 0, 12, 280);
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                scrollYear.Visibility = Visibility.Collapsed;
                scrollSection.Visibility = Visibility.Collapsed;
                dtSection.IsEnabled = true;
                YearT.Text = "0";
                SectionT.Text = "";
                dtSection.SelectedIndex = -1;
                tblYredit.Text = "";
                tblsecedit.Text = "";
            }
        }

        void Check_Exists()
        {
            if (update == true)
            {
                if (YearT.Text != defyr || SectionT.Text != defsec)
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT `Sec_year`, `Sec_name` FROM `tblsection` WHERE `Sec_id` IN (SELECT `Sec_id` FROM `tblsection` WHERE `Sec_id` <>'" + secid + "') AND Course_id='" + courseid + "' AND Sec_year ='" + YearT.Text + "'AND Sec_name ='" + SectionT.Text + "'", con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        bdrYear.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrSection.BorderBrush = new SolidColorBrush(Colors.Red);
                        tooltip = new ToolTip { Content = "Year & Section already exist in the database." };
                        bdrYear.ToolTip = tooltip;
                        bdrSection.ToolTip = tooltip;
                        yrsec = false;
                        con.Close();
                        dr.Close();
                    }
                    else
                    {
                        bdrYear.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrSection.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrYear.ClearValue(Border.ToolTipProperty);
                        bdrSection.ClearValue(Border.ToolTipProperty);
                        con.Close();
                        dr.Close();
                        yrsec = true;
                    }
                }
                else
                {
                    bdrYear.BorderBrush = new SolidColorBrush(Colors.Black);
                    bdrSection.BorderBrush = new SolidColorBrush(Colors.Black);
                    bdrYear.ClearValue(Border.ToolTipProperty);
                    bdrSection.ClearValue(Border.ToolTipProperty);
                    btnAdd.IsEnabled = false;
                }
            }
            else if (add == true)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT `Sec_year`, `Sec_name` FROM `tblsection` WHERE  Sec_year ='" + YearT.Text + "'AND Sec_name ='" + SectionT.Text + "' AND Course_id='" + courseid + "'", con);
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    bdrYear.BorderBrush = new SolidColorBrush(Colors.Red);
                    bdrSection.BorderBrush = new SolidColorBrush(Colors.Red);
                    tooltip = new ToolTip { Content = "Year & Section already exist in the database." };
                    bdrYear.ToolTip = tooltip;
                    bdrSection.ToolTip = tooltip;
                    yrsec = false;
                    con.Close();
                    dr.Close();
                }
                else
                {
                    bdrYear.BorderBrush = new SolidColorBrush(Colors.Black);
                    bdrSection.BorderBrush = new SolidColorBrush(Colors.Black);
                    bdrYear.ClearValue(Border.ToolTipProperty);
                    bdrSection.ClearValue(Border.ToolTipProperty);
                    con.Close();
                    dr.Close();
                    yrsec = true;
                }
            }
            else
            {
                bdrYear.BorderBrush = new SolidColorBrush(Colors.Black);
                bdrSection.BorderBrush = new SolidColorBrush(Colors.Black);
                bdrYear.ClearValue(Border.ToolTipProperty);
                bdrSection.ClearValue(Border.ToolTipProperty);
            }
            Button_Save_Checker();
        }

        void Button_Save_Checker()
        {
            if (update == true)
            {
                if (YearT.Text == defyr && SectionT.Text == defsec)
                {
                    btnAdd.IsEnabled = false;
                }
                else
                {
                    if (yrsec == true)
                    {
                        btnAdd.IsEnabled = true;
                    }
                    else
                    {
                        btnAdd.IsEnabled = false;
                    }
                }
            }
            else if (add == true)
            {
                if (yrsec == true)
                {
                    btnAdd.IsEnabled = true;
                }
                else
                {
                    btnAdd.IsEnabled = false;
                }
            }
            else
            {
                btnAdd.IsEnabled = true;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (update == true && yrsec == true)
            {
                if (btnAdd.IsEnabled == true)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        cmd = new MySqlCommand("UPDATE `tblsection` SET `Sec_year`=@yr,`Sec_name`=@sc WHERE `Sec_id`='" + secid + "'", con);

                        cmd.Parameters.AddWithValue("@yr", YearT.Text);
                        cmd.Parameters.AddWithValue("@sc", SectionT.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();

                        MessageBox.Show("The data has been updated successfully.");
                        Fill_Section();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }
                    update = false;
                    btnAdd.Content = "ADD NEW SECTION";
                    btnDelete.Content = "DELETE";
                    scrollYear.Visibility = Visibility.Collapsed;
                    scrollSection.Visibility = Visibility.Collapsed;
                    btnDelete.Visibility = Visibility.Collapsed;
                    btnAdd.Visibility = Visibility.Collapsed;
                    dtSection.IsEnabled = true;
                    YearT.Text = "";
                    SectionT.Text = "";
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dtSection.SelectedItem != null)
            {
                var subject = dtSection.SelectedItem as DataRowView;
                subject.Row["Check"] = true;
            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtSection.SelectedItem != null)
            {
                var subject = dtSection.SelectedItem as DataRowView;
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Done managing sections?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

        private void SectionT_TextChanged(object sender, TextChangedEventArgs e)
        {
            Section_Value();
            Check_Exists();
        }

        private void YearT_TextChanged(object sender, TextChangedEventArgs e)
        {
            Year_Value();
            Check_Exists();
        }
        

        private void S_codeT_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchW.Visibility = Visibility.Collapsed;
        }

        private void S_codeT_LostFocus(object sender, RoutedEventArgs e)
        {
            string x = S_codeT.Text;
            string xx = x.Replace("\\s", "");
            if (xx.Length == 0)
            {
                SearchW.Visibility = Visibility.Visible;
            }
        }

        private void S_codeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("Year LIKE '{0}%' AND Section LIKE '{0}%'", S_codeT.Text);
        }
    }
}
