using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mIns.xaml
    /// </summary>
    public partial class mIns : UserControl
    {
        string ins_idm = string.Empty;

        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;

        string reg_id = "";
        string prov_id = "";
        string muncit_id = "";
        string submun_id = "";
        bool sub_mun = false;

        ToolTip tooltip;
        bool bool_clicked = false;
        
        bool bool_reg = false;
        bool bool_prov = false;
        bool bool_cit = false;
        bool bool_scit = false;
        bool bool_brgy = false;
        bool gen_name = false;
        bool hasrow = false;

        string title_up = string.Empty;
        string fname_up = string.Empty;
        string mname_up = string.Empty;
        string lname_up = string.Empty;
        string reg_up = string.Empty;
        string prov_up = string.Empty;
        string cit_up = string.Empty;
        string scit_up = string.Empty;
        string brgy_up = string.Empty;
        string st_up = string.Empty;
        string con_up = string.Empty;

        public mIns()
        {
            InitializeComponent();

            ComboBox_Title();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Instructors))
                {
                    if ((window as Instructors).id_ins_update != null)
                    {
                        ins_idm = (window as Instructors).id_ins_update;
                    }
                }
            }
            
            if (ins_idm != string.Empty)
            {
                Load_Ins();
                btnSave.Content = "UPDATE";
                btnSave.IsEnabled = false;
            }

            Load_Region();

        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main_Grid.Focus();
        }

        // Load Selected Instructor
        void Load_Ins()
        {
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("select * from tblins where Ins_id='" + ins_idm + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cbTitle.Text = dr[1].ToString();
                            title_up = dr[1].ToString();

                            tbxFname.Text = dr[2].ToString();
                            fname_up = dr[2].ToString();
                            tbxMname.Text = dr[3].ToString();
                            mname_up = dr[3].ToString();
                            tbxLname.Text = dr[4].ToString();
                            lname_up = dr[4].ToString();
                            cbRegion.Text = dr[5].ToString();
                            cbRegion.SelectedValue = dr[5].ToString();
                            reg_up = dr[5].ToString();

                            cbProvince.Text = dr[6].ToString();
                            cbProvince.SelectedValue = dr[6].ToString();
                            prov_up = dr[6].ToString();

                            cbCitmun.Text = dr[7].ToString();
                            cbCitmun.SelectedValue = dr[7].ToString();
                            cit_up = dr[7].ToString();

                            cbSubmun.Text = dr[8].ToString();
                            cbSubmun.SelectedValue = dr[8].ToString();
                            scit_up = dr[8].ToString();

                            cbBrgy.Text = dr[9].ToString();
                            cbBrgy.SelectedValue = dr[9].ToString();
                            brgy_up = dr[9].ToString();

                            tbxStreet.Text = dr[10].ToString();
                            st_up = dr[10].ToString();

                            tbxContact.Text = dr[11].ToString();
                            con_up = dr[11].ToString();


                        }
                    }
                }
            }
        }

        void ComboBox_Title()
        {
            cbTitle.Items.Add("MR.");
            cbTitle.Items.Add("MS.");
        }

        void Load_Region()
        {   
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("select * from tblreg", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string reg = dr.GetString(1);
                        cbRegion.Items.Add(reg);
                    }
                }
            }
        }

        void Load_Province()
        {
            cbProvince.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
               cmd = new MySqlCommand("select * from tblprov where reg_id='" + reg_id + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string prov = dr.GetString(1);
                        cbProvince.Items.Add(prov);
                    }
                }
            }
        }

        void Load_CitMun()
        {
            cbCitmun.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("select * from tblmuncit where prov_id='" + prov_id + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string mcit = dr.GetString(1);
                        cbCitmun.Items.Add(mcit);
                    }
                }
            }
        }

        void Load_Subcm()
        {
            cbSubmun.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
               cmd = new MySqlCommand("select * from tblsubmun where muncit_id='" + muncit_id + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string smcit = dr.GetString(1);
                        cbSubmun.Items.Add(smcit);
                    }
                }
            }
        }

        void Load_Brgy()
        {
            cbBrgy.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("select * from tblbgy where muncit_id='" + muncit_id + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string bgy = dr.GetString(1);
                        cbBrgy.Items.Add(bgy);
                    }
                }
            }
        }

        void Load_Sbrgy()
        {
            cbBrgy.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(Connection))
            {
                cmd = new MySqlCommand("select * from tblsubbgy where submun_id='" + submun_id + "'", con);
                con.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string sbgy = dr.GetString(1);
                        cbBrgy.Items.Add(sbgy);
                    }
                }
            }
        }

        void Name_Check()
        {
            if(ins_idm != string.Empty)
            {
                if (cbTitle.SelectedIndex != -1 && FnameT.Text != "" && LnameT.Text != "")
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT `Ins_id`,`title`, `FName`, `MName`, `LName` FROM tblins WHERE Ins_id IN (SELECT `Ins_id` FROM tblins WHERE Ins_id <> '" + ins_idm +"') AND title = '" + cbTitle.SelectedValue.ToString() + "'AND FName = '" + FnameT.Text + "'AND MName = '" + MnameT.Text + "'AND LName = '" + LnameT.Text + "'", con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        bdrTitle.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrFname.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrMname.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrLname.BorderBrush = new SolidColorBrush(Colors.Red);
                        tooltip = new ToolTip { Content = "Instructor already exist in the database." };
                        bdrTitle.ToolTip = tooltip;
                        bdrFname.ToolTip = tooltip;
                        bdrMname.ToolTip = tooltip;
                        bdrLname.ToolTip = tooltip;
                        gen_name = false;
                        con.Close();
                        dr.Close();
                    }
                    else
                    {
                        bdrTitle.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrFname.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrMname.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrLname.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrTitle.ClearValue(Border.ToolTipProperty);
                        bdrFname.ClearValue(Border.ToolTipProperty);
                        bdrMname.ClearValue(Border.ToolTipProperty);
                        bdrLname.ClearValue(Border.ToolTipProperty);
                        con.Close();
                        dr.Close();
                        gen_name = true;
                    }
                }
            }
            else
            {
                if (cbTitle.SelectedIndex != -1 && FnameT.Text != "" && LnameT.Text != "")
                {
                    MySqlConnection con = new MySqlConnection(Connection);
                    cmd = new MySqlCommand("SELECT `title`, `FName`, `MName`, `LName` FROM `tblins`"
                                              + "WHERE title='" + cbTitle.SelectedValue.ToString() + "'AND FName='" + FnameT.Text + "'AND MName ='" + MnameT.Text + "'AND LName ='" + LnameT.Text + "'", con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        bdrTitle.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrFname.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrMname.BorderBrush = new SolidColorBrush(Colors.Red);
                        bdrLname.BorderBrush = new SolidColorBrush(Colors.Red);
                        tooltip = new ToolTip { Content = "Instructor already exist in the database." };
                        bdrTitle.ToolTip = tooltip;
                        bdrFname.ToolTip = tooltip;
                        bdrMname.ToolTip = tooltip;
                        bdrLname.ToolTip = tooltip;
                        gen_name = false;
                        hasrow = true;
                        con.Close();
                        dr.Close();
                    }
                    else
                    {
                        bdrTitle.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrFname.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrMname.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrLname.BorderBrush = new SolidColorBrush(Colors.Black);
                        bdrTitle.ClearValue(Border.ToolTipProperty);
                        bdrFname.ClearValue(Border.ToolTipProperty);
                        bdrMname.ClearValue(Border.ToolTipProperty);
                        bdrLname.ClearValue(Border.ToolTipProperty);
                        con.Close();
                        dr.Close();
                        gen_name = true;
                        hasrow = false;
                    }
                }
            }
        }

        void Button_Save_Enable()
        {
            if (cbTitle.SelectedIndex == -1 || FnameT.Text == "" || LnameT.Text == "")
            {
                cbRegion.SelectedIndex = -1;
                bdrRegion.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
            else
            {
                if (hasrow != true)
                {
                    bdrRegion.IsEnabled = true;
                    btnSave.IsEnabled = true;
                }
                else
                {
                    bdrRegion.IsEnabled = false;
                    btnSave.IsEnabled = false;
                }
            }
        }

        void Update_Change_Check()
        {
            if (ins_idm != string.Empty)
            {
                if (cbTitle.SelectedValue.ToString() == title_up && FnameT.Text == fname_up && MnameT.Text == mname_up && LnameT.Text == lname_up && cbRegion.SelectedValue.ToString() == reg_up && cbProvince.SelectedValue.ToString() == prov_up && cbCitmun.SelectedValue.ToString() == cit_up && cbSubmun.SelectedValue.ToString() == scit_up && cbBrgy.SelectedValue.ToString() == brgy_up && StreetT.Text == st_up && ContactT.Text == con_up)
                {
                    btnSave.IsEnabled = false;
                }
                else
                {
                    btnSave.IsEnabled = true;
                }
            }
        }
        // First Name Got Focus
        private void Fname_GotFocus(object sender, RoutedEventArgs e)
        {
            FnameW.Visibility = Visibility.Collapsed;
            tbxFname.Visibility = Visibility.Visible;
            tblFname.Visibility = Visibility.Visible;
            tbxFname.Focus();
            if (hasrow != true)
            {
                bdrFname.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            }
            if(tbxFname.Text.Length != 0)
            {
                tbxFname.CaretIndex = tbxFname.Text.Length;
            }
        }

        private void TbxFname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxFname.Text == "")
            {
                FnameW.Visibility = Visibility.Visible;
                tblFname.Visibility = Visibility.Collapsed;
                tbxFname.Visibility = Visibility.Collapsed;
            }
            else if (tbxFname.Text != "")
            {
                tbxFname.Visibility = Visibility.Collapsed;
                tblFname.Visibility = Visibility.Collapsed;
            }
            if (gen_name == true)
            {
                bdrFname.BorderBrush = Brushes.Black;
            }
            else
            {
                if (hasrow != true)
                {
                    bdrFname.BorderBrush = Brushes.Black;
                }
            }
        }

        private void TbxFname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxFname.Text != "")
            {
                FnameT.Text = tbxFname.Text;
                FnameW.Visibility = Visibility.Collapsed;
            }
            else
            {
                FnameT.Text = "";
                FnameW.Visibility = Visibility.Visible;
            }
            Name_Check();
        }
        
        //Middle Name Focus
        private void MnameW_GotFocus(object sender, RoutedEventArgs e)
        {
            MnameW.Visibility = Visibility.Collapsed;
            tbxMname.Visibility = Visibility.Visible;
            tblMname.Visibility = Visibility.Visible;
            tbxMname.Focus();
            if (hasrow != true)
            {
                bdrMname.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            }
            if (tbxMname.Text.Length != 0)
            {
                tbxMname.CaretIndex = tbxMname.Text.Length;
            }
        }

        private void TbxMname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxMname.Text == "")
            {
                MnameW.Visibility = Visibility.Visible;
                tblMname.Visibility = Visibility.Collapsed;
                tbxMname.Visibility = Visibility.Collapsed;
            }
            else if (tbxMname.Text != "")
            {
                tbxMname.Visibility = Visibility.Collapsed;
                tblMname.Visibility = Visibility.Collapsed;
            }
            if (gen_name == true)
            {
                bdrMname.BorderBrush = Brushes.Black;
            }
            else
            {
                if (hasrow != true)
                {
                    bdrMname.BorderBrush = Brushes.Black;
                }
            }
        }

        private void TbxMname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxMname.Text != "")
            {
                MnameT.Text = tbxMname.Text;
                MnameW.Visibility = Visibility.Collapsed;
            }
            else
            {
                MnameT.Text = "";
                MnameW.Visibility = Visibility.Visible;
            }
            Name_Check();
            Button_Save_Enable();
            Update_Change_Check();
        }

        //Last Name Focus
        private void LnameW_GotFocus(object sender, RoutedEventArgs e)
        {
            LnameW.Visibility = Visibility.Collapsed;
            tbxLname.Visibility = Visibility.Visible;
            tblLname.Visibility = Visibility.Visible;
            tbxLname.Focus();
            if (hasrow != true)
            {
                bdrLname.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            }
            if (tbxLname.Text.Length != 0)
            {
                tbxLname.CaretIndex = tbxLname.Text.Length;
            }
        }
        
        private void TbxLname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxLname.Text == "")
            {
                LnameW.Visibility = Visibility.Visible;
                tbxLname.Visibility = Visibility.Collapsed;
                tblLname.Visibility = Visibility.Collapsed;
            }
            else if (tbxLname.Text != "")
            {
                tbxLname.Visibility = Visibility.Collapsed;
                tblLname.Visibility = Visibility.Collapsed;
            }
            if (gen_name == true)
            {
                bdrLname.BorderBrush = Brushes.Black;
            }
            else
            {
                if (hasrow != true)
                {
                    bdrLname.BorderBrush = Brushes.Black;
                }
            }
        }

        private void TbxLname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxLname.Text != "")
            {
                LnameT.Text = tbxLname.Text;
                LnameW.Visibility = Visibility.Collapsed;
            }
            else
            {
                LnameT.Text = "";
                tbxLname.Visibility = Visibility.Visible;
            }
            Name_Check();
            Button_Save_Enable();
            Update_Change_Check();
        }

        // Title 
        private void CbTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrTitle.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }
        
        private void CbTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (gen_name == true)
            {
                bdrTitle.BorderBrush = Brushes.Black;
            }
            else
            {
                if (hasrow != true)
                {
                    bdrTitle.BorderBrush = Brushes.Black;
                }
            }
        }
        
        private void CbTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTitle.SelectedIndex != -1)
            {
                tblTitle.Visibility = Visibility.Collapsed;
            }
            else
            {
                tblTitle.Visibility = Visibility.Visible;
            }
            Name_Check();
            Button_Save_Enable();
            Update_Change_Check();
        }

        // Region
        private void CbRegion_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrRegion.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            if (ppReg.Visibility == Visibility.Visible)
            {
                ppReg.Visibility = Visibility.Collapsed;
            }

        }

        private void CbRegion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_clicked == true)
            {
                if (bool_clicked == true)
                {
                    if (cbRegion.SelectedIndex != -1)
                    {
                        bdrRegion.BorderBrush = Brushes.Black;
                        ppReg.Visibility = Visibility.Collapsed;
                        bool_prov = true;
                    }
                    else
                    {
                        bdrRegion.BorderBrush = Brushes.Red;
                        ppReg.Visibility = Visibility.Visible;
                        bool_prov = false;
                    }
                }
            }
            else
            {
                bdrRegion.BorderBrush = Brushes.Black;
            }
        }

        private void CbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRegion.SelectedIndex == -1)
            {
                cbProvince.SelectedIndex = -1;
                bdrProvince.IsEnabled = false;

                tblRegion.Visibility = Visibility.Visible;
                bool_reg = false;
            }
            else
            {
                bool_reg = true;
                using (MySqlConnection con = new MySqlConnection(Connection))
                {
                    cmd = new MySqlCommand("select * from tblreg where reg_name='" + cbRegion.SelectedValue.ToString() + "'", con);
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            reg_id = dr[0].ToString();
                            Load_Province();
                        }
                    }
                }
                bdrProvince.IsEnabled = true;
                tblRegion.Visibility = Visibility.Collapsed;
            }
            Update_Change_Check();
        }

        private void BdrRegion_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bdrRegion.IsEnabled == true)
            {
                if (bool_clicked == true)
                {
                    if (cbRegion.SelectedIndex != -1)
                    {
                        bdrRegion.BorderBrush = Brushes.Black;
                        ppReg.Visibility = Visibility.Collapsed;
                        bool_prov = true;
                    }
                    else
                    {
                        bdrRegion.BorderBrush = Brushes.Red;
                        ppReg.Visibility = Visibility.Visible;
                        bool_prov = false;
                    }
                }
            }
            else
            {
                if (ppReg != null && ppReg.Visibility == Visibility.Visible)
                {
                    ppReg.Visibility = Visibility.Collapsed;
                }
                bdrRegion.BorderBrush = Brushes.Black;
            }
        }

        // Province
        private void CbProvince_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrProvince.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbProvince_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_clicked == true)
            {
                if (bool_clicked == true)
                {
                    if (cbProvince.SelectedIndex != -1)
                    {
                        bdrProvince.BorderBrush = Brushes.Black;
                        ppProv.Visibility = Visibility.Collapsed;
                        bool_prov = true;
                    }
                    else
                    {
                        bdrProvince.BorderBrush = Brushes.Red;
                        ppProv.Visibility = Visibility.Visible;
                        bool_prov = false;
                    }
                }
            }
            else
            {
                bdrProvince.BorderBrush = Brushes.Black;
            }
        }

        private void CbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProvince.SelectedIndex == -1)
            {
                cbCitmun.SelectedIndex = -1;
                bdrCitmun.IsEnabled = false;
                tblProvince.Visibility = Visibility.Visible;
                if (bool_clicked == true)
                {
                    bdrProvince.BorderBrush = Brushes.Red;
                }
                bool_prov = false;
            }
            else
            {
                bool_prov = true;
                using (MySqlConnection con = new MySqlConnection(Connection))
                {
                    cmd = new MySqlCommand("select * from tblprov where prov_name='" + cbProvince.SelectedValue.ToString() + "'", con);
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            prov_id = dr[0].ToString();
                            Load_CitMun();
                        }
                    }
                }
                bdrCitmun.IsEnabled = true;
                tblProvince.Visibility = Visibility.Collapsed;
            }
            Update_Change_Check();
        }

        private void BdrProvince_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bdrProvince.IsEnabled == true)
            {
                if (bool_clicked == true)
                {
                    if (cbProvince.SelectedIndex != -1)
                    {
                        bdrProvince.BorderBrush = Brushes.Black;
                        ppProv.Visibility = Visibility.Collapsed;
                        bool_prov = true;
                    }
                    else
                    {
                        bdrProvince.BorderBrush = Brushes.Red;
                        ppProv.Visibility = Visibility.Visible;
                        bool_prov = false;
                    }
                }
            }
            else
            {
                if (ppProv != null && ppProv.Visibility == Visibility.Visible)
                {
                    ppProv.Visibility = Visibility.Collapsed;
                }
                bdrProvince.BorderBrush = Brushes.Black;
            }
        }

        // Municipality or City 
        private void CbCitmun_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrCitmun.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            ppCit.Visibility = Visibility.Collapsed;
        }

        private void CbCitmun_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_clicked == true)
            {
                if (cbCitmun.SelectedIndex != -1)
                {
                    bdrCitmun.BorderBrush = Brushes.Black;
                    ppCit.Visibility = Visibility.Collapsed;
                    bool_cit = true;
                }
                else
                {
                    bdrCitmun.BorderBrush = Brushes.Red;
                    ppCit.Visibility = Visibility.Visible;
                    bool_cit = false;
                }
            }
            else
            {
                bdrCitmun.BorderBrush = Brushes.Black;
            }
        }

        private void CbCitmun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCitmun.SelectedIndex == -1)
            {
                if (bdrSubmun.IsEnabled == true)
                {
                    cbSubmun.SelectedIndex = -1;
                    bdrSubmun.IsEnabled = false;
                }
                if (bdrBrgy.IsEnabled == true)
                {
                    cbBrgy.SelectedIndex = -1;
                    bdrBrgy.IsEnabled = false;
                }
                tblCitmun.Visibility = Visibility.Visible;
                bool_cit = false;
            }
            else
            {
                bool_cit = true;
                using (MySqlConnection con = new MySqlConnection(Connection))
                {
                    cmd = new MySqlCommand("select * from tblmuncit where muncit_name='" + cbCitmun.SelectedValue.ToString() + "'", con);
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[3].ToString() == "1")
                            {
                                muncit_id = dr[0].ToString();
                                Load_Subcm();
                                sub_mun = true;
                                bdrSubmun.IsEnabled = true;
                            }
                            else
                            {
                                muncit_id = dr[0].ToString();
                                Load_Brgy();
                                sub_mun = false;
                                cbSubmun.SelectedIndex = -1;
                                bdrSubmun.IsEnabled = false;
                                bdrBrgy.IsEnabled = true;
                            }
                        }
                    }
                }
                bool_cit = true;
                tblCitmun.Visibility = Visibility.Collapsed;
            }
            Update_Change_Check();
        }

        private void BdrCitmun_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bdrCitmun.IsEnabled == true)
            {
                if (bool_clicked == true)
                {
                    if (cbCitmun.SelectedIndex != -1)
                    {
                        bdrCitmun.BorderBrush = Brushes.Black;
                        ppCit.Visibility = Visibility.Collapsed;
                        bool_cit = true;
                    }
                    else
                    {
                        bdrCitmun.BorderBrush = Brushes.Red;
                        ppCit.Visibility = Visibility.Visible;
                        bool_cit = false;
                    }
                }
            }
            else
            {
                if (ppCit != null && ppCit.Visibility == Visibility.Visible)
                {
                    ppCit.Visibility = Visibility.Collapsed;
                }
                bdrCitmun.BorderBrush = Brushes.Black;
            }
        }

        // Sub Municipality or City
        private void CbSubmun_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrSubmun.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            ppSubCit.Visibility = Visibility.Collapsed;
        }

        private void CbSubmun_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_clicked == true)
            {
                if (cbSubmun.SelectedIndex != -1)
                {
                    bdrSubmun.BorderBrush = Brushes.Black;
                    ppSubCit.Visibility = Visibility.Collapsed;
                    bool_scit = true;
                }
                else
                {
                    bdrSubmun.BorderBrush = Brushes.Red;
                    ppSubCit.Visibility = Visibility.Visible;
                    bool_scit = false;
                }
            }
            else
            {
                bdrSubmun.BorderBrush = Brushes.Black;
            }
        }

        private void CbSubmun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSubmun.SelectedIndex == -1)
            {
                if (bdrBrgy.IsEnabled == true)
                {
                    cbBrgy.SelectedIndex = -1;
                    bdrBrgy.IsEnabled = false;
                }

                tblSubmun.Visibility = Visibility.Visible;
                bool_scit = false;
            }
            else
            {
                bool_scit = true;

                using (MySqlConnection con = new MySqlConnection(Connection))
                {
                    cmd = new MySqlCommand("select * from tblsubmun where submun_name='" + cbSubmun.SelectedValue.ToString() + "'", con);
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            submun_id = dr[0].ToString();
                            Load_Sbrgy();
                        }
                    }
                }
                bdrBrgy.IsEnabled = true;
                tblSubmun.Visibility = Visibility.Collapsed;
            }
            Update_Change_Check();
        }

        private void BdrSubmun_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bdrSubmun.IsEnabled == true)
            {
                if (bool_clicked == true)
                {
                    if (cbSubmun.SelectedIndex != -1)
                    {
                        bdrSubmun.BorderBrush = Brushes.Black;
                        ppSubCit.Visibility = Visibility.Collapsed;
                        bool_scit = true;
                    }
                    else
                    {
                        bdrSubmun.BorderBrush = Brushes.Red;
                        ppSubCit.Visibility = Visibility.Visible;
                        bool_scit = false;
                    }
                }
            }
            else
            {
                if (ppSubCit != null && ppSubCit.Visibility == Visibility.Visible)
                {
                    ppSubCit.Visibility = Visibility.Collapsed;
                }
                bool_scit = false;
                bdrSubmun.BorderBrush = Brushes.Black;
            }
           
        }

        // Barangay
        private void CbBrgy_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrBrgy.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            ppBrgy.Visibility = Visibility.Collapsed;
        }

        private void CbBrgy_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_clicked == true)
            {
                if (cbBrgy.SelectedIndex != -1)
                {
                    bdrBrgy.BorderBrush = Brushes.Black;
                    ppBrgy.Visibility = Visibility.Collapsed;
                    bool_brgy = true;
                }
                else
                {
                    bdrBrgy.BorderBrush = Brushes.Red;
                    ppBrgy.Visibility = Visibility.Visible;
                    bool_brgy = false;
                }
            }
            else
            {
                bdrBrgy.BorderBrush = Brushes.Black;
            }
        }

        private void BdrBrgy_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bdrBrgy.IsEnabled == true)
            {
                if (bool_clicked == true)
                {
                    if (cbBrgy.SelectedIndex != -1)
                    {
                        bdrBrgy.BorderBrush = Brushes.Black;
                        ppBrgy.Visibility = Visibility.Collapsed;
                        bool_brgy = true;
                    }
                    else
                    {
                        bdrBrgy.BorderBrush = Brushes.Red;
                        ppBrgy.Visibility = Visibility.Visible;
                        bool_brgy = false;
                    }
                }
            }
            else
            {
                if (ppBrgy != null && ppBrgy.Visibility == Visibility.Visible)
                {
                    ppBrgy.Visibility = Visibility.Collapsed;
                }
                bdrBrgy.BorderBrush = Brushes.Black;
            }
        }

        private void CbBrgy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbBrgy.SelectedIndex == -1)
            {
                tblBrgy.Visibility = Visibility.Visible;
                
                bool_brgy = false;
            }
            else
            {
                bool_brgy = true;
                tblBrgy.Visibility = Visibility.Collapsed;
            }
            Update_Change_Check();
        }


        // Contact 
        private void ContactW_GotFocus(object sender, RoutedEventArgs e)
        {
            ContactT.Visibility = Visibility.Collapsed;
            grdCon.Margin = new Thickness(0);
            conpre.Visibility = Visibility.Visible;
            ContactW.Visibility = Visibility.Collapsed;
            tbxContact.Visibility = Visibility.Visible;
            tbxContact.Visibility = Visibility.Visible;
            tblContact.Visibility = Visibility.Visible;
            tbxContact.Focus();
            bdrContact.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            grdCon.Visibility = Visibility.Visible;
            bdrConline.Visibility = Visibility.Visible;
            ContactWW.Visibility = Visibility.Visible;
            if (tbxContact.Text.Length != 0)
            {
                tbxContact.CaretIndex = tbxContact.Text.Length;
                ContactWW.Visibility = Visibility.Collapsed;
            }
        }

        private void TbxContact_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxContact.Text == "")
            {
                ContactT.Visibility = Visibility.Collapsed;
                grdCon.Margin = new Thickness(10);
                conpre.Visibility = Visibility.Collapsed;
                ContactWW.Visibility = Visibility.Collapsed;
                ContactW.Visibility = Visibility.Visible;
                tblContact.Visibility = Visibility.Collapsed;
                tbxContact.Visibility = Visibility.Collapsed;
                bdrContact.BorderBrush = Brushes.Black;
                bdrConline.Visibility = Visibility.Collapsed;
            }
            else if (tbxContact.Text != "")
            {
                ContactWW.Visibility = Visibility.Collapsed;
                grdCon.Visibility = Visibility.Visible;
                conpre.Visibility = Visibility.Collapsed;
                bdrConline.Visibility = Visibility.Visible;
                ContactT.Visibility = Visibility.Visible;
                tblContact.Visibility = Visibility.Collapsed;
                tbxContact.Visibility = Visibility.Collapsed;
                bdrContact.BorderBrush = Brushes.Black;
                grdCon.Margin = new Thickness(10);
                bdrConline.Visibility = Visibility.Collapsed;
            }
        }

        private void TbxContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxContact.Text.Length != 0)
            {
                ContactT.Text = tbxContact.Text;
                ContactWW.Visibility = Visibility.Collapsed;
            }
            else
            {
                ContactT.Text = "";
                ContactWW.Visibility = Visibility.Visible;
            }
            Update_Change_Check();
        }


        // Street
        private void StreetW_GotFocus(object sender, RoutedEventArgs e)
        {
            StreetW.Visibility = Visibility.Collapsed;
            tbxStreet.Visibility = Visibility.Visible;
            tblStreet.Visibility = Visibility.Visible;
            tbxStreet.Focus();
            bdrStreet.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            if (tbxStreet.Text.Length != 0)
            {
                tbxStreet.CaretIndex = tbxStreet.Text.Length;
            }
        }

        private void TbxStreet_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxStreet.Text == "")
            {
                StreetW.Visibility = Visibility.Visible;
                tblStreet.Visibility = Visibility.Collapsed;
                tbxStreet.Visibility = Visibility.Collapsed;
                bdrStreet.BorderBrush = Brushes.Black;
            }
            else if (tbxStreet.Text != "")
            {
                StreetW.Visibility = Visibility.Collapsed;
                tbxStreet.Visibility = Visibility.Collapsed;
                tblStreet.Visibility = Visibility.Collapsed;
                bdrStreet.BorderBrush = Brushes.Black;
            }

        }

        private void TbxStreet_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxStreet.Text != "")
            {
                StreetT.Text = tbxStreet.Text;
                StreetW.Visibility = Visibility.Collapsed;
            }
            else
            {
                StreetT.Text = "";
                StreetW.Visibility = Visibility.Visible;
            }

            Update_Change_Check();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool_clicked = true;
            if (cbRegion.SelectedIndex == -1)
            {
                bdrRegion.BorderBrush = new SolidColorBrush(Colors.Red);
                ppReg.Visibility = Visibility.Visible;
                bool_reg = false;
            }
            if (bdrProvince.IsEnabled == false)
            {
                bool_prov = false;
            }
            else
            {
                if(cbProvince.SelectedIndex == -1)
                {
                    bdrProvince.BorderBrush = new SolidColorBrush(Colors.Red);
                    ppProv.Visibility = Visibility.Visible;
                    bool_prov = false;
                }
            }
            if (bdrCitmun.IsEnabled == false)
            {
                bool_cit = false;
            }
            else
            {
                if (cbCitmun.SelectedIndex == -1)
                {
                    bdrCitmun.BorderBrush = new SolidColorBrush(Colors.Red);
                    ppCit.Visibility = Visibility.Visible;
                    bool_prov = false;
                }
            }
            if (sub_mun == true)
            {
                if (cbSubmun.SelectedIndex == -1)
                {
                    bdrSubmun.BorderBrush = new SolidColorBrush(Colors.Red);
                    ppSubCit.Visibility = Visibility.Visible;
                    bool_scit = false;
                }
            }
            if (cbBrgy.IsEnabled == false)
            {
                bool_brgy = false;
            }
            else
            {
                if (cbBrgy.SelectedIndex == -1)
                {
                    bdrBrgy.BorderBrush = new SolidColorBrush(Colors.Red);
                    ppBrgy.Visibility = Visibility.Visible;
                    bool_brgy = false;
                }
            }

            if (ins_idm != string.Empty)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                MySqlCommand cmd = new MySqlCommand("UPDATE `tblins` SET `title`=?title,`FName`=?fname,`MName`=?mname,`LName`=?lname,`Ins_reg`=?reg,`Ins_prov`=?prov,`Ins_cit`=?cit,`Ins_scit`=?scit,`Ins_brgy`=?brgy,`Ins_st`=?st,`Ins_con`=?con WHERE Ins_id='" + ins_idm + "'", con);
                cmd.Parameters.AddWithValue("@title", cbTitle.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@fname", FnameT.Text);
                if (MnameT.Text != "")
                {
                    cmd.Parameters.AddWithValue("@mname", MnameT.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@mname", "");
                }
                cmd.Parameters.AddWithValue("@lname", LnameT.Text);
                cmd.Parameters.AddWithValue("@reg", cbRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@prov", cbProvince.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@cit", cbCitmun.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@scit", cbSubmun.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@brgy", cbBrgy.SelectedValue.ToString());

                if (StreetT.Text != "")
                {
                    cmd.Parameters.AddWithValue("@st", StreetT.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@st", "");
                }
                if (ContactT.Text != "")
                {
                    cmd.Parameters.AddWithValue("@con", "+63" + ContactT.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@con", "");
                }
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Account has been updated successfully.");
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Instructors))
                    {
                        (window as Instructors).grdcontent.Children.Remove(this);
                        (window as Instructors).Fill_DataGrid_Instructor();
                        (window as Instructors).bdrContent.IsEnabled = true;
                    }
                }
            }
            else
            {
                if (gen_name == true &&  bool_reg == true && bool_prov == true && bool_cit == true && bool_scit == true && bool_brgy == true)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        cmd = new MySqlCommand("INSERT INTO tblins(title, FName, MName, LName, Ins_reg, Ins_prov, Ins_cit, Ins_scit, Ins_brgy, Ins_st, Ins_con) "
                                                 + "VALUES(@title, @fname, @mname, @lname, @reg, @prov, @cit, @scit, @brgy, @st, @cont)", con);

                        cmd.Parameters.AddWithValue("@title", cbTitle.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@fname", FnameT.Text);
                        if (MnameT.Text != "")
                        {
                            cmd.Parameters.AddWithValue("@mname", MnameT.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@mname", "");
                        }
                        cmd.Parameters.AddWithValue("@lname", LnameT.Text);
                        cmd.Parameters.AddWithValue("@reg", cbRegion.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@prov", cbProvince.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@cit", cbCitmun.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@scit", cbSubmun.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@brgy", cbBrgy.SelectedValue.ToString());

                        if (StreetT.Text != "")
                        {
                            cmd.Parameters.AddWithValue("@st", StreetT.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@st", "");
                        }
                        if (ContactT.Text != "")
                        {
                            cmd.Parameters.AddWithValue("@cont", "+63" + ContactT.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@cont", "");
                        }
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();

                        MessageBox.Show("The data has been saved successfully.");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Instructors))
                            {
                                (window as Instructors).grdcontent.Children.Remove(this);
                                (window as Instructors).Fill_DataGrid_Instructor();
                                (window as Instructors).bdrContent.IsEnabled = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }

                }
                else if (gen_name == true && bool_reg == true && bool_prov == true && bool_cit == true && bool_scit == false && bool_brgy == true)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        cmd = new MySqlCommand("INSERT INTO tblins(title, FName, MName, LName, Ins_reg, Ins_prov, Ins_cit, Ins_brgy, Ins_st, Ins_con) "
                                                 + "VALUES(@title, @fname, @mname, @lname, @reg, @prov, @cit, @brgy, @st, @cont)", con);

                        cmd.Parameters.AddWithValue("@title", cbTitle.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@fname", FnameT.Text);
                        if (MnameT.Text != "")
                        {
                            cmd.Parameters.AddWithValue("@mname", MnameT.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@mname", "");
                        }
                        cmd.Parameters.AddWithValue("@lname", LnameT.Text);
                        cmd.Parameters.AddWithValue("@reg", cbRegion.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@prov", cbProvince.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@cit", cbCitmun.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@scit", "");
                        cmd.Parameters.AddWithValue("@brgy", cbBrgy.SelectedValue.ToString());

                        if (StreetT.Text != "")
                        {
                            cmd.Parameters.AddWithValue("@st", StreetT.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@st", "");
                        }
                        if (ContactT.Text != "")
                        {
                            cmd.Parameters.AddWithValue("@cont", "+63" + ContactT.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@cont", "");
                        }
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();

                        MessageBox.Show("The data has been saved successfully.");

                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Instructors))
                            {
                                (window as Instructors).grdcontent.Children.Remove(this);
                                (window as Instructors).Fill_DataGrid_Instructor();
                                (window as Instructors).bdrContent.IsEnabled = true;
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
            if (ins_idm != string.Empty)
            {
                if (cbTitle.SelectedValue.ToString() == title_up && FnameT.Text == fname_up && MnameT.Text == mname_up && LnameT.Text == lname_up && cbRegion.SelectedValue.ToString() == reg_up && cbProvince.SelectedValue.ToString() == prov_up && cbCitmun.SelectedValue.ToString() == cit_up && cbSubmun.SelectedValue.ToString() == scit_up && cbBrgy.SelectedValue.ToString() == brgy_up && StreetT.Text == st_up && ContactT.Text == con_up)
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Instructors))
                        {
                            (window as Instructors).grdcontent.Children.Remove(this);
                            (window as Instructors).bdrContent.IsEnabled = true;

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
                            if (window.GetType() == typeof(Instructors))
                            {
                                (window as Instructors).grdcontent.Children.Remove(this);
                                (window as Instructors).bdrContent.IsEnabled = true;

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
                if (cbTitle.SelectedIndex != -1 || FnameT.Text != "" || MnameT.Text != "" || LnameT.Text != "" || cbRegion.SelectedIndex != -1 || cbProvince.SelectedIndex != -1 || cbSubmun.SelectedIndex != -1 || cbCitmun.SelectedIndex != -1 || cbBrgy.SelectedIndex != -1 || StreetT.Text != "" || ContactT.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("Any changes will not be saved.", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Instructors))
                            {
                                (window as Instructors).grdcontent.Children.Remove(this);
                                (window as Instructors).bdrContent.IsEnabled = true;

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
                        if (window.GetType() == typeof(Instructors))
                        {
                            (window as Instructors).grdcontent.Children.Remove(this);
                            (window as Instructors).bdrContent.IsEnabled = true;

                        }
                    }
                }
            }
        }

        // Restrictions
        private void TbxFname_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z\\s]"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
            if (tbxFname.Text.Length == 15)
            {
                SystemSounds.Asterisk.Play();
            }
        }

        private void TbxMname_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

        private void TbxLname_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

        private void TbxStreet_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z0-9.,]"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

        private void TbxContact_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
                SystemSounds.Asterisk.Play();
            }
        }

    }
}
