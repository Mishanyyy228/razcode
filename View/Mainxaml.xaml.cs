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

        private List<string> _uniqueCategoriesList;

        public List<string> UniqueCategoriesList
        {
            get => _uniqueCategoriesList;
            set
            {
                _uniqueCategoriesList = value;
                OnPropertyChanged();
            }
        }

        public Mainxaml(string nameUser)
        {
            var tasks = new ObservableCollection<Todo>();
            this.Tasks = tasks;
            DataContext = this;

            var uniqueCategories = tasks.Select(t => t.category).Distinct().ToList();

            UniqueCategoriesList = new List<string>(uniqueCategories);

            DataContext = this;
            InitializeComponent();
            
            if(nameUser != null)
            {
                UserBox.Content = nameUser;
            }

            Loaded += OnLoaded;
            Taske_List.ItemsSource = Tasks;
            DataContext = this;
            Category_List.ItemsSource = UniqueCategoriesList;
            DataContext = this;

            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            Taske_List.Visibility = Visibility.Hidden;
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await LoadTasksAsync();
            Task_List.ItemsSource = Tasks;

            await LoadCategoryAsync();
            var classTask = new Todo();
            long timestamp = classTask.date;
            DateTime dateTime = DateTime.FromBinary(timestamp);

            DateTime combinedDateTime = DateTime.FromBinary(timestamp);

            string datePart = combinedDateTime.ToShortDateString();
            string timePart = combinedDateTime.ToLongTimeString();
        }
        public async Task LoadCategoryAsync()
        {
            var repository = new Repository1();
            var updatedTodolist = await repository.GetTodosAsync();

            var categories = updatedTodolist.Select(task => task.category).Distinct().ToList();

            Category_List.ItemsSource = categories;
        }
        public async Task LoadTasksAsync()
        {
            var repository = new Repository1();
            var updatedTodolist = await repository.GetTodosAsync();
            foreach (var task in updatedTodolist)
            {
                Tasks.Add(task);
            }
            Task_List.ItemsSource = Tasks;
        }

        private void Task_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskName.Content = string.Empty;
            TaskDescriotion.Text = "";
            TaskDate.Text = string.Empty;
            TaskDateTime.Text = string.Empty;
            Todo classTask = (Todo)Task_List.SelectedItem;
            if (classTask != null)
            {
                if ( classTask !=null)
                {
                    long timestamp = classTask.date; 
                    DateTime dateTime = DateTime.FromBinary(timestamp);

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
            }
            Buttone_Delete.Visibility = Visibility.Visible;
            Buttone_Gotovo.Visibility = Visibility.Visible;
        }
        private void Taske_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Todo classTask = (Todo)Taske_List.SelectedItem;
            if (classTask == null)
            {

            }

            if (classTask != null)
            {
                TaskDate.Text = classTask.date.ToString();
                TaskDateTime.Text = classTask.date.ToString();
                TaskName.Content = classTask.title;
                TaskDescriotion.Text = classTask.description;
                gridthick1.BorderThickness = new Thickness(1);
                gridthick1.BorderBrush = Brushes.Black;
            }
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://45.144.64.179/");
            Todo classTask = (Todo)Task_List.SelectedItem;

            if (classTask != null)
            {
                var taskId = classTask.id;
                var content = new StringContent(taskId, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                var result = await client.PutAsync($"api/todos/mark/{taskId}", content);
                if (result.IsSuccessStatusCode)
                {
                    var repository = new Repository1();
                    MessageBox.Show("Задача выполнена");
                    await LoadTasksAsync();
                }
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Задача не выполнена. Возникла ошибка: {(int)result.StatusCode}. Сообщение: {await result.Content.ReadAsStringAsync()}");
                }
            }
            TaskName.Content = "";
            TaskDescriotion.Text = "";
            TaskDate.Text = "";
            TaskDateTime.Text = "";
            gridthick.Visibility = Visibility.Hidden;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            gridthick.Visibility = Visibility.Hidden;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://45.144.64.179/");
            Todo classTask = (Todo)Task_List.SelectedItem;

            if (classTask != null)
            {
                var taskId = classTask.id;
                var content = new StringContent(taskId, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                var result = await client.DeleteAsync($"/api/todos/{taskId}");
                if (result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Задача удалена");
                }
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Задача не удалена. Возникла ошибка: {(int)result.StatusCode}. Сообщение: {await result.Content.ReadAsStringAsync()}");
                }
            }
            TaskName.Content = " ";
            TaskDescriotion.Text = " ";
            TaskDate.Text = " ";
            TaskDateTime.Text = " ";
            gridthick.Visibility = Visibility.Hidden;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            gridthick.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            TaskName.Content = "";
            TaskDescriotion.Text = "";
            TaskDate.Text = "";
            TaskDateTime.Text = "";
            NewTask_Image.Visibility = Visibility.Hidden;
            Task_List.Visibility = Visibility.Hidden;
            Taske_List.Visibility = Visibility.Visible;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            gridthick.Visibility = Visibility.Hidden;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            gridthick1.Visibility = Visibility.Hidden;
            Taske_List.Visibility = Visibility.Hidden;
            Task_List.Visibility = Visibility.Visible;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            NewTask_Image.Visibility = Visibility.Visible;
            TaskName.Content = "";
            TaskDescriotion.Text = "";
            TaskDate.Text = "";
            TaskDateTime.Text = "";
        }

        private void Category_List_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string selectedCity = Category_List.SelectedItem as string;
            var filteredPeople = Tasks.Where(p => p.category == selectedCity).ToList();
            if (filteredPeople.Count <= 0 && selectedCity != "Все")
            {
                MessageBox.Show("В данной категории нет задач.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Task_List.ItemsSource = filteredPeople.ToList();
                Taske_List.ItemsSource = filteredPeople.ToList();
                TaskName.Content = "";
                TaskDescriotion.Text = "";
                TaskDate.Text = "";
                TaskDateTime.Text = "";
                Buttone_Delete.Visibility = Visibility.Hidden;
                Buttone_Gotovo.Visibility = Visibility.Hidden;
                if (selectedCity == "Все")
                {
                    TaskName.Content = "";
                    TaskDescriotion.Text = "";
                    TaskDate.Text = "";
                    TaskDateTime.Text = "";
                    Task_List.ItemsSource = Tasks;
                    Taske_List.ItemsSource = Tasks;
                }
                if (filteredPeople == null)
                {
                    var filteredPeoples = Tasks.Where(p => p.category == selectedCity);
                    Task_List.ItemsSource = filteredPeoples.ToList();
                    Taske_List.ItemsSource = filteredPeoples.ToList();
                    TaskName.Content = "";
                    TaskDescriotion.Text = "";
                    TaskDate.Text = "";
                    TaskDateTime.Text = "";
                }
            }
        }

        private void NewTask_Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var add = new TaskNew();
            if (add.ShowDialog() == true && add.NewTaskes != null)
            {
                Tasks.Add(add.NewTaskes);
                UniqueCategoriesList.Add("Все");
                string category = add.NewTaskes.category;
                var uniqueCategories = Tasks.Select(t => t.category).Distinct().ToList();
                UniqueCategoriesList = new List<string>(uniqueCategories);
                UniqueCategoriesList.Add("Все");
                Task_List.ItemsSource = Tasks;
                DataContext = this;
                Taske_List.ItemsSource = Tasks;
                DataContext = this;
                Category_List.ItemsSource = UniqueCategoriesList;
                DataContext = this;
            }
        }
    }
}

