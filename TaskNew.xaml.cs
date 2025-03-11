using lab.Repository;
using System;
using System.Collections.Generic;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            var userRepo1 = new TaskRepository();
            {
                if (Date_PickerBox.SelectedDate.HasValue!=null | Cmb1.SelectedItem != null | txt_name.Text.Length<=20 | txt_category.Text.Length <= 10)
                {
                    if(Cmb1.SelectedItem != null)
                    {
                        try
                        {
                            string selectedDateAsString = Date_PickerBox.SelectedDate.Value.ToShortDateString();
                            if (selectedDateAsString == null)
                            {
                                MessageBox.Show("Выберите дату.");
                            }
                        }
                        catch(Exception ex)
                        {
                            if(Date_PickerBox.SelectedDate.Value.ToShortDateString()==null)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        if (Cmb1.SelectedItem == null)
                        {
                            MessageBox.Show("Выберите время.");
                        }
                        else
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
                                    //if (Date_PickerBox.SelectedDate.HasValue)
                                    //{
                                    //    DateTime selectedDate = Date_PickerBox.SelectedDate.Value;

                                    //    // Преобразуем дату в числовой формат YYYYMMDD
                                    //    int numericDate = selectedDate.Year * 10000 + selectedDate.Month * 100 + selectedDate.Day;

                                    //    // Выводим числовую дату
                                    //    Console.WriteLine(numericDate);
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("Выберите дату.");
                                    //}
                                    int selectedItemValue = int.Parse(Cmb1.SelectedItem.ToString());

                                    // Комбинируем дату и выбранный элемент
                                    int combinedText = Date_PickerBox.SelectedDate.Value.Year * 10000 +
                                                       Date_PickerBox.SelectedDate.Value.Month * 100 +
                                                       Date_PickerBox.SelectedDate.Value.Day +
                                                       selectedItemValue;

                                    //int combinedText = $"{Date_PickerBox.SelectedDate.Value.ToShortDateString()} {Cmb1.SelectedItem}";
                                    NewTaskes = new Todo
                                    {
                                        title = txt_name.Text,
                                        category = txt_category.Text,
                                        date = combinedText,
                                        description = txt_opis.Text,
                                        isCompleated = false,
                                    };

                                    bool registr = userRepo1.AddTask(txt_name.Text, txt_opis.Text, txt_category.Text, combinedText);

                                    if (registr)
                                    {
                                        MessageBox.Show("Задача добавлена");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}