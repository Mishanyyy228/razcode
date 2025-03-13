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





namespace lab
{
    /// <summary>
    /// Логика взаимодействия для Mainxaml.xaml
    /// </summary>
    public partial class Mainxaml : Page, INotifyPropertyChanged
    {
        public TaskRepository _repository;
        public UserRepository _repository1;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
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


        public Mainxaml(string qwe)
        {
            _repository = new TaskRepository();
            var tasks = new ObservableCollection<Todo>(TaskRepository.AllTasks);
            this.Tasks = tasks;
            DataContext = this;

            var uniqueCategories = tasks.Select(t => t.category).Distinct().ToList();

            UniqueCategoriesList = new List<string>(uniqueCategories);

            DataContext = this;
            InitializeComponent();
            
            if( qwe!= null)
            {
                UserBox.Content = qwe;
            }

            Loaded += OnLoaded;
            Taske_List.ItemsSource = Tasks;
            DataContext = this;
            //Task_List.ItemsSource = Tasks;
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
        }
        public async Task LoadTasksAsync()
        {
            var repository = new Repository1();
            var todolist = await repository.GetTodosAsync();

            // Очистите существующий список, если он есть
            //Tasks.Clear();

            // Заполните Tasks новыми элементами
            foreach (var task in todolist)
            {
                Tasks.Add(task);
            }

            // Привяжите Tasks к ItemsSource
            Task_List.ItemsSource = Tasks;
        }

        private void Task_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Formatter = new DateFormatter();
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

                    // Разбиваем на дату и время
                    string datePart = combinedDateTime.ToShortDateString();
                    string timePart = combinedDateTime.ToLongTimeString();

                    TaskDateTime.Text = datePart;
                    TaskDate.Text = timePart;         // Полная дата
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
            var Formatter = new DateFormatter();

            Todo classTask = (Todo)Taske_List.SelectedItem;
            if (classTask == null)
            {

            }
            //int indexOfSpace = classTask.date.LastIndexOf(' '); 

            if (classTask != null)
            {
                TaskDate.Text = classTask.date.ToString();
                //var firstDate = classTask.date.ToString();
                TaskDateTime.Text = classTask.date.ToString();
                TaskName.Content = classTask.title;
                TaskDescriotion.Text = classTask.description;
                gridthick1.BorderThickness = new Thickness(1);
                gridthick1.BorderBrush = Brushes.Black;
            }
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Todo classTask = (Todo)Task_List.SelectedItem;

            if (classTask != null)
            {
                classTask.isCompleated = true;
                Task_List.Items.Refresh();
                Category_List.Items.Refresh();
                Task_List.ItemsSource = Tasks;
                DataContext = this;
                MessageBox.Show("Задача выполнена!");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Todo classTask = (Todo)Task_List.SelectedItem;

            if (classTask != null)
            {
                classTask.isCompleated = true;
                Category_List.Items.Refresh();
                Task_List.Items.Refresh();
                MessageBox.Show("Задача удалена");
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

