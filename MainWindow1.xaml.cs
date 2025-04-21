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
        ITodoRepository todoRepository;
        private DispatcherTimer timer;
        public MainWindow1()
        {
            todoRepository = new TodoRepository();

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
                var thisToken = DataBaseService.GetLastAddedToken();
                var repository = new FileRepository();
                var infoUser = await repository.GetUser(thisToken);

                // Проверяем, что токен вообще существует
                if (string.IsNullOrEmpty(thisToken))
                {
                    // Если токена нет, переходим на страницу авторизации
                    MainFrame.Navigate(new LogIn());
                    Manager.MainFrame = MainFrame;
                    progressBar.Visibility = Visibility.Collapsed;
                    timer.Stop();
                    return;
                }

                // Дальнейшая логика продолжается только если токен есть
                if (infoUser != null)
                {
                    var reptodosTask2 = ReturnFalseTodos();
                    bool reptodos1 = await reptodosTask2;
                    progressBar.Visibility = Visibility.Collapsed;
                    timer.Stop();
                    Manager.MainFrame = MainFrame;

                    if (reptodos1 == true)
                    {
                        Manager.MainFrame.Navigate(new Mainxaml());
                    }
                    else
                    {
                        Manager.MainFrame.Navigate(new MainEmpty());
                    }
                }
                else
                {
                    // Если токен есть, но пользователь не найден
                    MainFrame.Navigate(new LogIn());
                    Manager.MainFrame = MainFrame;
                    progressBar.Visibility = Visibility.Collapsed;
                    timer.Stop();
                }
            }
            else
            {
                progressBar.Value += 5;
            }
        }
        public async Task<bool> ReturnFalseTodos()
        {
            var allTodos = await todoRepository.GetTodosAsync();
            var completedTodos = allTodos.Where(todo => todo.isCompleted == false);
            var countCompleatedTodos = completedTodos.Count();
            if (countCompleatedTodos == 0)
            {
                return false;
            }
            return true;
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
