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
    public partial class Mainxaml : Window
    {
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

        public ObservableCollection<ClassTask> Tasks { get; set; }
        private UserRepository _userRepository;

        public Mainxaml()
        {
            InitializeComponent();
            DataContext = this;
            if (UserRepository.CurrentUser != null)
            {
                Username = UserRepository.CurrentUser.Username;
            }

            Tasks = new ObservableCollection<ClassTask>
            {
            };
            Task_List.ItemsSource = Tasks;
            DataContext = this;
            Buttone_Delete.Visibility = Visibility.Hidden;
            Buttone_Gotovo.Visibility = Visibility.Hidden;

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Task_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClassTask classTask = (ClassTask)Task_List.SelectedItem;
            TaskName.Content = classTask.Name;
            TaskDescriotion.Text = classTask.Description;
            TaskDate.Text = classTask.Date.ToString("HH:mm");
            //TaskDateTime.Text = classTask.DateTime.ToString("dd MMMMMMMMMM yyyy");
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
                MessageBox.Show("Задача выполнена!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClassTask classTask = (ClassTask)Task_List.SelectedItem;

            if (classTask != null)
            {
                classTask.IsCompleted = true;
                Task_List.Items.Refresh();
                MessageBox.Show("Задача удалена");
            }
            //if (classTask != null)
            //{
            //    Task_List.Items.Remove(classTask);
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Task_List_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void ListBoxItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        private void House_Click(object sender, EventArgs e)
        {

        }

        private void Work_Selected(object sender, RoutedEventArgs e)
        {

        }
    }

}

