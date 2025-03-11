using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab
{
    public class DateFormatter
    {
        public int FormatDate(string inputDate)
        {
            try
            {
                // Парсим входную строку в объект DateTime
                DateTime date = DateTime.ParseExact(inputDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                // Преобразуем дату в числовой формат
                return date.Year * 10000 + date.Month * 100 + date.Day;
            }
            catch (FormatException ex)
            {
               MessageBox.Show($"Ошибка формата: {ex.Message}");
                return 0;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"Передана пустая строка: {ex.Message}");
                return 0;
            }
        }
    }
}
