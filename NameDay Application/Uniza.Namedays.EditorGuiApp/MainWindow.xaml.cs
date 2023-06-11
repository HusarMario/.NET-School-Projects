using Microsoft.Win32;
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
using Uniza.Namedays.EditorGUIApp;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        DateTime time = DateTime.Now;
        NamedayCalendar calendar = new NamedayCalendar();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (calendar.NameCount != 0)
            {
                MessageBoxResult result = MessageBox.Show("Naozaj chcete vynulovať kalednár?","Resetovať kalendár", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            calendar.Clear();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                FileInfo file = new FileInfo(path);
                calendar.Load(file);
            }
      
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                FileInfo file = new FileInfo(path);
                calendar.Save(file);
            }

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new();
            about.ShowDialog();
        }

        private void Today_Click(object sender, RoutedEventArgs e)
        {
            time = DateTime.Now;
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            regex.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Additional additional = new(calendar);
            additional.ShowDialog();
        }
    }
}
