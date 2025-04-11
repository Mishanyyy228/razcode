using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab.DataBase
{
    public static class BaseConnect
    {
        public static void SaveToken(Token token)
        {
            using (var context = new ResponceTokenContext())
            {
                try
                {
                    // Проверим, есть ли такой токен в базе данных
                    if (!context.Token.Any(t => t.access_token == token.access_token))
                    {
                        // Добавление нового токена
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

        public static void DeleteLastAddedToken()
        {
            using (var context = new ResponceTokenContext())
            {
                try
                {
                    var lastToken = context.Token.OrderByDescending(t => t.id).FirstOrDefault();
                    if (lastToken != null)
                    {
                        context.Token.Remove(lastToken);
                        context.SaveChanges();
                        MessageBox.Show("Последний токен удалён успешно.");
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

        public static string GetLastAddedToken()
        {
            using (var context = new ResponceTokenContext())
            {
                try
                {
                    var lastToken = context.Token.OrderByDescending(t => t.id).Select(t => t.access_token).FirstOrDefault();
                    return lastToken;
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

