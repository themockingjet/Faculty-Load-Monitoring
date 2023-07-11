using MySql.Data.MySqlClient;
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
    /// Interaction logic for vSecsched.xaml
    /// </summary>
    public partial class vSecsched : Window
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        string nl = System.Environment.NewLine;
        MySqlCommand cmd;
        MySqlDataReader reader;
        MySqlDataAdapter da;

        string semester = Main.semester_id;
        string schoolyear = Main.schoolyear_id;
        public vSecsched()
        {
            InitializeComponent();

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

            ComboBox_Course();

            txtCuraysem.Text = "ACADEMIC YEAR: " + Main.main_schoolyear + "   SEMESTER: " + Main.main_semester;
        }

        #region
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

        // ================================= MAIN ===============================
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

        void ComboBox_Course()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Sec_id, CONCAT(Course_name,'-',CONCAT(IF(Sec_year ='0','',Sec_year),Sec_name)) as Section, Course_name FROM tblsched LEFT JOIN tblsection USING (Sec_id) JOIN tblcourse WHERE Sec_id IN (SELECT Sec_id FROM tblopensec) AND tblcourse.Course_id = tblsched.Course_id AND tblsched.Sem_id = '" + semester + "'AND tblsched.Sy_id = '" + schoolyear + "'", con))
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

        void Fill_Sched()
        {
            grdSchedule.Children.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                con.Open();
                if (cbYsec.SelectedIndex != -1)
                {
                    cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND Room_id IN (SELECT Room_id FROM tblopenrooms) AND sched.Sec_id ='" + cbYsec.SelectedValue.ToString() + "'AND Sem_id ='" + semester + "'AND Sy_id='" + schoolyear + "'", con);
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
                    txt.Text = ldins + nl + ldsub + " : " + ldtype + nl + ldroom;
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
        
        private string Selcourse
        {
            get
            {
                var subject = cbYsec.SelectedItem as DataRowView;
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
        
        private void CbYsec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbYsec.SelectedIndex != -1)
            {
                Fill_Sched();
                tblvSched.Text = Selcourse + " - " + selsection;
                btnExcel.IsEnabled = true;
            }
            else
            {
                Fill_Sched();
                tblvSched.Text = "";
                btnExcel.IsEnabled = false;
            }
        }
        
        private void CbYsec_TextChanged(object sender, TextChangedEventArgs e)
        {
            (cbYsec.Template.FindName("PART_EditableTextBox", cbYsec) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void CbYsec_GotFocus(object sender, RoutedEventArgs e)
        {
            cbYsec.IsDropDownOpen = true;
        }

        int printrow = 0;
        string printcol = string.Empty;

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Excel.Application oXL = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;

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



            cmd = new MySqlCommand("SELECT sched.Sched_Day, sched.Sub_type, sched.Sub_Other, sched.Sched_row, sched.Sched_Dur , ins.LName, cou.Course_name, CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name) as Section, sub.Sub_code , room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND Room_id IN (SELECT Room_id FROM tblopenrooms) AND sched.Sec_id='" + cbYsec.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "'", con);
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

            cmd = new MySqlCommand("SELECT sub.Sub_code, sub.Sub_desc, CONCAT(cou.Course_name,'-',CONCAT(if(sec.Sec_year='0','',sec.Sec_year), sec.Sec_name)) AS Section, GROUP_CONCAT( CONCAT (IF(sched.Sched_Day = 'THURSDAY', SUBSTRING(sched.Sched_Day,1,2), SUBSTRING(sched.Sched_Day,1,1)),' ',CONCAT(TIME_FORMAT(sched.Sched_Sti, '%l'),'-',CONCAT(TIME_FORMAT(sched.Sched_Eti, '%l'),TIME_FORMAT(sched.Sched_Eti, '%p')))) ORDER BY FIELD(sched.Sched_Day, 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY')) as Day, ins.LName, room.Room_name FROM tblsched sched LEFT JOIN tblins ins USING (Ins_id) LEFT JOIN tblcourse cou USING (Course_id) LEFT JOIN tblsection sec USING (Sec_id) LEFT JOIN tblsubject sub USING (Sub_id) LEFT JOIN tblroom room USING (Room_id) WHERE Ins_id IN (SELECT Ins_id FROM tblopenins) AND Room_id IN (SELECT Room_id FROM tblopenrooms) AND sched.Sec_id='" + cbYsec.SelectedValue.ToString() + "'AND sched.Sem_id ='" + semester + "'AND sched.Sy_id='" + schoolyear + "' GROUP BY sub.Sub_code ORDER BY sched.Sched_id", con);
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
    }
}
