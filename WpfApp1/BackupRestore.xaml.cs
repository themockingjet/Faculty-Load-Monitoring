using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for BackupRestore.xaml
    /// </summary>
    public partial class BackupRestore : UserControl
    {

        string constring = "server=localhost;user=root;pwd=theblackrose3;database=classschedule;CharSet=utf8";
        static string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FLAMS\\";
        static string FileName = "FLAMS_Backup_";
        public BackupRestore()
        {
            InitializeComponent();
        }

        public static bool SourceFileExists()
        {
            if (Directory.GetFiles(path, "*.sql").Length == 0)
            {
                MessageBox.Show("Unable to find the database. Try to back up database first.", "Import", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdmaincontent.IsEnabled = false;

                }
            }
            if (!SourceFileExists())
                return;

            Exception error = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            mb.ImportInfo.TargetDatabase = "classschedule";
                            mb.ImportInfo.EncryptionPassword = "FLAMS";
                            mb.ImportInfo.EnableEncryption = true;

                            string pattern = "*.sql";
                            var dirInfo = new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FLAMS\\");
                            var latestfile = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

                            mb.ImportFromFile(latestfile.ToString());


                            error = mb.LastError;
                        }

                        conn.Close();
                    }
                }

                if (error == null)
                    MessageBox.Show("Data has been restored successfully.");
                else
                    MessageBox.Show("Finished with errors." + Environment.NewLine + Environment.NewLine + error.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdmaincontent.IsEnabled = true;

                }
            }
            Mouse.OverrideCursor = null;
        }

        private void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdmaincontent.IsEnabled = false;

                }
            }

            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        try
                        {
                            cmd.Connection = conn;
                            mb.ExportInfo.EnableEncryption = true;
                            mb.ExportInfo.EncryptionPassword = "FLAMS";

                            DateTime currentDateTime = DateTime.Now;
                            string formattedDateTime = currentDateTime.ToString("dddd, yyyy-MM-dd HH-mm");

                            string savefile = System.IO.Path.Combine(path, FileName) + formattedDateTime + ".sql";


                            //string savefile = System.IO.Path.GetDirectoryName(path + FileName);
                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }

                            conn.Open();
                            mb.ExportToFile(savefile);
                            conn.Close();
                            MessageBox.Show("Data has been backed-up successfully.");
                        }
                        catch (Exception ex) 
                        {
                            MessageBox.Show(ex.ToString());
                            throw;
                        }
                    }
                }
            }

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdmaincontent.IsEnabled = true;

                }
            }
            Mouse.OverrideCursor = null;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Main))
                {
                    (window as Main).grdcontent.Children.Remove(this);
                    (window as Main).grdmaincontent.IsEnabled = true;

                }
            }
        }
    }
}
