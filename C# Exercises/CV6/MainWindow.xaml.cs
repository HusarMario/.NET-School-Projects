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

namespace CV6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowHelloButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello World!", "My WPF App");
        }

        private void ChangeBackgroundColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Would you like to change background color?",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Background = Brushes.DeepSkyBlue;
            }
        } 
    }
}
