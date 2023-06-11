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
using System.Windows.Shapes;

namespace Uniza.Namedays.EditorGUIApp
{
    /// <summary>
    /// Interaction logic for Additional.xaml
    /// </summary>
    public partial class Additional : Window
    {
        NamedayCalendar calendar;
        public Additional(NamedayCalendar calendar)
        {
            InitializeComponent();
            this.calendar = calendar;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //calendar.Add(date.Data, name.Text);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
