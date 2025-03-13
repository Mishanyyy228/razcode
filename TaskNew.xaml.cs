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
        public TaskNew()
        {
            InitializeComponent();
            PopulateTimeComboBox();

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
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://45.144.64.179/");
            this.DialogResult = true;
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
                            try
                            {
                                if (Date_PickerBox.SelectedDate.HasValue)
                                {
                                    var selectedDate = Date_PickerBox.SelectedDate.Value.Date;
                                    var selectedTime = Cmb1.SelectedItem.ToString(); // Строка формата "HH:mm"

                                    // Преобразуем строку времени в часы и минуты
                                    var timeParts = selectedTime.Split(':');
                                    var hours = int.Parse(timeParts[0]);
                                    var minutes = int.Parse(timeParts[1]);

                                    // Создаем DateTime, объединяя дату и время
                                    var combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hours, minutes, 0);

                                    // Преобразуем DateTime в long
                                    long timestamp = combinedDateTime.ToBinary();

                                    var NewTaskes = new Todo
                                    {
                                        title = txt_name.Text,
                                        category = txt_category.Text,
                                        date = timestamp, 
                                        description = txt_opis.Text,
                                        isCompleated = false
                                    };
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                                    var responce = await client.PostAsJsonAsync("api/todos", NewTaskes);

                                    if (responce.IsSuccessStatusCode)
                                    {
                                        MessageBox.Show("Задача добавлена");


                                    }
                                    if (!responce.IsSuccessStatusCode)
                                    {
                                        MessageBox.Show($"Запрос информации о пользователе не удался. Код ошибки: {(int)responce.StatusCode}. Сообщение: {await responce.Content.ReadAsStringAsync()}");

                                    }
                                }

                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                    }


                }
                    }
                }
            }
        }
