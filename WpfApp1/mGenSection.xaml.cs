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
    /// Interaction logic for mGenSection.xaml
    /// </summary>
    public partial class mGenSection : UserControl
    {
        string Connection = "SERVER=localhost;DATABASE=classschedule;UID=root;PASSWORD=theblackrose3;";
        MySqlCommand cmd;
        MySqlConnection con;

        public mGenSection()
        {
            InitializeComponent();
            ComboBox_Manage_Course();
        }

        void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]$"))
            {
                e.Handled = true;
            }
        }

        void submit_check()
        {
            if(txt1st.Text == "" || txt2nd.Text == "" || txt3rd.Text == "" || txt4th.Text == "" || txt1st.Text == "0" || txt2nd.Text == "0" || txt3rd.Text == "0" || txt4th.Text == "0" || cbCourse.SelectedIndex == -1)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }
        }

        private void Txt1st_TextChanged(object sender, TextChangedEventArgs e)
        {
            submit_check();
        }

        
        void ComboBox_Manage_Course()
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
        
        int sectionid = 0;
        int x1 = 0;
        int x2 = 0;
        int x3 = 0;
        int x4 = 0;
        int xx1 = 0;
        int xx2 = 0;
        int xx3 = 0;
        int xx4 = 0;
        int t1 = 0;
        int t2 = 0;
        int t3 = 0;
        int t4 = 0;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.IsHitTestVisible = false;

            x1 = int.Parse(txt1st.Text);
            x2 = int.Parse(txt2nd.Text);
            x3 = int.Parse(txt3rd.Text);
            x4 = int.Parse(txt4th.Text);

            xx1 = x1 % 40;
            xx2 = x2 % 40;
            xx3 = x3 % 40;
            xx4 = x4 % 40;

            generate_firstyear();
            generate_secondyear();
            generate_thirdyear();
            generate_fourthyear();

            this.IsHitTestVisible = true;
            Mouse.OverrideCursor = null;
            MessageBox.Show(string.Format("No. of generated section for:\nFirst Year: {0}\nSecond Year: {1}\nThird Year: {2}\nFourth Year: {3}", t1,t2,t3,t4));
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(managesec))
                {
                    (window as managesec).grdcontent.Children.Remove(this);
                    (window as managesec).bdrContent.IsEnabled = true;
                    (window as managesec).Load_Grid();
                }
            }
        }
        
        void generate_firstyear()
        {
            if (x1 != 0)
            {
                int noofsection1st = x1 / 40;
                if (xx1 != 0 && xx1 > 15)
                {
                    noofsection1st += 1;
                }

                con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT COUNT(*) FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 1 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                con.Open();
                string noofresult = cmd.ExecuteScalar().ToString();
                con.Dispose();

                if (int.Parse(noofresult) >= noofsection1st)
                {
                    for (int y = 1; y <= noofsection1st; y++)
                    {
                        cmd = new MySqlCommand("SELECT Sec_id FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 1 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                        con.Open();
                        sectionid = (Int32)cmd.ExecuteScalar();
                        con.Dispose();

                        cmd = new MySqlCommand("INSERT INTO `tblopensec`(`Sec_id`, `Sem_id`, `Sy_id`) VALUES (@sec,@sem,@sy)", con);
                        cmd.Parameters.AddWithValue("@sec", sectionid);
                        cmd.Parameters.AddWithValue("@sem", Main.semester_id);
                        cmd.Parameters.AddWithValue("@sy", Main.schoolyear_id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();
                        t1 += 1;
                    }
                }
                else if (int.Parse(noofresult) < noofsection1st)
                {
                    MessageBox.Show("No. of remaining inactive section for first year: " + noofsection1st + "\nNo. of sections to be set as active for first year: " + noofresult, "FAILED");
                }
            }
        }

        void generate_secondyear()
        {
            if (x2 != 0)
            {
                int noofsection2nd = x2 / 40;
                if (xx2 != 0 && xx2 > 15)
                {
                    noofsection2nd += 1;
                }

                con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT COUNT(*) FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 2 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                con.Open();
                string noofresult = cmd.ExecuteScalar().ToString();
                con.Dispose();

                if (int.Parse(noofresult) >= noofsection2nd)
                {
                    for (int y = 1; y <= noofsection2nd; y++)
                    {
                        cmd = new MySqlCommand("SELECT Sec_id FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 2 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                        con.Open();
                        sectionid = (Int32)cmd.ExecuteScalar();
                        con.Dispose();

                        cmd = new MySqlCommand("INSERT INTO `tblopensec`(`Sec_id`, `Sem_id`, `Sy_id`) VALUES (@sec,@sem,@sy)", con);
                        cmd.Parameters.AddWithValue("@sec", sectionid);
                        cmd.Parameters.AddWithValue("@sem", Main.semester_id);
                        cmd.Parameters.AddWithValue("@sy", Main.schoolyear_id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();
                        t2 += 1;
                    }
                }
                else if (int.Parse(noofresult) < noofsection2nd)
                {
                    MessageBox.Show("No. of remaining inactive section for first year: " + noofsection2nd + "\nNo. of sections to be set as active for first year: " + noofresult, "FAILED");
                }
            }
        }

        void generate_thirdyear()
        {
            if (x3 != 0)
            {
                int noofsection3rd = x3 / 40;
                if (xx3 != 0 && xx3 > 15)
                {
                    noofsection3rd += 1;
                }

                con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT COUNT(*) FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 3 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                con.Open();
                string noofresult = cmd.ExecuteScalar().ToString();
                con.Dispose();

                if (int.Parse(noofresult) >= noofsection3rd)
                {
                    for (int y = 1; y <= noofsection3rd; y++)
                    {
                        cmd = new MySqlCommand("SELECT Sec_id FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 3 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                        con.Open();
                        sectionid = (Int32)cmd.ExecuteScalar();
                        con.Dispose();

                        cmd = new MySqlCommand("INSERT INTO `tblopensec`(`Sec_id`, `Sem_id`, `Sy_id`) VALUES (@sec,@sem,@sy)", con);
                        cmd.Parameters.AddWithValue("@sec", sectionid);
                        cmd.Parameters.AddWithValue("@sem", Main.semester_id);
                        cmd.Parameters.AddWithValue("@sy", Main.schoolyear_id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();
                        t3 += 1;
                    }
                }
                else if (int.Parse(noofresult) < noofsection3rd)
                {
                    MessageBox.Show("No. of remaining inactive section for first year: " + noofsection3rd + "\nNo. of sections to be set as active for first year: " + noofresult, "FAILED");
                }
            }
        }

        void generate_fourthyear()
        {
            if (x4 != 0)
            {
                int noofsection4th = x4 / 40;
                if (xx4 != 0 && xx4 > 15)
                {
                    noofsection4th += 1;
                }

                con = new MySqlConnection(Connection);
                cmd = new MySqlCommand("SELECT COUNT(*) FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 4 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                con.Open();
                string noofresult = cmd.ExecuteScalar().ToString();
                con.Dispose();

                if (int.Parse(noofresult) >= noofsection4th)
                {
                    for (int y = 1; y <= noofsection4th; y++)
                    {
                        cmd = new MySqlCommand("SELECT Sec_id FROM `tblsection` WHERE Sec_id NOT IN (SELECT Sec_id FROM tblopensec) AND Sec_year = 4 AND Course_id = '" + cbCourse.SelectedValue.ToString() + "'", con);
                        con.Open();
                        sectionid = (Int32)cmd.ExecuteScalar();
                        con.Dispose();

                        cmd = new MySqlCommand("INSERT INTO `tblopensec`(`Sec_id`, `Sem_id`, `Sy_id`) VALUES (@sec,@sem,@sy)", con);
                        cmd.Parameters.AddWithValue("@sec", sectionid);
                        cmd.Parameters.AddWithValue("@sem", Main.semester_id);
                        cmd.Parameters.AddWithValue("@sy", Main.schoolyear_id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Dispose();
                        t4 += 1;
                    }
                }
                else if (int.Parse(noofresult) < noofsection4th)
                {
                    MessageBox.Show("No. of remaining inactive section for first year: " + noofsection4th + "\nNo. of sections to be set as active for first year: " + noofresult, "FAILED");
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(managesec))
                {
                    (window as managesec).grdcontent.Children.Remove(this);
                    (window as managesec).bdrContent.IsEnabled = true;
                }
            }
        }

        private void CbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            submit_check();
        }
    }
}
