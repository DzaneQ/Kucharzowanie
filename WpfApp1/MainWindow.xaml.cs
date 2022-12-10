using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window5 window_start = new Window5();
            Close();
            window_start.Show(); 
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool isHelperOpen = false;
            foreach (Window opened_window in Application.Current.Windows)
            {
                if (opened_window is Window3)
                {
                    opened_window.Activate();
                    isHelperOpen = true;
                }
            }
            if (!isHelperOpen)
            {
                Window6 credits = new Window6();
                credits.Show();
            }
        }
    }
}
