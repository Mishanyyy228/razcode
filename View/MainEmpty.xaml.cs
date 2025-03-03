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

namespace lab
{
    /// <summary>
    /// Логика взаимодействия для MainEmpty.xaml
    /// </summary>
    public partial class MainEmpty : Page
    {
        public MainEmpty()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Mainxaml(null,null));

            //Mainxaml main = new Mainxaml();
            //WindowManager.SwitchWindow(this, main);
        }
    }
}
