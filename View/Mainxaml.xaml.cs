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
        public ObservableCollection<ClassTask> Tasks { get; set; }

        private UserRepository _userRepository;

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

        public event PropertyChangedEventHandler PropertyChangeded;

        public Mainxaml(string qwe,string eqrwer)
        {

            _repository = new TaskRepository();
            var tasks = new ObservableCollection<ClassTask>(TaskRepository.AllTasks);
            this.Tasks = tasks;
            DataContext = this;

            // Получение уникальных категорий
            var uniqueCategories = tasks.Select(t => t.Category).Distinct().ToList();

            // Сохранение уникальных категорий в отдельный список
            UniqueCategoriesList = new List<string>(uniqueCategories);
            // Остальные настройки контекста данных
            DataContext = this;
            InitializeComponent();
            
            if (qwe != null & eqrwer!=null)
            {
                var qazwsx = new UserRepository();
                var qwert = qazwsx.GetUser(qwe,eqrwer);
                UserBox.Content = qwert.Username;
                //UserBox.Content = user.Username;

                //var qwerty = _repository1.GetUser(qwer, qwera).Username;
                //UserBox.Content = qwerty;
            }

            Taske_List.ItemsSource = Tasks;
            DataContext = this;
            Task_List.ItemsSource = Tasks;
            DataContext = this;
            Category_List.ItemsSource = UniqueCategoriesList;
            DataContext = this;

            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            Taske_List.Visibility = Visibility.Hidden;
            //_repository1 = repository1;
        }

        private void Task_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskName.Content = string.Empty;
            TaskDescriotion.Text = "";
            TaskDate.Text = string.Empty;
            TaskDateTime.Text = string.Empty;
            ClassTask classTask = (ClassTask)Task_List.SelectedItem;
            if (classTask == null)
            {

            }
            if (classTask != null)
            {
                TaskName.Content = classTask.Name;
                TaskDescriotion.Text = classTask.Description;
                TaskDate.Text = classTask.Date.ToString("HH:mm");
                TaskDateTime.Text = classTask.DateAndTime.ToString("dd MMMMMMMMMM yyyy");
                gridthick.BorderThickness = new Thickness(1);
                gridthick.BorderBrush = Brushes.Black;
            }
            Buttone_Delete.Visibility = Visibility.Visible;
            Buttone_Gotovo.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClassTask classTask = (ClassTask)Task_List.SelectedItem;

            if (classTask != null)
            {
                classTask.IsCompleted = true;
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
            ClassTask classTask = (ClassTask)Task_List.SelectedItem;

            if (classTask != null)
            {
                classTask.IsCompleted = true;
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

        private void AddTaskButton_Click_2(object sender, RoutedEventArgs e)
        {

            var add = new NewTask();
            if (add.ShowDialog() == true && add.NewTaskes != null) 
            {
                Tasks.Add(add.NewTaskes); 
                string category = add.NewTaskes.Category;
                var uniqueCategories = Tasks.Select(t => t.Category).Distinct().ToList();
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            TaskName.Content = "";
            TaskDescriotion.Text = "";
            TaskDate.Text = "";
            TaskDateTime.Text = "";
            AddTaskButton.Visibility = Visibility.Hidden;
            Task_List.Visibility = Visibility.Hidden;
            Taske_List.Visibility = Visibility.Visible;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            gridthick.Visibility = Visibility.Hidden;
        }
        private void Taske_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClassTask classTask = (ClassTask)Taske_List.SelectedItem;
            if (classTask == null)
            {

            }
            if (classTask != null)
            {
                TaskName.Content = classTask.Name;
                TaskDescriotion.Text = classTask.Description;
                TaskDate.Text = classTask.Date.ToString("HH:mm");
                TaskDateTime.Text = classTask.DateAndTime.ToString("dd MMMMMMMMMM yyyy");
                gridthick1.BorderThickness = new Thickness(1);
                gridthick1.BorderBrush = Brushes.Black;
            }
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            gridthick1.Visibility = Visibility.Hidden;
            Taske_List.Visibility = Visibility.Hidden;
            Task_List.Visibility = Visibility.Visible;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;
            AddTaskButton.Visibility = Visibility.Visible;
            TaskName.Content = "";
            TaskDescriotion.Text = "";
            TaskDate.Text = "";
            TaskDateTime.Text = "";
        }

        private void Category_List_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string selectedCity = Category_List.SelectedItem as string;
            var filteredPeople = Tasks.Where(p => p.Category == selectedCity).ToList();
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
                    // Показываем всех людей
                    Task_List.ItemsSource = Tasks;
                    Taske_List.ItemsSource = Tasks;
                }
                if (filteredPeople == null)
                {
                    var filteredPeoples = Tasks.Where(p => p.Category == selectedCity);
                    Task_List.ItemsSource = filteredPeoples.ToList();
                    Taske_List.ItemsSource = filteredPeoples.ToList();
                    TaskName.Content = "";
                    TaskDescriotion.Text = "";
                    TaskDate.Text = "";
                    TaskDateTime.Text = "";
                }
            }
        }


    }
}

