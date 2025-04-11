using lab.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using System.Xml.Linq;
using taskLibrary;

namespace lab
{
    /// <summary>
    /// Логика взаимодействия для TaskNew.xaml
    /// </summary>
    public partial class TaskNew : Window
    {
        ITodoRepository todoRepository;
        public TaskNew()
        {
            InitializeComponent();
            PopulateTimeComboBox();
            todoRepository = new TodoRepository();
        }
        private void PopulateTimeComboBox()
        {
            for (int hour = 0; hour <= 23; hour++)
            {
                for (int minute = 0; minute <= 59; minute += 15)
                {
                    string time = $"{hour:D2}:{minute:D2}";
                    Cmb1.Items.Add(time);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public Todo NewTaskes { get; private set; }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            {
                if (Date_PickerBox.SelectedDate.HasValue != null | Cmb1.SelectedItem != null | txt_name.Text.Length <= 20 | txt_category.Text.Length <= 10)
                {
                    if (txt_category.Text.Length >= 15)
                    {
                        MessageBox.Show("Название категории слишком длинное");
                    }
                    else
                    {
                        if (txt_name.Text.Length >= 20)
                        {
                            MessageBox.Show("Название задачи слишком длинное");
                        }
                        else
                        {
                            if (Date_PickerBox.SelectedDate.HasValue)
                            {
                                var selectedDate = Date_PickerBox.SelectedDate.Value.Date;
                                var selectedTime = Cmb1.SelectedItem.ToString();

                                var timeParts = selectedTime.Split(':');
                                var hours = int.Parse(timeParts[0]);
                                var minutes = int.Parse(timeParts[1]);

                                var combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hours, minutes, 0);

                                long timestamp = combinedDateTime.ToBinary();

                                var NewTaskes = new Todo
                                {
                                    title = txt_name.Text,
                                    category = txt_category.Text,
                                    date = timestamp,
                                    description = txt_opis.Text,
                                    isCompleted = false
                                };
                                var infoUser = await todoRepository.NewTodos(NewTaskes);
                                MessageBox.Show(infoUser);
                                this.DialogResult = true; // Указываем, что диалог был успешно закрыт
                                this.Close(); // Закрываем окно                            }
                            }
                        }
                    }
                }
            }
        }
    }
}
