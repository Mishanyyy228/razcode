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
using lab.Repository;
using lab.DataBase;
using System.Windows.Threading;


namespace lab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow1.xaml
    /// </summary>
    public partial class MainWindow1 : Window
    {
        private DispatcherTimer timer;
        public MainWindow1()
        {
            InitializeComponent();
            progressBar.Visibility = Visibility.Visible;

            Loaded += OnLoaded;
        }
        public bool IsWindowOpen<T>() where T : Window
        {
            return Application.Current.Windows.OfType<T>().Any();
        }
        private async void OnTimerTick(object sender, EventArgs e)
        {
            if (progressBar.Value >= 100)
            {
                timer.Stop();
                var thisToken = BaseConnect.GetLastAddedToken();
                var repository = new FileRepository();
                var infoUser = await repository.GetUser(thisToken);
                if (infoUser != null)
                {
                    MainFrame.Navigate(new Mainxaml());
                    Manager.MainFrame = MainFrame;
                    progressBar.Visibility = Visibility.Collapsed;
                }
                if (infoUser == null)
                {
                    MainFrame.Navigate(new LogIn());
                    Manager.MainFrame = MainFrame;
                    progressBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                progressBar.Value += 10;
            }
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += OnTimerTick;
            timer.Start(); 
        }
        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content is Page page)
            {
                var fa = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                page.BeginAnimation(OpacityProperty, fa);
            }
        }
    }
}
