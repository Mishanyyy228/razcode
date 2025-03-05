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
        public MainEmpty(string qwe, string eqrwer)
        {
            var em = qwe;
            var pas = eqrwer;
            InitializeComponent();
            if (qwe != null & eqrwer != null)
            {
                var qazwsx = new UserRepository();
                var qwert = qazwsx.GetUser(qwe, eqrwer);
                Current_user.Content = qwert.Name;
            }
        }
        public  string Em { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var qwe = Current_user.Content;
            var qa = qwe.ToString();
            Manager.MainFrame.Navigate(new Mainxaml(qa));
        }
    }
}
