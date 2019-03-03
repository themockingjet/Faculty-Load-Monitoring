using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mSchedule.xaml
    /// </summary>
    public partial class mSchedule : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";

        string ins_name = "";
        Border bdr;

        public mSchedule()
        {
            InitializeComponent();
            //ComboBox_Hour_E();
            ComboBox_Instructors();
            ComboBox_Rooms();
            ComboBox_Hour_S();

        }

        void ComboBox_Instructors()
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

        void ComboBox_Rooms()
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

        void ComboBox_Hour_S()
        {
            cbhour.Items.Add("7:00 AM");
            cbhour.Items.Add("7:30 AM");
            cbhour.Items.Add("8:00 AM");
            cbhour.Items.Add("8:30 AM");
            cbhour.Items.Add("9:00 AM");
            cbhour.Items.Add("9:30 AM");
            cbhour.Items.Add("10:00 AM");
            cbhour.Items.Add("10:30 AM");
            cbhour.Items.Add("11:00 AM");
            cbhour.Items.Add("11:30 AM");
            cbhour.Items.Add("12:00 PM");
            cbhour.Items.Add("12:30 PM");
            cbhour.Items.Add("1:00 PM");
            cbhour.Items.Add("1:30 PM");
            cbhour.Items.Add("2:00 PM");
            cbhour.Items.Add("2:30 PM");
            cbhour.Items.Add("3:00 PM");
            cbhour.Items.Add("3:30 PM");
            cbhour.Items.Add("4:00 PM");
            cbhour.Items.Add("4:30 PM");
            cbhour.Items.Add("5:00 PM");
            cbhour.Items.Add("5:30 PM");
            cbhour.Items.Add("6:00 PM");
            cbhour.Items.Add("6:30 PM");
            cbhour.Items.Add("7:00 PM");
            cbhour.Items.Add("7:30 PM");
            cbhour.Items.Add("8:00 PM");

            cbhour1.Items.Add("7:00 AM");
            cbhour1.Items.Add("7:30 AM");
            cbhour1.Items.Add("8:00 AM");
            cbhour1.Items.Add("8:30 AM");
            cbhour1.Items.Add("9:00 AM");
            cbhour1.Items.Add("9:30 AM");
            cbhour1.Items.Add("10:00 AM");
            cbhour1.Items.Add("10:30 AM");
            cbhour1.Items.Add("11:00 AM");
            cbhour1.Items.Add("11:30 AM");
            cbhour1.Items.Add("12:00 PM");
            cbhour1.Items.Add("12:30 PM");
            cbhour1.Items.Add("1:00 PM");
            cbhour1.Items.Add("1:30 PM");
            cbhour1.Items.Add("2:00 PM");
            cbhour1.Items.Add("2:30 PM");
            cbhour1.Items.Add("3:00 PM");
            cbhour1.Items.Add("3:30 PM");
            cbhour1.Items.Add("4:00 PM");
            cbhour1.Items.Add("4:30 PM");
            cbhour1.Items.Add("5:00 PM");
            cbhour1.Items.Add("5:30 PM");
            cbhour1.Items.Add("6:00 PM");
            cbhour1.Items.Add("6:30 PM");
            cbhour1.Items.Add("7:00 PM");
            cbhour1.Items.Add("7:30 PM");
            cbhour1.Items.Add("8:00 PM");
        }

        //void ComboBox_Hour_E()
        //{
        //    cbhoure.Items.Add("7:00 AM");
        //    cbhoure.Items.Add("7:30 AM");
        //    cbhoure.Items.Add("8:00 AM");
        //    cbhoure.Items.Add("8:30 AM");
        //    cbhoure.Items.Add("9:00 AM");
        //    cbhoure.Items.Add("9:30 AM");
        //    cbhoure.Items.Add("10:00 AM");
        //    cbhoure.Items.Add("10:30 AM");
        //    cbhoure.Items.Add("11:00 AM");
        //    cbhoure.Items.Add("11:30 AM");
        //    cbhoure.Items.Add("12:00 NN");
        //    cbhoure.Items.Add("12:30 NN");
        //    cbhoure.Items.Add("1:00 PM");
        //    cbhoure.Items.Add("1:30 PM");
        //    cbhoure.Items.Add("2:00 PM");
        //    cbhoure.Items.Add("2:30 PM");
        //    cbhoure.Items.Add("3:00 PM");
        //    cbhoure.Items.Add("3:30 PM");
        //    cbhoure.Items.Add("4:00 PM");
        //    cbhoure.Items.Add("4:30 PM");
        //    cbhoure.Items.Add("5:00 PM");
        //    cbhoure.Items.Add("5:30 PM");
        //    cbhoure.Items.Add("6:00 PM");
        //    cbhoure.Items.Add("6:30 PM");
        //    cbhoure.Items.Add("7:00 PM");
        //    cbhoure.Items.Add("7:30 PM");
        //    cbhoure.Items.Add("8:00 PM");
        //}

        void Border_Init()
        {
            bdr = new Border();
            bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            bdr.BorderThickness = new Thickness(1);
            bdr.Background = new SolidColorBrush(Colors.White);
            Grid.SetColumn(bdr, 0);
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;

            TextBlock txt1 = new TextBlock();
            txt1.Text = "2005 Products Shipped";
            txt1.FontSize = 20;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRowSpan(bdr, 4);

            grid.Children.Add(txt1);
            bdr.Child = grid;

            grdMONDAY.Children.Add(bdr);
        }

        //void Day_Col_Init()
        //{
        //    if (cbday.SelectedValue.ToString() == "MONDAY")
        //    {
        //        Grid.SetColumn(bdr, 1);
        //    }
        //    else if (cbday.SelectedValue.ToString() == "TUESDAY")
        //    {
        //        Grid.SetColumn(bdr, 2);
        //    }
        //    else if (cbday.SelectedValue.ToString() == "WEDNESDAY")
        //    {
        //        Grid.SetColumn(bdr, 3);
        //    }
        //    else if (cbday.SelectedValue.ToString() == "THURSDAY")
        //    {
        //        Grid.SetColumn(bdr, 4);
        //    }
        //    else if (cbday.SelectedValue.ToString() == "FRIDAY")
        //    {
        //        Grid.SetColumn(bdr, 5);
        //    }
        //    else if (cbday.SelectedValue.ToString() == "SATURDAY")
        //    {
        //        Grid.SetColumn(bdr, 6);
        //    }
        //}

        //void Hour_Start_Row_Init()
        //{
        //    if (cbhour.SelectedValue.ToString() == "7:00 AM")
        //    {
        //        Grid.SetRow(bdr, 2);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "7:30 AM")
        //    {
        //        Grid.SetRow(bdr, 3);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "8:00 AM")
        //    {
        //        Grid.SetRow(bdr, 4);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "8:30 AM")
        //    {
        //        Grid.SetRow(bdr, 5);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "9:00 AM")
        //    {
        //        Grid.SetRow(bdr, 6);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "9:30 AM")
        //    {
        //        Grid.SetRow(bdr, 7);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "10:00 AM")
        //    {
        //        Grid.SetRow(bdr, 8);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "10:30 AM")
        //    {
        //        Grid.SetRow(bdr, 9);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "11:00 AM")
        //    {
        //        Grid.SetRow(bdr, 10);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "11:30 AM")
        //    {
        //        Grid.SetRow(bdr, 11);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "12:00 NN")
        //    {
        //        Grid.SetRow(bdr, 12);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "12:30 NN")
        //    {
        //        Grid.SetRow(bdr, 13);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "1:00 PM")
        //    {
        //        Grid.SetRow(bdr, 14);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "1:30 PM")
        //    {
        //        Grid.SetRow(bdr, 15);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "2:00 PM")
        //    {
        //        Grid.SetRow(bdr, 16);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "2:30 PM")
        //    {
        //        Grid.SetRow(bdr, 17);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "3:00 PM")
        //    {
        //        Grid.SetRow(bdr, 18);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "3:30 PM")
        //    {
        //        Grid.SetRow(bdr, 19);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "4:00 PM")
        //    {
        //        Grid.SetRow(bdr, 20);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "4:30 PM")
        //    {
        //        Grid.SetRow(bdr, 21);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "5:00 PM")
        //    {
        //        Grid.SetRow(bdr, 22);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "5:30 PM")
        //    {
        //        Grid.SetRow(bdr, 23);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "6:00 PM")
        //    {
        //        Grid.SetRow(bdr, 24);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "6:30 PM")
        //    {
        //        Grid.SetRow(bdr, 25);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "7:00 PM")
        //    {
        //        Grid.SetRow(bdr, 26);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "7:30 PM")
        //    {
        //        Grid.SetRow(bdr, 27);
        //    }
        //    else if (cbhour.SelectedValue.ToString() == "8:00 PM")
        //    {
        //        Grid.SetRow(bdr, 28);
        //    }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //Border_Init();
        //    //Day_Col_Init();
        //    //Hour_Start_Row_Init();
        //    //Grid grid = new Grid();
        //    //grid.HorizontalAlignment = HorizontalAlignment.Stretch;
        //    //grid.VerticalAlignment = VerticalAlignment.Stretch;

        //    //TextBlock txt1 = new TextBlock();
        //    //txt1.Text = "2005 Products Shipped";
        //    //txt1.FontSize = 20;
        //    //txt1.FontWeight = FontWeights.Bold;
        //    //Grid.SetRowSpan(bdr, 4);

        //    //grid.Children.Add(txt1);
        //    //bdr.Child = grid;

        //    //grdScheds.Children.Add(bdr);


        //    //MessageBox.Show(cbInstructor.SelectedValue.ToString());

        //}

        private void BtnView_MouseEnter(object sender, MouseEventArgs e)
        {
            btnView.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void BtnView_MouseLeave(object sender, MouseEventArgs e)
        {
            btnView.Foreground = new SolidColorBrush(Colors.White);
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            vSchedule UCobj = new vSchedule();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    (window as Schedule).bdrContent.Children.Add(UCobj);
                    (window as Schedule).bdrContent.Children.Remove(this);
                }
            }
        }

        private void BtnSave_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSave.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void BtnSave_MouseLeave(object sender, MouseEventArgs e)
        {
            btnSave.Foreground = new SolidColorBrush(Colors.White);
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
                        if (dr.GetString(4) == string.Empty)
                        {
                            ins_name = fname + " " + lname;
                        }
                        else
                        {
                            string mname = dr.GetString(4);
                            ins_name = fname + " " + mname + " " + lname;
                        }
                    }
                }
            }
            if (cbRoom.SelectedIndex == -1)
            {
                tbltwo.Text = ins_name;
                bdrOne.BorderThickness = new Thickness(0);
                bdrZero.BorderThickness = new Thickness(0);
            }
            else
            {
                bdrOne.BorderThickness = new Thickness(1);
                bdrZero.BorderThickness = new Thickness(1);
                tbltwo.Text = "";
                tblzero.Text = ins_name;
                tblone.Text = cbRoom.SelectedValue.ToString();
            }
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    (window as Schedule).manage_instructor_name = ins_name;
                }
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
                tblzero.Text = ins_name;
                tblone.Text = cbRoom.SelectedValue.ToString();
            }
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    (window as Schedule).manage_room_name = cbRoom.SelectedValue.ToString();
                }
            }
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
            //var element = (UIElement)e.Source;
            
            //int rows = Grid.GetRow(grdmScheds);
            //MessageBox.Show(grdmScheds.ColumnDefinitions.Count().ToString());

            foreach (UIElement ele in grdMONDAY.Children)
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


                    //MessageBox.Show(time1.ToString("h:mm tt"));

                    //MessageBox.Show(c.ToString() + "," + r.ToString() + "," + rs.ToString());
                    //if (bdrx.Child != null)
                    //{
                    //    //pnl = bdrx.Child as StackPanel;

                    //}

                    //if (pnl != null)
                    //{
                    //    foreach (TextBlock txt in pnl.Children)
                    //    {
                    //        MessageBox.Show(txt.Text);
                    //    }
                    //}
                    //MessageBox.Show(cbhour.SelectedIndex.ToString());

                    //======================

                }

            }


            //foreach (UIElement ele in grdmScheds.Children)
            //{
            //    if (ele.GetType() == typeof(Border))
            //    {
            //        //StackPanel pnl = null;
            //        Border bdrx = (Border)ele;
            //        int c = Grid.GetColumn(bdrx);
            //        int r = Grid.GetRow(bdrx);
            //        int rs = Grid.GetRowSpan(bdrx);

            //        string x = bdrx.Name.Substring(1);
            //        TimeSpan ts = DateTime.ParseExact(x, "h_mm_tt", CultureInfo.InvariantCulture).TimeOfDay;
            //        for (int i = 0; i <= (rs - 1); i++)
            //        {
            //            DateTime time1 = DateTime.Today.Add(ts);
            //            cbhour.Items.Remove(time1.ToString("h:mm tt"));
            //            ts = ts + TimeSpan.FromMinutes(30);
            //        }


            //        //MessageBox.Show(time1.ToString("h:mm tt"));

            //        //MessageBox.Show(c.ToString() + "," + r.ToString() + "," + rs.ToString());
            //        //if (bdrx.Child != null)
            //        //{
            //        //    //pnl = bdrx.Child as StackPanel;

            //        //}

            //        //if (pnl != null)
            //        //{
            //        //    foreach (TextBlock txt in pnl.Children)
            //        //    {
            //        //        MessageBox.Show(txt.Text);
            //        //    }
            //        //}
            //        //MessageBox.Show(cbhour.SelectedIndex.ToString());

            //        //======================

            //    }

            //}


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
                        cbhour.Items.Remove(x);
                    }
                }
            }
        }
    }
}
