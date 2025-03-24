using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
using lab.model;
using lab.Repository;


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

            Loaded += OnLoaded;


            MainFrame.Navigate(new LogIn());
            Manager.MainFrame = MainFrame;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var thisToken = BaseConnect.GetLastAddedToken();
            //MessageBox.Show(thisToken);
            var repository = new FileRepository();
            var infoUser = await repository.GetUser(thisToken);
            if (infoUser != null)
            {
                MainFrame.Navigate(new MainEmpty(infoUser, thisToken));
                Manager.MainFrame = MainFrame;
            }
        }
        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var fa = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            (e.Content as Page).BeginAnimation(OpacityProperty, fa);
        }

        private void Exit_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Удаление токена
            BaseConnect.DeleteLastAddedToken();
            this.Close();
            var add = new TaskNew();
            if (add.ShowDialog() == true)
            {
                add.Close();
            }
        }
    }
}
