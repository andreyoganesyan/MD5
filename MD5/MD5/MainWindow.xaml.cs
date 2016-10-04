using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace MD5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string[] files;
        MD5HashMaker md5HashMaker = new MD5HashMaker();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        DataGridFiller dataGridFiller = new DataGridFiller();
        DuplicatesFinder duplicateFinder = new DuplicatesFinder();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {

            folderBrowserDialog.ShowDialog();
            files = Directory.GetFiles(folderBrowserDialog.SelectedPath);
            folderPathTB.Text = folderBrowserDialog.SelectedPath;
            dataGridFiller.Fill(hashesDataGrid, duplicateFinder.findDuplicates(folderBrowserDialog.SelectedPath));

            
        }

        private void calcHashBtn_Click(object sender, RoutedEventArgs e)
        {
            



        }
        Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
