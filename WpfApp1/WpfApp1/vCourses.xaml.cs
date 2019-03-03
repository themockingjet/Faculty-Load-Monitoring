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
    /// Interaction logic for vCourses.xaml
    /// </summary>
    public partial class vCourses : UserControl
    {
        public vCourses()
        {
            InitializeComponent();

            

        }

        // Data Grid Columns
        private void DtIns_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Sub_code":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Sub_desc":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "T_units":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Year":
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Course":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "Semester":
                    e.Column.Visibility = Visibility.Visible;
                    break;
                default:
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void CdescT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CdescT.Text.Length != 0)
            {
                CdescW.Visibility = Visibility.Collapsed;
            }
            else
            {
                CdescW.Visibility = Visibility.Hidden;
            }
            if (CcodeT.LineCount > 1)
            {
                bdrCcode.Height = 60;
            }
            else
            {
                bdrCcode.Height = 40;
            }
        }

        private void CcodeT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CcodeT.Text.Length != 0)
            {
                CcodeW.Visibility = Visibility.Collapsed;
            }
            else
            {
                CcodeW.Visibility = Visibility.Hidden;
            }
        }
    }
}
