using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    public class DateFormatter
    {
        public string FormatDate(string inputDate)
        {
            try
            {
                // Парсим входную строку в объект DateTime
                DateTime date = DateTime.ParseExact(inputDate, "dd.MM.yyyy", null);

                // Преобразуем дату в нужный формат
                return date.ToString("d MMMM yyyy");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка формата: {ex.Message}");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Передана пустая строка: {ex.Message}");
                return null;
            }
        }
    }
}
