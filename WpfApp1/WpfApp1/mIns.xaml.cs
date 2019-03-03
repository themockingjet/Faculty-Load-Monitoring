using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for mIns.xaml
    /// </summary>
    public partial class mIns : UserControl
    {
        string ins_idm = string.Empty;

        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=;";
        MySqlCommand cmd;
        MySqlDataReader reader;

        string reg_id = "";
        string prov_id = "";
        string muncit_id = "";
        string submun_id = "";
        bool sub_mun = false;

        ToolTip tooltip;
        bool bool_clicked = false;
        bool bool_title = true;
        bool bool_fname = true;
        bool bool_lname = true;
        bool bool_reg = true;
        bool bool_prov = true;
        bool bool_cit = true;
        bool bool_scit = true;
        bool bool_brgy = true;

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
            }

            Load_Region();

            MySqlConnection con = new MySqlConnection(Connection);

            
            
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
            cbTitle.Items.Add("Mr.");
            cbTitle.Items.Add("Ms.");
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

        private void TbxFname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxFname.Text != "")
            {
                FnameT.Text = tbxFname.Text;
                FnameW.Visibility = Visibility.Collapsed;
            }
            else
            {
                FnameW.Visibility = Visibility.Visible;
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
                tbxLname.Visibility = Visibility.Visible;
            }
        }

        private void TbxMname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxMname.Text != "")
            {
                MnameT.Text = tbxMname.Text;
                MnameT.Visibility = Visibility.Collapsed;
            }
            else
            {
                MnameW.Visibility = Visibility.Visible;
            }
        }

        // First Name Got Focus
        private void Fname_GotFocus(object sender, RoutedEventArgs e)
        {
            FnameW.Visibility = Visibility.Collapsed;
            tbxFname.Visibility = Visibility.Visible;
            tblFname.Visibility = Visibility.Visible;
            tbxFname.Focus();
            bdrFname.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            if(tbxFname.Text.Length != 0)
            {
                tbxFname.CaretIndex = tbxFname.Text.Length;
            }
        }

        private void TbxFname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_fname == true)
            {
                if (bool_clicked == true)
                {
                    if (tbxFname.Text == "")
                    {
                        FnameT.Text = "";
                        FnameW.Visibility = Visibility.Visible;
                        tblFname.Visibility = Visibility.Collapsed;
                        tbxFname.Visibility = Visibility.Collapsed;
                        bdrFname.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "First Name is required" };
                        bdrFname.ToolTip = tooltip;
                        bool_fname = false;
                    }
                    else if (tbxFname.Text != "")
                    {
                        FnameT.Text = tbxFname.Text;
                        tbxFname.Visibility = Visibility.Collapsed;
                        tblFname.Visibility = Visibility.Collapsed;
                        bdrFname.BorderBrush = Brushes.Black;
                        bdrFname.ClearValue(Border.ToolTipProperty);
                        bool_fname = true;
                    }
                }
                else
                {
                    if (tbxFname.Text == "")
                    {
                        FnameT.Text = "";
                        FnameW.Visibility = Visibility.Visible;
                        tblFname.Visibility = Visibility.Collapsed;
                        tbxFname.Visibility = Visibility.Collapsed;
                        bdrFname.BorderBrush = Brushes.Black;
                    }
                    else if (tbxFname.Text != "")
                    {
                        FnameT.Text = tbxFname.Text;
                        tbxFname.Visibility = Visibility.Collapsed;
                        tblFname.Visibility = Visibility.Collapsed;
                        bdrFname.BorderBrush = Brushes.Black;
                    }
                }
            }
            else
            {
                if(bool_clicked == true)
                {
                    if (tbxFname.Text == "")
                    {
                        FnameT.Text = "";
                        FnameW.Visibility = Visibility.Visible;
                        tblFname.Visibility = Visibility.Collapsed;
                        tbxFname.Visibility = Visibility.Collapsed;
                        bdrFname.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "First Name is required" };
                        bdrFname.ToolTip = tooltip;
                        bool_fname = false;
                    }
                    else if (tbxFname.Text != "")
                    {
                        FnameT.Text = tbxFname.Text;
                        tbxFname.Visibility = Visibility.Collapsed;
                        tblFname.Visibility = Visibility.Collapsed;
                        bdrFname.BorderBrush = Brushes.Black;
                        bdrFname.ClearValue(Border.ToolTipProperty);
                        bool_fname = true;
                    }
                }
            }
        }

        //Middle Name Focus
        private void MnameW_GotFocus(object sender, RoutedEventArgs e)
        {
            MnameW.Visibility = Visibility.Collapsed;
            tbxMname.Visibility = Visibility.Visible;
            tblMname.Visibility = Visibility.Visible;
            tbxMname.Focus();
            bdrMname.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            if (tbxMname.Text.Length != 0)
            {
                tbxMname.CaretIndex = tbxMname.Text.Length;
            }
        }

        private void TbxMname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxMname.Text == "")
            {
                MnameT.Text = "";
                MnameW.Visibility = Visibility.Visible;
                tblMname.Visibility = Visibility.Collapsed;
                tbxMname.Visibility = Visibility.Collapsed;
                bdrMname.BorderBrush = Brushes.Black;
            }
            else if (tbxMname.Text != "")
            {
                MnameT.Text = tbxMname.Text;
                tblMname.Visibility = Visibility.Collapsed;
                tbxMname.Visibility = Visibility.Collapsed;
                bdrMname.BorderBrush = Brushes.Black;
            }
        }

        //Last Name Focus
        private void LnameW_GotFocus(object sender, RoutedEventArgs e)
        {
            LnameW.Visibility = Visibility.Collapsed;
            tbxLname.Visibility = Visibility.Visible;
            tblLname.Visibility = Visibility.Visible;
            tbxLname.Focus();
            bdrLname.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
            if (tbxLname.Text.Length != 0)
            {
                tbxLname.CaretIndex = tbxLname.Text.Length;
            }
        }
        
        private void TbxLname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_lname == true)
            {
                if (bool_clicked == true)
                {
                    if (tbxLname.Text == "")
                    {
                        LnameT.Text = "";
                        LnameW.Visibility = Visibility.Visible;
                        tblLname.Visibility = Visibility.Collapsed;
                        tbxLname.Visibility = Visibility.Collapsed;
                        bdrLname.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Last Name is required" };
                        bdrLname.ToolTip = tooltip;
                        bool_lname = false;
                    }
                    else if (tbxLname.Text != "")
                    {
                        LnameT.Text = tbxLname.Text;
                        tblLname.Visibility = Visibility.Collapsed;
                        tbxLname.Visibility = Visibility.Collapsed;
                        bdrLname.BorderBrush = Brushes.Black;
                        bdrLname.ClearValue(Border.ToolTipProperty);
                        bool_lname = true;
                    }
                }
                else
                {
                    if (tbxLname.Text == "")
                    {
                        LnameT.Text = "";
                        LnameW.Visibility = Visibility.Visible;
                        tblLname.Visibility = Visibility.Collapsed;
                        tbxLname.Visibility = Visibility.Collapsed;
                        bdrLname.BorderBrush = Brushes.Black;
                    }
                    else if (tbxLname.Text != "")
                    {
                        LnameT.Text = tbxLname.Text;
                        tblLname.Visibility = Visibility.Collapsed;
                        tbxLname.Visibility = Visibility.Collapsed;
                        bdrLname.BorderBrush = Brushes.Black;
                    }
                }
            }
            else
            {
                    if (tbxLname.Text == "")
                    {
                        LnameT.Visibility = Visibility.Collapsed;
                        LnameW.Visibility = Visibility.Visible;
                        tblLname.Visibility = Visibility.Collapsed;
                        tbxLname.Visibility = Visibility.Collapsed;
                        bdrLname.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Last Name is required" };
                        bdrLname.ToolTip = tooltip;
                        bool_lname = false;
                    }
                    else if (tbxLname.Text != "")
                    {
                        LnameT.Visibility = Visibility.Visible;
                        LnameT.Text = tbxLname.Text;
                        tblLname.Visibility = Visibility.Collapsed;
                        tbxLname.Visibility = Visibility.Collapsed;
                        bdrLname.BorderBrush = Brushes.Black;
                        bdrLname.ClearValue(Border.ToolTipProperty);
                        bool_lname = true;
                    }
            }
        }

        // Title 
        private void CbTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrTitle.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }
        
        private void CbTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if(bool_title == true)
            {
                bdrTitle.BorderBrush = Brushes.Black;
            }
            else
            {
                if(cbTitle.SelectedIndex != -1)
                {
                    bdrTitle.BorderBrush = Brushes.Black;
                    bdrTitle.ClearValue(Border.ToolTipProperty);
                    bool_title = true;
                }
                else
                {
                    bdrTitle.BorderBrush = Brushes.Red;
                    tooltip = new ToolTip { Content = "Last Name is required" };
                    bdrTitle.ToolTip = tooltip;
                    bool_title = false;
                }
            }
        }
        
        private void CbTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bool_title == true)
            {
                if (cbTitle.SelectedIndex != -1)
                {
                    tblTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tblTitle.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (cbTitle.SelectedIndex != -1)
                {
                    tblTitle.Visibility = Visibility.Collapsed;
                    bdrTitle.ClearValue(Border.ToolTipProperty);
                }
                else
                {
                    tblTitle.Visibility = Visibility.Visible;
                }
            }
        }

        // Region
        private void CbRegion_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrRegion.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbRegion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_reg == true)
            {
                bdrRegion.BorderBrush = Brushes.Black;
            }
            else
            {
                if (cbRegion.SelectedIndex != -1)
                {
                    bdrRegion.BorderBrush = Brushes.Black;
                    bdrRegion.ClearValue(Border.ToolTipProperty);
                    bool_reg = true;
                }
                else
                {
                    bdrRegion.BorderBrush = Brushes.Red;
                    tooltip = new ToolTip { Content = "Last Name is required" };
                    bdrRegion.ToolTip = tooltip;
                    bool_reg = false;
                }
            }
        }

        private void CbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRegion.SelectedIndex == -1)
            {
                bdrProvince.IsEnabled = false;
            }
            else
            {
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
        }

        // Province
        private void CbProvince_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrProvince.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbProvince_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_prov == true)
            {
                bdrProvince.BorderBrush = Brushes.Black;
            }
            else
            {
                if (cbProvince.SelectedIndex != -1)
                {
                    bdrProvince.BorderBrush = Brushes.Black;
                    bdrProvince.ClearValue(Border.ToolTipProperty);
                    bool_prov = true;
                }
                else
                {
                    bdrProvince.BorderBrush = Brushes.Red;
                    tooltip = new ToolTip { Content = "Last Name is required" };
                    bdrProvince.ToolTip = tooltip;
                    bool_prov = false;
                }
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
                    if (cbProvince.SelectedIndex != -1)
                    {
                        bdrProvince.BorderBrush = Brushes.Black;
                        bdrProvince.ClearValue(Border.ToolTipProperty);
                        bool_prov = true;
                    }
                    else
                    {
                        bdrProvince.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Province is required" };
                        bdrProvince.ToolTip = tooltip;
                        bool_prov = false;
                    }
                }
            }
            else
            {
                if (bool_prov == true)
                {
                    bdrProvince.BorderBrush = Brushes.Black;
                }
                else
                {
                    if (cbProvince.SelectedIndex != -1)
                    {
                        bdrProvince.BorderBrush = Brushes.Black;
                        bdrProvince.ClearValue(Border.ToolTipProperty);
                        bool_prov = true;
                    }
                    else
                    {
                        bdrProvince.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Last Name is required" };
                        bdrProvince.ToolTip = tooltip;
                        bool_prov = false;
                    }
                }
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
        }

        private void BdrProvince_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bdrProvince.IsEnabled == true)
            {
                if (bool_prov == true)
                {
                    bdrProvince.BorderBrush = Brushes.Black;
                    bdrProvince.ClearValue(Border.ToolTipProperty);
                    bool_prov = true;
                }
                else
                {
                    bdrProvince.BorderBrush = new SolidColorBrush(Colors.Red);
                    tooltip = new ToolTip { Content = "Province is required!" };
                    bdrProvince.ToolTip = tooltip;
                    bool_prov = false;
                }
            }
        }

        // Municipality or City 
        private void CbCitmun_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrCitmun.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbCitmun_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_cit == true)
            {
                bdrCitmun.BorderBrush = Brushes.Black;
            }
            else
            {
                if (cbProvince.SelectedIndex != -1)
                {
                    bdrCitmun.BorderBrush = Brushes.Black;
                    bdrCitmun.ClearValue(Border.ToolTipProperty);
                    bool_cit = true;
                }
                else
                {
                    bdrCitmun.BorderBrush = Brushes.Red;
                    tooltip = new ToolTip { Content = "City/Municipality is required!" };
                    bdrCitmun.ToolTip = tooltip;
                    bool_cit = false;
                }
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
                if (bool_clicked == true)
                {
                    if (cbCitmun.SelectedIndex != -1)
                    {
                        bdrCitmun.BorderBrush = Brushes.Black;
                        bdrCitmun.ClearValue(Border.ToolTipProperty);
                        bool_prov = true;
                    }
                    else
                    {
                        bdrCitmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Province is required" };
                        bdrCitmun.ToolTip = tooltip;
                        bool_prov = false;
                    }
                }
                tblCitmun.Visibility = Visibility.Visible;
            }
            else
            {
                if (bool_clicked == true)
                {
                    if (cbCitmun.SelectedIndex != -1)
                    {
                        bdrCitmun.BorderBrush = Brushes.Black;
                        bdrCitmun.ClearValue(Border.ToolTipProperty);
                        bool_prov = true;
                    }
                    else
                    {
                        bdrCitmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Province is required" };
                        bdrCitmun.ToolTip = tooltip;
                        bool_prov = false;
                    }
                }
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
                tblCitmun.Visibility = Visibility.Collapsed;
            }
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
                        bdrCitmun.ClearValue(Border.ToolTipProperty);
                        bool_cit = true;
                    }
                    else
                    {
                        bdrCitmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "City/Municipality is required" };
                        bdrCitmun.ToolTip = tooltip;
                        bool_cit = false;
                    }
                }
            }
            else
            {
                bdrCitmun.BorderBrush = Brushes.Black;
            }
        }

        // Sub Municipality or City
        private void CbSubmun_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrSubmun.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbSubmun_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_scit == true)
            {
                bdrSubmun.BorderBrush = Brushes.Black;
            }
            else
            {
                if (bool_clicked == true)
                {
                    if (cbSubmun.SelectedIndex != -1)
                    {
                        bdrSubmun.BorderBrush = Brushes.Black;
                        bdrSubmun.ClearValue(Border.ToolTipProperty);
                        bool_scit = true;
                    }
                    else
                    {
                        bdrSubmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Sub City/Municipality is required" };
                        bdrSubmun.ToolTip = tooltip;
                        bool_scit = false;
                    }
                }
                else
                {
                    bdrSubmun.BorderBrush = Brushes.Black;
                }
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
                
                if (bool_clicked == true)
                {
                    if (cbSubmun.SelectedIndex != -1)
                    {
                        bdrSubmun.BorderBrush = Brushes.Black;
                        bdrSubmun.ClearValue(Border.ToolTipProperty);
                        bool_scit = true;
                    }
                    else
                    {
                        bdrSubmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Province is required" };
                        bdrSubmun.ToolTip = tooltip;
                        bool_scit = false;
                    }
                }

                tblSubmun.Visibility = Visibility.Visible;
            }
            else
            {
                if (bool_clicked == true)
                {
                    if (cbSubmun.SelectedIndex != -1)
                    {
                        bdrSubmun.BorderBrush = Brushes.Black;
                        bdrSubmun.ClearValue(Border.ToolTipProperty);
                        bool_scit = true;
                    }
                    else
                    {
                        bdrSubmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Province is required" };
                        bdrSubmun.ToolTip = tooltip;
                        bool_scit = false;
                    }
                }
                else
                {
                    bool_scit = true;
                }


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
                        bdrSubmun.ClearValue(Border.ToolTipProperty);
                        bool_scit = true;
                    }
                    else
                    {
                        bdrSubmun.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Sub City/Municipality is required" };
                        bdrSubmun.ToolTip = tooltip;
                        bool_scit = false;
                    }
                }
            }
            else
            {
                bool_scit = false;
                bdrSubmun.BorderBrush = Brushes.Black;
            }
           
        }

        // Barangay
        private void CbBrgy_GotFocus(object sender, RoutedEventArgs e)
        {
            bdrBrgy.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void CbBrgy_LostFocus(object sender, RoutedEventArgs e)
        {
            if (bool_brgy == true)
            {
                bdrBrgy.BorderBrush = Brushes.Black;
            }
            else
            {
                if (cbTitle.SelectedIndex != -1)
                {
                    bdrBrgy.BorderBrush = Brushes.Black;
                    bdrBrgy.ClearValue(Border.ToolTipProperty);
                    bool_brgy = true;
                }
                else
                {
                    bdrBrgy.BorderBrush = Brushes.Red;
                    tooltip = new ToolTip { Content = "Barangay is required!" };
                    bdrBrgy.ToolTip = tooltip;
                    bool_brgy = false;
                }
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
                        bdrBrgy.ClearValue(Border.ToolTipProperty);
                        bool_brgy = true;
                    }
                    else
                    {
                        bdrBrgy.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Barangay is required" };
                        bdrBrgy.ToolTip = tooltip;
                        bool_brgy = false;
                    }
                }
            }
            else
            {
                bdrBrgy.BorderBrush = Brushes.Black;
            }
        }

        private void CbBrgy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbBrgy.SelectedIndex == -1)
            {
                tblBrgy.Visibility = Visibility.Visible;
                if (bool_clicked == true)
                {
                    if (cbBrgy.SelectedIndex != -1)
                    {
                        bdrBrgy.BorderBrush = Brushes.Black;
                        bdrBrgy.ClearValue(Border.ToolTipProperty);
                        bool_brgy = true;
                    }
                    else
                    {
                        bdrBrgy.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Barangay is required" };
                        bdrBrgy.ToolTip = tooltip;
                        bool_brgy = false;
                    }
                }
            }
            else
            {
                if (bool_clicked == true)
                {
                    if (cbSubmun.SelectedIndex != -1)
                    {
                        bdrBrgy.BorderBrush = Brushes.Black;
                        bdrBrgy.ClearValue(Border.ToolTipProperty);
                        bool_brgy = true;
                    }
                    else
                    {
                        bdrBrgy.BorderBrush = Brushes.Red;
                        tooltip = new ToolTip { Content = "Barangay is required" };
                        bdrBrgy.ToolTip = tooltip;
                        bool_brgy = false;
                    }
                }
                tblBrgy.Visibility = Visibility.Collapsed;
            }
        }


        //
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
                ContactT.Text = tbxContact.Text;
                tblContact.Visibility = Visibility.Collapsed;
                tbxContact.Visibility = Visibility.Collapsed;
                bdrContact.BorderBrush = Brushes.Black;
                grdCon.Margin = new Thickness(10);
                bdrConline.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main_Grid.Focus();
        }

        private void TbxContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxContact.Text.Length != 0)
            {
                ContactWW.Visibility = Visibility.Collapsed;
            }
            else
            {
                ContactWW.Visibility = Visibility.Visible;
            }
        }

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
            if (tbxFname.Text == "")
            {
                StreetT.Visibility = Visibility.Collapsed;
                StreetW.Visibility = Visibility.Visible;
                tblStreet.Visibility = Visibility.Collapsed;
                tbxStreet.Visibility = Visibility.Collapsed;
                bdrStreet.BorderBrush = Brushes.Black;
            }
            else if (tbxFname.Text != "")
            {
                StreetT.Visibility = Visibility.Visible;
                StreetT.Text = tbxStreet.Text;
                tbxStreet.Visibility = Visibility.Collapsed;
                tblStreet.Visibility = Visibility.Collapsed;
                bdrStreet.BorderBrush = Brushes.Black;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool_clicked = true;
            if (cbTitle.SelectedIndex == -1)
            {
                bdrTitle.BorderBrush = new SolidColorBrush(Colors.Red);
                tooltip = new ToolTip{ Content = "Title is required!" };
                bdrTitle.ToolTip = tooltip;
                bool_title = false;
            }
            if (FnameT.Text == "")
            {
                bdrFname.BorderBrush = new SolidColorBrush(Colors.Red);
                tooltip = new ToolTip { Content = "First Name is required!" };
                bdrFname.ToolTip = tooltip;
                bool_fname = false;
            }
            if (LnameT.Text == "")
            {
                bdrLname.BorderBrush = new SolidColorBrush(Colors.Red);
                tooltip = new ToolTip { Content = "Last Name is required!" };
                bdrLname.ToolTip = tooltip;
                bool_lname = false;
            }
            if (cbRegion.SelectedIndex == -1)
            {
                bdrRegion.BorderBrush = new SolidColorBrush(Colors.Red);
                tooltip = new ToolTip { Content = "Region is required!" };
                bdrRegion.ToolTip = tooltip;
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
                    tooltip = new ToolTip { Content = "Province is required!" };
                    bdrRegion.ToolTip = tooltip;
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
                    tooltip = new ToolTip { Content = "City/Municipality is required!" };
                    bdrCitmun.ToolTip = tooltip;
                    bool_prov = false;
                }
            }
            if (sub_mun == true)
            {
                if (cbSubmun.SelectedIndex == -1)
                {
                    bdrSubmun.BorderBrush = new SolidColorBrush(Colors.Red);
                    tooltip = new ToolTip { Content = "Sub City/Municipality is required!" };
                    bdrSubmun.ToolTip = tooltip;
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
                    tooltip = new ToolTip { Content = "City/Municipality is required!" };
                    bdrBrgy.ToolTip = tooltip;
                    bool_brgy = false;
                }
            }

            if (ins_idm != string.Empty)
            {
                MySqlConnection con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT `Ins_id`, `title`, `FName`, `MName`, `LName`, `Ins_reg`, `Ins_prov`, `Ins_cit`, `Ins_scit`, `Ins_brgy`, `Ins_st`, `Ins_con` FROM `tblins`"
                                          + "WHERE Ins_id = '" + ins_idm + "'AND title='" + cbTitle.SelectedValue.ToString() + "'AND FName='" + FnameT.Text + "'AND MName ='" + MnameT.Text + "'AND LName ='" + LnameT.Text + "'AND Ins_reg='" + cbRegion.SelectedValue.ToString()
                                          + "'AND Ins_prov ='" + cbProvince.SelectedValue.ToString() + "'AND Ins_cit ='" + cbCitmun.SelectedValue.ToString() + "'AND Ins_scit ='" + cbSubmun.SelectedValue.ToString() + "'AND Ins_brgy ='" + cbBrgy.SelectedValue.ToString() + "'AND Ins_st='" + StreetT.Text
                                          + "'AND Ins_con ='" + ContactT.Text + "'", con);
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    MessageBox.Show("Account information is already in the database.");
                    con.Close();
                    dr.Close();
                }
                else
                {
                    con.Close();
                    MySqlCommand cmd = new MySqlCommand("UPDATE `tblins` SET `title`=?title,`FName`=?fname,`MName`=?mname,`LName`=?lname,`Ins_reg`=?reg,`Ins_prov`=?prov,`Ins_cit`=?cit,`Ins_scit`=?scit,`Ins_brgy`=?brgy,`Ins_st`=?st,`Ins_con`=?con", con);
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
                }
            }
            else
            {
                if (bool_title == true && bool_fname == true && bool_lname == true && bool_reg == true && bool_prov == true && bool_cit == true && bool_scit == true && bool_brgy == true)
                {
                    MessageBox.Show("ALL");
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        con.Open();

                        cmd = new MySqlCommand("SELECT * FROM tblins WHERE title='" + cbTitle.SelectedValue.ToString() + "'AND LName = '" + LnameT.Text + "'AND FName ='" + FnameT.Text + "'", con);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Duplicate Entry.", "DUPLICATE ENTRY", MessageBoxButton.OK, MessageBoxImage.Error);
                            con.Close();
                            reader.Close();
                        }
                        else
                        {
                            con.Dispose();
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
                                cmd.Parameters.AddWithValue("@cotn", "+63" + ContactT.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cont", "");
                            }
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Dispose();
                        }
                        MessageBox.Show("The data has been saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }

                }
                else if (bool_title == true && bool_fname == true && bool_lname == true && bool_reg == true && bool_prov == true && bool_cit == true && bool_scit == false && bool_brgy == true)
                {
                    MessageBox.Show("RICE");
                    try
                    {
                        MySqlConnection con = new MySqlConnection(Connection);
                        con.Open();

                        cmd = new MySqlCommand("SELECT * FROM tblins WHERE title='" + cbTitle.SelectedValue.ToString() + "'AND LName = '" + LnameT.Text + "'AND FName ='" + FnameT.Text + "'", con);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Duplicate Entry.", "DUPLICATE ENTRY", MessageBoxButton.OK, MessageBoxImage.Error);
                            con.Close();
                            reader.Close();
                        }
                        else
                        {
                            con.Dispose();
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
                                cmd.Parameters.AddWithValue("@cotn", "+63" + ContactT.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cont", "");
                            }
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Dispose();
                        }
                        MessageBox.Show("The data has been saved successfully.");
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
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Instructors))
                {
                    (window as Instructors).grdcontent.Children.Remove(this);

                }
            }
        }

        
    }
}
