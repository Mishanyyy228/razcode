using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace lab.DataBase
{
    public static class DataBaseService
    {
        public static void SaveToken(Token token)
        {
            using (var context = new ApplicationContext())
            {
                try
                {
                    if (!context.Token.Any(t => t.access_token == token.access_token))
                    {
                        context.Token.Add(token);
                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Токен уже существует в базе данных.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении токена: {ex.Message}");
                }
            }
        }

        // Метод удаления последнего токена переписываем иначе,
        // так как нет уникального идентификатора для сортировки
        public static void DeleteLastAddedToken()
        {
            using (var context = new ApplicationContext())
            {
                try
                {
                    var anyToken = context.Token.FirstOrDefault();
                    if (anyToken != null)
                    {
                        context.Token.Remove(anyToken);
                        context.SaveChanges();
                        MessageBox.Show("Один из токенов удалён успешно.");
                    }
                    else
                    {
                        MessageBox.Show("Нет токенов для удаления.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении токена: {ex.Message}");
                }
            }
        }

        // Возврат последнего токена тоже переделываем
        // Вернуть последний добавленный токен
        public static string GetLastAddedToken()
        {
            using (var context = new ApplicationContext())
            {
                try
                {
                    // Сначала проверяем, есть ли токены в базе данных
                    if (context.Token.Any())
                    {
                        // Если есть, выберем самый свежий токен
                        var lastToken = context.Token.Select(t => t.access_token).FirstOrDefault();
                        return lastToken;
                    }
                    else
                    {
                        // Если токенов нет, выводим сообщение и возвращаем null
                        MessageBox.Show("Нет токенов в базе данных.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении токена: {ex.Message}");
                    return null;
                }
            }
        }
    }
}