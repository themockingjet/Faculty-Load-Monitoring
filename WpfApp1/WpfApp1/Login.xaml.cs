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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            btn_Min.MouseEnter += new MouseEventHandler(btn_Min_MouseEnter);
            btn_Min.MouseLeave += new MouseEventHandler(btn_Min_MouseLeave);

            void btn_Min_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Min.Margin = new Thickness(204, -11, 0, 0);
                btn_Min.Height = 28;
                btn_Min.Width = 28;
            }

            void btn_Min_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Min.Margin = new Thickness(200, -14, 0, 0); 
                btn_Min.Height = 32;
                btn_Min.Width = 32;
            }

            btn_Close.MouseEnter += new MouseEventHandler(btn_Close_MouseEnter);
            btn_Close.MouseLeave += new MouseEventHandler(btn_Close_MouseLeave);

            void btn_Close_MouseLeave(object sender, MouseEventArgs e)
            {
                btn_Close.Margin = new Thickness(232,-11,-9,0);
                btn_Close.Height = 28;
                btn_Close.Width = 28;
            }

            void btn_Close_MouseEnter(object sender, MouseEventArgs e)
            {
                btn_Close.Margin = new Thickness(232, -14, -13, 0);
                btn_Close.Height = 32;
                btn_Close.Width = 32;
            }
        }
    }
}
