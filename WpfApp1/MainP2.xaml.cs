using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainP2.xaml
    /// </summary>
    public partial class MainP2 : Page
    {
        public MainP2()
        {
            InitializeComponent();
        }

        private void Btn_backuprestore_Click(object sender, RoutedEventArgs e)
        {
            BackupRestore UCobj = new BackupRestore();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdcontent.Children.Add(UCobj);
                    (window as Main).grdmaincontent.IsEnabled = false;
                }
            }
        }

        private void Btn_schedins_Click(object sender, RoutedEventArgs e)
        {
            vInssched win = new vInssched();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_schedroom_Click(object sender, RoutedEventArgs e)
        {
            vRmsched win = new vRmsched();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }
        

        private void Btn_schedsec_Click(object sender, RoutedEventArgs e)
        {
            vSecsched win = new vSecsched();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_sy_Click(object sender, RoutedEventArgs e)
        {
            Schoolyear win = new Schoolyear();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }

        private void Btn_user_Click(object sender, RoutedEventArgs e)
        {
            mS win = new mS();
            win.Show();
            var MainW = Main.GetWindow(this);
            if (MainW != null)
            {
                MainW.Close();
            }
        }
    }
}
