using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for vSchedule.xaml
    /// </summary>
    public partial class vSchedule : UserControl
    {
        Border bdr;
        public vSchedule()
        {
            InitializeComponent();
            ComboBox_Day();
            ComboBox_Hour_E();
        }
        

        void ComboBox_Hour_E()
        {
            cbhoure.Items.Add("7:00 AM");
            cbhoure.Items.Add("7:30 AM");
            cbhoure.Items.Add("8:00 AM");
            cbhoure.Items.Add("8:30 AM");
            cbhoure.Items.Add("9:00 AM");
            cbhoure.Items.Add("9:30 AM");
            cbhoure.Items.Add("10:00 AM");
            cbhoure.Items.Add("10:30 AM");
            cbhoure.Items.Add("11:00 AM");
            cbhoure.Items.Add("11:30 AM");
            cbhoure.Items.Add("12:00 NN");
            cbhoure.Items.Add("12:30 NN");
            cbhoure.Items.Add("1:00 PM");
            cbhoure.Items.Add("1:30 PM");
            cbhoure.Items.Add("2:00 PM");
            cbhoure.Items.Add("2:30 PM");
            cbhoure.Items.Add("3:00 PM");
            cbhoure.Items.Add("3:30 PM");
            cbhoure.Items.Add("4:00 PM");
            cbhoure.Items.Add("4:30 PM");
            cbhoure.Items.Add("5:00 PM");
            cbhoure.Items.Add("5:30 PM");
            cbhoure.Items.Add("6:00 PM");
            cbhoure.Items.Add("6:30 PM");
            cbhoure.Items.Add("7:00 PM");
            cbhoure.Items.Add("7:30 PM");
            cbhoure.Items.Add("8:00 PM");
        }

        void ComboBox_Day()
        {
            cbday.Items.Add("MONDAY");
            cbday.Items.Add("TUESDAY");
            cbday.Items.Add("WEDNESDAY");
            cbday.Items.Add("THURSDAY");
            cbday.Items.Add("FRIDAY");
            cbday.Items.Add("SATURDAY");
        }

        void Border_Init()
        {
            bdr = new Border();
            bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            bdr.BorderThickness = new Thickness(1);
            bdr.Background = new SolidColorBrush(Colors.White);
        }

        void Day_Col_Init()
        {
            if(cbday.SelectedValue.ToString() == "MONDAY")
            {
                Grid.SetColumn(bdr, 1);
            }
            else if(cbday.SelectedValue.ToString() == "TUESDAY")
            {
                Grid.SetColumn(bdr, 2);
            }
            else if (cbday.SelectedValue.ToString() == "WEDNESDAY")
            {
                Grid.SetColumn(bdr, 3);
            }
            else if (cbday.SelectedValue.ToString() == "THURSDAY")
            {
                Grid.SetColumn(bdr, 4);
            }
            else if (cbday.SelectedValue.ToString() == "FRIDAY")
            {
                Grid.SetColumn(bdr, 5);
            }
            else if (cbday.SelectedValue.ToString() == "SATURDAY")
            {
                Grid.SetColumn(bdr, 6);
            }
        }

        void Hour_Start_Row_Init()
        {
            if (cbhour.SelectedValue.ToString() == "7:00 AM")
            {
                Grid.SetRow(bdr, 2);
            }
            else if (cbhour.SelectedValue.ToString() == "7:30 AM")
            {
                Grid.SetRow(bdr, 3);
            }
            else if (cbhour.SelectedValue.ToString() == "8:00 AM")
            {
                Grid.SetRow(bdr, 4);
            }
            else if (cbhour.SelectedValue.ToString() == "8:30 AM")
            {
                Grid.SetRow(bdr, 5);
            }
            else if (cbhour.SelectedValue.ToString() == "9:00 AM")
            {
                Grid.SetRow(bdr, 6);
            }
            else if (cbhour.SelectedValue.ToString() == "9:30 AM")
            {
                Grid.SetRow(bdr, 7);
            }
            else if (cbhour.SelectedValue.ToString() == "10:00 AM")
            {
                Grid.SetRow(bdr, 8);
            }
            else if (cbhour.SelectedValue.ToString() == "10:30 AM")
            {
                Grid.SetRow(bdr, 9);
            }
            else if (cbhour.SelectedValue.ToString() == "11:00 AM")
            {
                Grid.SetRow(bdr, 10);
            }
            else if (cbhour.SelectedValue.ToString() == "11:30 AM")
            {
                Grid.SetRow(bdr, 11);
            }
            else if (cbhour.SelectedValue.ToString() == "12:00 NN")
            {
                Grid.SetRow(bdr, 12);
            }
            else if (cbhour.SelectedValue.ToString() == "12:30 NN")
            {
                Grid.SetRow(bdr, 13);
            }
            else if (cbhour.SelectedValue.ToString() == "1:00 PM")
            {
                Grid.SetRow(bdr, 14);
            }
            else if (cbhour.SelectedValue.ToString() == "1:30 PM")
            {
                Grid.SetRow(bdr, 15);
            }
            else if (cbhour.SelectedValue.ToString() == "2:00 PM")
            {
                Grid.SetRow(bdr, 16);
            }
            else if (cbhour.SelectedValue.ToString() == "2:30 PM")
            {
                Grid.SetRow(bdr, 17);
            }
            else if (cbhour.SelectedValue.ToString() == "3:00 PM")
            {
                Grid.SetRow(bdr, 18);
            }
            else if (cbhour.SelectedValue.ToString() == "3:30 PM")
            {
                Grid.SetRow(bdr, 19);
            }
            else if (cbhour.SelectedValue.ToString() == "4:00 PM")
            {
                Grid.SetRow(bdr, 20);
            }
            else if (cbhour.SelectedValue.ToString() == "4:30 PM")
            {
                Grid.SetRow(bdr, 21);
            }
            else if (cbhour.SelectedValue.ToString() == "5:00 PM")
            {
                Grid.SetRow(bdr, 22);
            }
            else if (cbhour.SelectedValue.ToString() == "5:30 PM")
            {
                Grid.SetRow(bdr, 23);
            }
            else if (cbhour.SelectedValue.ToString() == "6:00 PM")
            {
                Grid.SetRow(bdr, 24);
            }
            else if (cbhour.SelectedValue.ToString() == "6:30 PM")
            {
                Grid.SetRow(bdr, 25);
            }
            else if (cbhour.SelectedValue.ToString() == "7:00 PM")
            {
                Grid.SetRow(bdr, 26);
            }
            else if (cbhour.SelectedValue.ToString() == "7:30 PM")
            {
                Grid.SetRow(bdr, 27);
            }
            else if (cbhour.SelectedValue.ToString() == "8:00 PM")
            {
                Grid.SetRow(bdr, 28);
            }
        }

        void Duration()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Border_Init();
            //Day_Col_Init();
            //Hour_Start_Row_Init();
            ////Grid grid = new Grid();
            ////grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            ////grid.VerticalAlignment = VerticalAlignment.Stretch;

            ////TextBlock txt1 = new TextBlock();
            ////txt1.Text = "2005 Products Shipped";
            ////txt1.FontSize = 20;
            ////txt1.FontWeight = FontWeights.Bold;
            //Grid.SetRowSpan(bdr, 4);

            ////grid.Children.Add(txt1);
            ////bdr.Child = grid;

            //grdScheds.Children.Add(bdr);
            string time = "9:00 PM";
            //var dt = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan ts = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            ts = ts + TimeSpan.FromHours(4);
            DateTime time1 = DateTime.Today.Add(ts);
            //MessageBox.Show(dt.ToString("hh:mm tt", CultureInfo.CurrentCulture));
            //MessageBox.Show(dt.ToString("hh:mm tt", CultureInfo.CurrentCulture));
            MessageBox.Show(time1.ToString("h:mm tt"));
            MessageBox.Show(ts.ToString());
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
            mSchedule UCobj = new mSchedule();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Schedule))
                {
                    (window as Schedule).bdrContent.Children.Add(UCobj);
                    (window as Schedule).bdrContent.Children.Remove(this);
                }
            }
        }
    }
}
