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
using lab.model;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;





namespace lab
{
    /// <summary>
    /// Логика взаимодействия для Mainxaml.xaml
    /// </summary>
    public partial class Mainxaml : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ObservableCollection<Todo> Tasks { get; set; }

        public Mainxaml(string nameUser)
        {
            var tasks = new ObservableCollection<Todo>();
            this.Tasks = tasks;
            DataContext = this;

            var uniqueCategories = tasks.Select(t => t.category).Distinct().ToList();

            DataContext = this;
            InitializeComponent();
            
            if(nameUser != null)
            {
                UserBox.Content = nameUser;
            }

            Loaded += OnLoaded;
            DataContext = this;

            ClearElement clearElement = new ClearElement();
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, Taske_List, null);
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var repository = new RepositoryFile();
            var infoUser = await repository.GetImageUser(Image_UserDefault ,Image_User1);
            Image_User1.Source = infoUser;
            await LoadCategoryAsync();
            await FalseTaskAsync();
        }
        public async Task LoadCategoryAsync()
        {
            ObservableCollection<string> Categories = new ObservableCollection<string>();
            Categories.Add("Все");
            var repository = new TodoRepository();
            var allTasks = await repository.GetTodosAsync();

            Tasks.Clear();

            foreach (var task in allTasks)
            {
                if (!Categories.Contains(task.category))
                {
                    Categories.Add(task.category);
                }
            }
            Category_List.ItemsSource = null;
            Category_List.ItemsSource = Categories;
        }
        public async Task TrueTaskAsync()
        {
            var repository = new TodoRepository();
            var allTodos = await repository.GetTodosAsync();  
            var completedTodos = allTodos.Where(todo => todo.isCompleted == true);

            Tasks.Clear();

            foreach (var todo in completedTodos)
            {
                Tasks.Add(todo);
            }

            Taske_List.ItemsSource = null;
            Taske_List.ItemsSource = Tasks;
        }
        public async Task FalseTaskAsync()
        {
            var repository = new TodoRepository();
            var allTasks = await repository.GetTodosAsync();

            var completedTasks = allTasks.Where(task => task.isCompleted == false);

            Tasks.Clear(); 
            foreach (var task in completedTasks)
            {
                Tasks.Add(task); 
            }
            Task_List.ItemsSource = null;
            Task_List.ItemsSource = Tasks;
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
            var repository = new TodoRepository();
            var currentImage = await repository.TodosIsReady(classTask, Task_List);
            MessageBox.Show(currentImage);
            await TrueTaskAsync();
            await LoadCategoryAsync();
            await FalseTaskAsync();
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            gridthick.BorderBrush = Brushes.White;
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo,null,null);
            gridthick1.BorderBrush = Brushes.White;
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Todo classTask = (Todo)Task_List.SelectedItem;
            var repository = new TodoRepository();
            var currentImage = await repository.DeleteTodos(classTask,Task_List);
            MessageBox.Show(currentImage);
            await LoadCategoryAsync();
            await FalseTaskAsync();
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
            gridthick.BorderBrush = Brushes.White;
            clearElement.HiddenElement(Buttone_Delete, Buttone_Gotovo, null,null);
            gridthick1.BorderBrush = Brushes.White;
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ClearElement clearElement = new ClearElement();
            clearElement.ClearText(TaskName, TaskDateTime, TaskDate, TaskDescriotion);
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
                await TrueTaskAsync();
                await LoadCategoryAsync();
                await FalseTaskAsync();
                var uniqueCategories = Tasks.Select(t => t.category).Distinct().ToList();
            }
        }
        private async void Image_UserDefault_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var repository = new RepositoryFile();
            var currentImage = await repository.PostAndGetImageUser( Image_UserDefault, Image_User1);
            Image_User1.Source = currentImage;
        }

        private async void Image_User1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var repository = new RepositoryFile();
            var currentImage = await repository.PostAndGetImageUser(Image_UserDefault, Image_User1);
            Image_User1.Source = currentImage;
        }
    }
}

