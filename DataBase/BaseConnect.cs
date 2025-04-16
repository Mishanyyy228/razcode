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
    public static class BaseConnect
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
                    // Берём произвольный токен из списка
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
        public static string GetLastAddedToken()
        {
            using (var context = new ApplicationContext())
            {
                try
                {
                    var randomToken = context.Token.Select(t => t.access_token).FirstOrDefault();
                    return randomToken;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении токена: {ex.Message}");
                    return null;
                }
            }
        }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Token> Token => Set<Token>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }
}