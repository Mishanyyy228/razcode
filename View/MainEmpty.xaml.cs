using lab.Repository;
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
        public MainEmpty(string qwe)
        {
            var em = qwe;
            InitializeComponent();
            if (qwe != null )
            {
                Current_user.Content = em;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var qwe = Current_user.Content;
            var qa = qwe.ToString();
            Manager.MainFrame.Navigate(new Mainxaml(qa));
        }
    }
}
