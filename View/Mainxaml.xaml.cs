using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using lab.Repository;
using lab.Validation;
using System.Reflection.Emit;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoEntities;
using taskLibrary;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using lab.DataBase;





namespace lab
{
    /// <summary>
    /// Логика взаимодействия для Mainxaml.xaml
    /// </summary>
    public partial class Mainxaml : Page, INotifyPropertyChanged
    {
        IFileRepository fileRepository;
        ITodoRepository todoRepository;
        private bool _isLoaded = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Todo> Tasks { get; set; }
        private bool _loadedOnce = false;
        public Mainxaml()
        {
            fileRepository = new FileRepository();
            todoRepository = new TodoRepository();

            var tasks = new ObservableCollection<Todo>();
            this.Tasks = tasks;

            var uniqueCategories = tasks.Select(t => t.category).Distinct().ToList();

            DataContext = this;
            InitializeComponent();

            for (int i = 0; i < 1; i++)
            {
                Loaded += OnLoadedPage;
            }
            Loaded += OnLoaded;

            ClearElement clearElement = new ClearElement();
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, Taske_List, null);
        }
        private async Task<bool> ReturnTodos()
        {
            var todosUser = await todoRepository.GetTodosAsync();
            var Todos = todosUser.Count();
            if (Todos >= 0)
            {
                return true;
            }
            return false;
        }
        private async Task<bool> ReturnTrueTodos()
        {
            var allTodos = await todoRepository.GetTodosAsync();
            var completedTodos = allTodos.Where(todo => todo.isCompleted == true);
            var countCompleatedTodos = completedTodos.Count();
            if (countCompleatedTodos == 0)
            {
                return false;
            }
            return true;
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

        private async void OnLoadedPage(object sender, RoutedEventArgs e)
        {
            if (App.IsPageLoaded)
            {
                return;
            }

            App.IsPageLoaded = true; 

            var reptodosTask2 = ReturnFalseTodos();
            bool reptodos1 = await reptodosTask2;

            if (reptodos1 == true)
            {
                Manager.MainFrame.Navigate(new Mainxaml());
            }
            else
            {
                Manager.MainFrame.Navigate(new MainEmpty());
            }
        }
        public partial class App : Application
        {
            public static bool IsPageLoaded { get; set; } 

        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var tokenOfUser = BaseConnect.GetLastAddedToken();
            var name = await fileRepository.GetUser(tokenOfUser);
            UserBox.Content = name;
            var infoUser = await fileRepository.GetImageUser(Image_UserDefault, Image_User1);
            Image_User1.Source = infoUser;
            await LoadCategoryFalseAsync();
            await FalseTaskAsync();
        }
        public async Task LoadCategoryTrueAsync()
        {
            ObservableCollection<string> Categories = new ObservableCollection<string>();
            var reptodosTask = ReturnTrueTodos();
            bool reptodos = await reptodosTask;

            if (reptodos==true)
            {
                Categories.Add("Все");
            }
            var allTodos = await todoRepository.GetTodosAsync();
            var completedTodos = allTodos.Where(todo => todo.isCompleted == true);
            Tasks.Clear();

            foreach (var task in completedTodos)
            {
                if (!Categories.Contains(task.category))
                {
                    Categories.Add(task.category);
                }
            }
            Category_List.ItemsSource = null;
            Category_List.ItemsSource = Categories;
        }
        public async Task LoadCategoryFalseAsync()
        {
            ObservableCollection<string> Categories = new ObservableCollection<string>();
            var reptodosTaskFalse = ReturnFalseTodos();
            bool reptodosFalse = await reptodosTaskFalse;

            if (reptodosFalse == true)
            {
                Categories.Add("Все");
            }
            var allTodosFalse = await todoRepository.GetTodosAsync();
            var FalseTodos = allTodosFalse.Where(todo => todo.isCompleted == false);
            Tasks.Clear();

            foreach (var taskFalse in FalseTodos)
            {
                if (!Categories.Contains(taskFalse.category))
                {
                    Categories.Add(taskFalse.category);
                }
            }
            Category_List.ItemsSource = null;
            Category_List.ItemsSource = Categories;
        }
        public async Task TrueTaskAsync()
        {
            var allTodos = await todoRepository.GetTodosAsync();  
            var completedTodos = allTodos.Where(todo => todo.isCompleted == true);

            Tasks.Clear();

            foreach (var todo in completedTodos)
            {
                Tasks.Add(todo);
            }

            Taske_List.ItemsSource = null;
            Taske_List.ItemsSource = Tasks.Reverse();
        }
        public async Task FalseTaskAsync()
        {
            var allTasks = await todoRepository.GetTodosAsync();

            var completedTasks = allTasks.Where(task => task.isCompleted == false);

            Tasks.Clear(); 
            foreach (var task in completedTasks)
            {
                Tasks.Add(task);
            }
            Task_List.ItemsSource = null;
            Task_List.ItemsSource = Tasks.Reverse();
        }
        private void Task_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            Todo classTask = (Todo)Task_List.SelectedItem;
            if (classTask != null)
            {
                long timestamp = classTask.date;

                DateTime combinedDateTime = DateTime.FromBinary(timestamp);

                string datePart = combinedDateTime.ToShortDateString();
                string timePart = combinedDateTime.ToLongTimeString();

                TaskDateTime.Text = datePart;
                TaskDate.Text = timePart;
                TaskName.Content = classTask.title;
                TaskDescriotion.Text = classTask.description;
                gridthick.BorderThickness = new Thickness(1);
                gridthick.BorderBrush = Brushes.Black;
            }
            Buttone_Delete.Visibility = Visibility.Visible;
            Buttone_Gotovo.Visibility = Visibility.Visible;
        }
        private void Taske_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearElement clearElement = new ClearElement();
            Todo classTask = (Todo)Taske_List.SelectedItem;

