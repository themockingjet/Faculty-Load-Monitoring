using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for mRoom.xaml
    /// </summary>
    public partial class mRoom : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlDataReader reader;

        string roomid = string.Empty;
        bool clicked = false;

        string sroom = string.Empty;
        string stype = string.Empty;
        string scap = string.Empty;

        bool room = true;
        bool type = true;
        bool cap = true;
        bool hasrow = false;

        public mRoom()
        {
            InitializeComponent();
            Update_Room_Id();
            Room_Type();
            ToolTips();
        }

        void Room_Type()
        {
            cbType.Items.Add("LAB");
            cbType.Items.Add("LEC");
        }

        void Update_Room_Id()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Rooms))
                {
                    if ((window as Rooms).roomid != null)
                    {
                        roomid = (window as Rooms).roomid;
                        if (roomid != string.Empty)
                        {
                            Load_Room_Details();
                            btnSave.Content = "UPDATE";
                            btnSave.IsEnabled = false;
                        }
                    }
                }
            }
           
        }

        private void Load_Room_Details()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("select * from tblroom where Room_id='" + roomid + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string room = dr[1].ToString();
                            sroom = room;
                            if (room.Contains('-'))
                            {
                                string[] roomsplit = room.Split(new char[] { ' ', '-' });
                                RoomT.Text = roomsplit[0];
                                RoomT2.Text = roomsplit[3];
                            }
                            else
                            {
                                RoomT2.Text = room;
                            }
                            cbType.SelectedValue = dr[2].ToString();
                            cbType.Text = dr[2].ToString();
                            stype = dr[2].ToString();
                            CapT.Text = dr[3].ToString();
                            scap = dr[3].ToString();
                        }
                    }
                }
            }
        }

        void LetterNumber(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[A-Za-z0-9]$"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

        void LetterOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[A-Za-z]$"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

        void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]$"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

        void ToolTips()
        {
            ttRoom.Text = "Room Name - Room Number\ne.g.:\tCL - 1\n\tLR - 5\n(blank room name) - Room Number\ne.g.:\t(Blank) - 202";
            ttType.Text = "Indicate if the room is suitable\nfor Laboratory or Lecture.";
            ttCap.Text = "Determines the capacity of the room.";
        }

        void Check_Room_Exist()
        {
            if (roomid != string.Empty)
            {
                string joinroom = string.Empty;
                string joinroom2 = string.Empty;
                if (RoomT.Text == "" && RoomT2.Text != "")
                {
                    joinroom = RoomT2.Text;
                    if (joinroom != sroom)
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        con.Open();
                        cmd = new MySqlCommand("SELECT * FROM tblroom WHERE Room_id IN (SELECT  Room_id FROM tblroom WHERE  Room_id <> '" + roomid + "') AND Room_name='" + joinroom + "'", con);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            room = false;
                            hasrow = true;
                        }
                        else
                        {
                            room = true;
                            hasrow = false;
                        }
                    }
                }
                else if (RoomT.Text != "" && RoomT2.Text != "")
                {
                    joinroom2 = RoomT.Text + " - " + RoomT2.Text;
                    if (joinroom2 != sroom)
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        con.Open();
                        cmd = new MySqlCommand("SELECT * FROM tblroom WHERE Room_id IN (SELECT  Room_id FROM tblroom WHERE  Room_id <> '" + roomid + "') AND Room_name='" + joinroom2 + "'", con);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            room = false;
                            hasrow = true;
                        }
                        else
                        {
                            room = true;
                            hasrow = false;
                        }
                    }
                }
                else if (RoomT.Text != "" && RoomT2.Text == "")
                {
                    joinroom2 = RoomT.Text + " - " + RoomT2.Text;
                    if (joinroom2 != sroom)
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        con.Open();
                        cmd = new MySqlCommand("SELECT * FROM tblroom WHERE Room_id IN (SELECT  Room_id FROM tblroom WHERE  Room_id <> '" + roomid + "') AND Room_name='" + joinroom2 + "'", con);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            room = false;
                            hasrow = true;
                        }
                        else
                        {
                            room = true;
                            hasrow = false;
                        }
                    }
                }
            }
            else
            {
                string joinroom = string.Empty;
                string joinroom2 = string.Empty;
                if (RoomT.Text == "" && RoomT2.Text != "")
                {
                    joinroom = RoomT2.Text;
                    MySqlConnection con = new MySqlConnection(Connection);
                    con.Open();
                    cmd = new MySqlCommand("SELECT * FROM tblroom WHERE Room_name='" + joinroom + "'", con);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        room = false;
                        hasrow = true;
                    }
                    else
                    {
                        room = true;
                        hasrow = false;
                    }
                }
                else if (RoomT.Text != "" && RoomT2.Text != "")
                {
                    joinroom2 = RoomT.Text + " - " + RoomT2.Text;
                    MySqlConnection con = new MySqlConnection(Connection);
                    con.Open();
                    cmd = new MySqlCommand("SELECT * FROM tblroom WHERE Room_name='" + joinroom2 + "'", con);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        room = false;
                        hasrow = true;

                    }
                    else
                    {
                        room = true;
                        hasrow = false;
                    }
                }
                else if (RoomT.Text != "" && RoomT2.Text == "")
                {
                    joinroom2 = RoomT.Text + " - " + RoomT2.Text;
                    MySqlConnection con = new MySqlConnection(Connection);
                    con.Open();
                    cmd = new MySqlCommand("SELECT * FROM tblroom WHERE Room_name='" + joinroom2 + "'", con);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        room = false;
                        hasrow = true;

                    }
                    else
                    {
                        room = true;
                        hasrow = false;
                    }
                }

            }
            Save_Check();
        }

        void Save_Check()
        {
            string joinroom = string.Empty;
            string joinroom2 = string.Empty;
            if (roomid != string.Empty)
            {
                if(cbType.SelectedIndex != -1)
                {
                    if (RoomT.Text == "" && RoomT2.Text != "")
                    {
                        joinroom = RoomT2.Text;
                        if (joinroom == sroom && cbType.SelectedValue.ToString() == stype && CapT.Text == scap)
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
                    else if (RoomT.Text != "" && RoomT.Text != "")
                    {
                        joinroom2 = RoomT.Text + " - " + RoomT2.Text;
                        if (joinroom2 == sroom && cbType.SelectedValue.ToString() == stype && CapT.Text == scap)
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
                    else if (RoomT.Text != "" && RoomT.Text == "")
                    {
                        btnSave.IsEnabled = false;
                    }
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

        private void CapT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrCap.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CapT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (CapT.Text == "")
                {
                    ToolTip tt = new ToolTip { Content = "Capacity cannot be blank." };
                    bdrCap.BorderBrush = new SolidColorBrush(Colors.Red);
                    bdrCap.ToolTip = tt;
                }
                else
                {
                    bdrCap.BorderBrush = Brushes.Black;
                    bdrCap.ClearValue(Border.ToolTipProperty);
                }
            }
            else
            {
                bdrCap.BorderBrush = Brushes.Black;
            }
        }

        private void RoomT2_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrRoom2.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void RoomT2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (RoomT2.Text == "")
                {
                    ToolTip tt = new ToolTip { Content = "Type cannot be blank." };
                    bdrRoom2.BorderBrush = new SolidColorBrush(Colors.Red);
                    bdrRoom2.ToolTip = tt;
                }
                else
                {
                    bdrRoom.BorderBrush = Brushes.Black;
                    bdrRoom2.BorderBrush = Brushes.Black;
                    bdrRoom2.ClearValue(Border.ToolTipProperty);
                }
            }
            else
            {
                bdrRoom2.BorderBrush = Brushes.Black;
            }
        }

        private void CbType_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrType.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (clicked == true)
            {
                if (cbType.SelectedIndex == -1)
                {
                    ToolTip tt = new ToolTip { Content = "Type cannot be blank." };
                    bdrType.BorderBrush = new SolidColorBrush(Colors.Red);
                    bdrType.ToolTip = tt;
                }
                else
                {
                    bdrType.BorderBrush = Brushes.Black;
                    bdrType.ClearValue(Border.ToolTipProperty);
                }
            }
            else
            {
                bdrType.BorderBrush = Brushes.Black;
            }
        }

        private void RoomT_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrRoom.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void RoomT_LostFocus(object sender, RoutedEventArgs e)
        {
            bdrRoom.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void RoomT_TextChanged(object sender, TextChangedEventArgs e)
        {
            Check_Room_Exist();
            if (clicked == true)
            {
                if (RoomT2.Text == "" && RoomT.Text == "")
                {
                    bdrRoom.BorderBrush = Brushes.Red;
                    bdrRoom2.BorderBrush = Brushes.Red;
                    ppRoomtxt.Text = "Room is required";
                    ppRoom.Visibility = Visibility.Visible;
                    room = false;
                }
                else if (RoomT2.Text != "" && RoomT.Text == "")
                {
                    bdrRoom.BorderBrush = Brushes.Black;
                    bdrRoom2.BorderBrush = Brushes.Black;
                    ppRoom.Visibility = Visibility.Collapsed;
                    ppRoomNumber.Visibility = Visibility.Collapsed;
                    room = true;
                }
                else if (RoomT2.Text == "" && RoomT.Text != "")
                {
                    bdrRoom.BorderBrush = Brushes.Red;
                    bdrRoom2.BorderBrush = Brushes.Red;
                    ppRoom.Visibility = Visibility.Collapsed;
                    ppRoomNumber.Visibility = Visibility.Visible;
                    room = false;
                }
                else if (RoomT2.Text != "" && RoomT.Text != "")
                {
                    if (hasrow != true)
                    {
                        ppRoomtxt.Text = "Room is required";
                        ppRoom.Visibility = Visibility.Collapsed;
                        ppRoomNumber.Visibility = Visibility.Collapsed;
                        bdrRoom2.BorderBrush = Brushes.Black;
                        bdrRoom.BorderBrush = Brushes.Black;
                        room = true;
                    }
                    else
                    {
                        ppRoomtxt.Text = "Room already exist";
                        ppRoomNumber.Visibility = Visibility.Collapsed;
                        ppRoom.Visibility = Visibility.Visible;
                        bdrRoom2.BorderBrush = Brushes.Red;
                        bdrRoom.BorderBrush = Brushes.Red;
                        room = false;
                    }
                }
            }
            else
            {
                if (hasrow != true)
                {
                    ppRoomtxt.Text = "Room is required";
                    ppRoom.Visibility = Visibility.Collapsed;
                    bdrRoom2.BorderBrush = Brushes.Black;
                    bdrRoom.BorderBrush = Brushes.Black;
                }
                else
                {
                    ppRoomtxt.Text = "Room already exist";
                    ppRoom.Visibility = Visibility.Visible;
                    bdrRoom2.BorderBrush = Brushes.Red;
                    bdrRoom.BorderBrush = Brushes.Red;
                }
            }
        }

        private void RoomT2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Check_Room_Exist();
            if (clicked == true)
            {
                if (RoomT2.Text == "" && RoomT.Text == "")
                {
                    bdrRoom.BorderBrush = Brushes.Red;
                    bdrRoom2.BorderBrush = Brushes.Red;
                    ppRoomtxt.Text = "Room is required";
                    ppRoom.Visibility = Visibility.Visible;
                    room = false;
                }
                else if (RoomT2.Text != "" && RoomT.Text == "")
                {
                    bdrRoom.BorderBrush = Brushes.Black;
                    bdrRoom2.BorderBrush = Brushes.Black;
                    ppRoom.Visibility = Visibility.Collapsed;
                    ppRoomNumber.Visibility = Visibility.Collapsed;
                    room = true;
                }
                else if (RoomT2.Text == "" && RoomT.Text != "")
                {
                    bdrRoom.BorderBrush = Brushes.Red;
                    bdrRoom2.BorderBrush = Brushes.Red;
                    ppRoom.Visibility = Visibility.Collapsed;
                    ppRoomNumber.Visibility = Visibility.Visible;
                    room = false;
                }
                else if (RoomT2.Text != "" && RoomT.Text != "")
                {
                    if (hasrow != true)
                    {
                        ppRoomtxt.Text = "Room is required";
                        ppRoom.Visibility = Visibility.Collapsed;
                        ppRoomNumber.Visibility = Visibility.Collapsed;
                        bdrRoom2.BorderBrush = Brushes.Black;
                        bdrRoom.BorderBrush = Brushes.Black;
                        room = true;
                    }
                    else
                    {
                        ppRoomtxt.Text = "Room already exist";
                        ppRoomNumber.Visibility = Visibility.Collapsed;
                        ppRoom.Visibility = Visibility.Visible;
                        bdrRoom2.BorderBrush = Brushes.Red;
                        bdrRoom.BorderBrush = Brushes.Red;
                        room = false;
                    }
                }
            }
            else
            {
                if (hasrow != true)
                {
                    ppRoomtxt.Text = "Room is required";
                    ppRoom.Visibility = Visibility.Collapsed;
                    bdrRoom2.BorderBrush = Brushes.Black;
                    bdrRoom.BorderBrush = Brushes.Black;
                }
                else
                {
                    ppRoomtxt.Text = "Room already exist";
                    ppRoom.Visibility = Visibility.Visible;
                    bdrRoom2.BorderBrush = Brushes.Red;
                    bdrRoom.BorderBrush = Brushes.Red;
                }
            }
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (cbType.SelectedIndex == -1)
                {
                    bdrType.BorderBrush = new SolidColorBrush(Colors.Red);
                    ppType.Visibility = Visibility.Visible;
                    type = false;
                }
                else
                {
                    bdrType.BorderBrush = Brushes.Black;
                    ppType.Visibility = Visibility.Collapsed;
                    type = true;
                }
            }
            else
            {
                bdrType.BorderBrush = Brushes.Black;
            }
            Check_Room_Exist();
        }

        private void CapT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (clicked == true)
            {
                if (CapT.Text == "")
                {
                    ToolTip tt = new ToolTip { Content = "Capacity cannot be blank." };
                    bdrCap.ToolTip = tt;
                }
                else
                {
                    bdrCap.ClearValue(Border.ToolTipProperty);
                    cap = true;
                }
            }
            Check_Room_Exist();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            clicked = true;
            if (cbType.SelectedIndex == -1)
            {
                ppType.Visibility = Visibility.Visible;
                bdrType.BorderBrush = Brushes.Red;
                type = true;
            }
            if (RoomT.Text != "" && RoomT2.Text == "")
            {
                ppRoomNumber.Visibility = Visibility.Visible;
                bdrRoom2.BorderBrush = Brushes.Red;
                room = false;
            }
            else if (RoomT.Text == "" && RoomT2.Text == "")
            {
                ppRoom.Visibility = Visibility.Visible;
                bdrRoom.BorderBrush = Brushes.Red;
                bdrRoom2.BorderBrush = Brushes.Red;
                room = false;
            }
            if (CapT.Text == "")
            {
                ppCap.Visibility = Visibility.Visible;
                bdrCap.BorderBrush = Brushes.Red;
                cap = false;
            }
            
            if (roomid != string.Empty)
            {
                if (room == true && type == true && cap == true)
                {
                    string joinroom = string.Empty;
                    if (RoomT.Text == "" && RoomT2.Text != "")
                    {
                        joinroom = RoomT2.Text;
                    }
                    else if (RoomT.Text != "" && RoomT.Text != "")
                    {
                        joinroom = RoomT.Text + " - " + RoomT2.Text;
                    }
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        cmd = new MySqlCommand("UPDATE `tblroom` SET `Room_name`=@room,`Room_type`=@type,`Room_cap`=@cap WHERE Room_id ='" + roomid + "'", con);
                        cmd.Parameters.AddWithValue("@room", joinroom);
                        cmd.Parameters.AddWithValue("@type", cbType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@cap", CapT.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();
                        MessageBox.Show("The data has been updated successfully.");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Rooms))
                            {
                                (window as Rooms).grdcontent.Children.Remove(this);
                                (window as Rooms).Load_Rooms();
                                (window as Rooms).bdrContent.IsEnabled = true;
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
                if (room == true && type == true && cap == true)
                {
                    string joinroom = string.Empty;
                    if (RoomT.Text == "" && RoomT2.Text != "")
                    {
                        joinroom = RoomT2.Text;
                    }
                    else if (RoomT.Text != "" && RoomT.Text != "")
                    {
                        joinroom = RoomT.Text + " - " + RoomT2.Text;
                    }
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        con.Open();
                        cmd = new MySqlCommand("INSERT INTO `tblroom`(`Room_name`, `Room_type`, `Room_cap`) VALUES (@room,@type,@cap) ", con);

                        cmd.Parameters.AddWithValue("@room", joinroom);
                        cmd.Parameters.AddWithValue("@type", cbType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@cap", CapT.Text);
                        cmd.ExecuteNonQuery();
                        con.Dispose();

                        MessageBox.Show("The data has been saved successfully.");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Rooms))
                            {
                                (window as Rooms).grdcontent.Children.Remove(this);
                                (window as Rooms).Load_Rooms();
                                (window as Rooms).bdrContent.IsEnabled = true;
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
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (roomid != string.Empty)
            {
                if (btnSave.IsEnabled == true)
                {
                    MessageBoxResult result = MessageBox.Show("Any changes will not be saved. Do you want to continue?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Rooms))
                            {
                                (window as Rooms).grdcontent.Children.Remove(this);
                                (window as Rooms).bdrContent.IsEnabled = true;
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
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Rooms))
                        {
                            (window as Rooms).grdcontent.Children.Remove(this);
                            (window as Rooms).bdrContent.IsEnabled = true;
                        }
                    }
                }
            }
            else
            {
                if (RoomT.Text != "" || RoomT2.Text != "" || cbType.SelectedIndex != -1 || CapT.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("Any changes will not be saved. Do you want to continue?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Rooms))
                            {
                                (window as Rooms).grdcontent.Children.Remove(this);
                                (window as Rooms).bdrContent.IsEnabled = true;
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
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Rooms))
                        {
                            (window as Rooms).grdcontent.Children.Remove(this);
                            (window as Rooms).bdrContent.IsEnabled = true;
                        }
                    }
                }
            }
        }

    }
}
