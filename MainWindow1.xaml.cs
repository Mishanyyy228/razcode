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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow1.xaml
    /// </summary>
    public partial class MainWindow1 : Window
    {
        public MainWindow1()
        {
            InitializeComponent();
            MainFrame.Navigate(new LogIn());
            Manager.MainFrame = MainFrame;
        }
        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var fa = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            (e.Content as Page).BeginAnimation(OpacityProperty, fa);
        }
    }
}