            if (classTask != null)
            {
                long timestamp = classTask.date;
                DateTime combinedDateTime = DateTime.FromBinary(timestamp);

                string datePart = combinedDateTime.ToShortDateString();
                string timePart = combinedDateTime.ToLongTimeString();

                TaskDateTime.Text = datePart;
                TaskDate.Text = timePart;
                TaskName.Content = classTask.title;
                TaskDescriotion.Text = classTask.description;
                gridthick.BorderThickness = new Thickness(1);
                gridthick.BorderBrush = Brushes.Black;
            }
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, null, null);
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Todo classTask = (Todo)Task_List.SelectedItem;
            var currentImage = await todoRepository.TodosIsReady(classTask, Task_List);
            MessageBox.Show(currentImage);
            await LoadCategoryFalseAsync();
            await FalseTaskAsync();
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            gridthick.BorderBrush = Brushes.White;
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo,null,null);
            gridthick1.BorderBrush = Brushes.White;
            var reptodosTask2 = ReturnFalseTodos();
            bool reptodos1 = await reptodosTask2;

            if (reptodos1 == true)
            {
                Manager.MainFrame.Navigate(new Mainxaml());
            }
            else
            {
                Manager.MainFrame.Navigate(new MainEmpty());
            }
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Todo classTask = (Todo)Task_List.SelectedItem;
            var currentImage = await todoRepository.DeleteTodos(classTask,Task_List);
            MessageBox.Show(currentImage);
            await LoadCategoryFalseAsync();
            await FalseTaskAsync();
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            gridthick.BorderBrush = Brushes.White;
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, null,null);
            gridthick1.BorderBrush = Brushes.White;
            var reptodosTask2 = ReturnFalseTodos();
            bool reptodos1 = await reptodosTask2;
            if (reptodos1 == true)
            {
                Manager.MainFrame.Navigate(new Mainxaml());
            }
            else
            {
                Manager.MainFrame.Navigate(new MainEmpty());
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            await LoadCategoryTrueAsync();
            await TrueTaskAsync();
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, Task_List, Taske_List);
            NewTask_Image.Visibility = Visibility.Hidden;
            gridthick.BorderBrush = Brushes.White;
            gridthick1.BorderBrush = Brushes.White;
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            gridthick.BorderBrush = Brushes.White;
            gridthick1.BorderBrush = Brushes.White;
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, Taske_List ,Task_List);
            NewTask_Image.Visibility = Visibility.Visible;
            await LoadCategoryFalseAsync();
            await FalseTaskAsync();
        }

        private void Category_List_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ClearElement clearElement = new ClearElement();

            string selectedCity = Category_List.SelectedItem as string;
            var filteredPeople = Tasks.Where(p => p.category == selectedCity).ToList();
            if (filteredPeople.Count <= 0 && selectedCity != "Все")
            {
                MessageBox.Show("В данной категории нет задач.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                gridthick.BorderBrush = Brushes.White;
                gridthick1.BorderBrush = Brushes.White;
                clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, null, null);
                Task_List.ItemsSource = Tasks;
                Taske_List.ItemsSource = Tasks;
            }
            else
            {
                Task_List.ItemsSource = filteredPeople.ToList();
                Taske_List.ItemsSource = filteredPeople.ToList();
                clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
                clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo,null,null);
                if (selectedCity == "Все")
                {
                    gridthick.BorderBrush = Brushes.White;
                    gridthick1.BorderBrush = Brushes.White;
                    clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
                    Task_List.ItemsSource = Tasks;
                    Taske_List.ItemsSource = Tasks;
                }
                if (filteredPeople == null)
                {
                    gridthick.BorderBrush = Brushes.White;
                    gridthick1.BorderBrush = Brushes.White;
                    var filteredPeoples = Tasks.Where(p => p.category == selectedCity);
                    Task_List.ItemsSource = filteredPeoples.ToList();
                    Taske_List.ItemsSource = filteredPeople.ToList();
                    clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
                }
            }
        }

        private async void NewTask_Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var add = new TaskNew();
            if (add.ShowDialog() == true)
            {
                await LoadCategoryFalseAsync();
                await FalseTaskAsync();
                var uniqueCategories = Tasks.Select(t => t.category).Distinct().ToList();
            }
        }
        private async void Image_UserDefault_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentImage = await fileRepository.PostAndGetImageUser( Image_UserDefault, Image_User1);
            Image_User1.Source = currentImage;
        }

        private async void Image_User1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentImage = await fileRepository.PostAndGetImageUser(Image_UserDefault, Image_User1);
            Image_User1.Source = currentImage;
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            BaseConnect.DeleteLastAddedToken();
            Manager.MainFrame.Navigate(new LogIn());
        }
    }
}

