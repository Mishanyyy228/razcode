using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab
{
    public static class BaseConnect
    {
        private const string ConnectionString =
            @"Data Source=MISHANYYY228\SQLEXPRESS; Initial Catalog=ResponceToken; User ID=sa; Password=Qwerty0690;";

        public static void SaveToken(Token token)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Проверка наличия таблицы TokeenTable
                    CheckTableExists(connection);

                    if (!TokenAlreadyExists(connection, token.access_token))
                    {
                        // Запрос на вставку нового токена
                        string insertQuery = "INSERT INTO TokenTable (Token) VALUES (@Token)";
                        using (var command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Token", token.access_token);
                            command.ExecuteNonQuery();
                            //MessageBox.Show("Токен успешно сохранён.");
                        }
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
                finally
                {
                    connection.Close();
                }
            }
        }
        //проверка на повтор
        private static bool TokenAlreadyExists(SqlConnection connection, string token)
        {
            string selectQuery = "SELECT COUNT(*) FROM TokenTable WHERE Token = @token";
            using (var command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@token", token);
                int count = (int)command.ExecuteScalar();
                return count>0;
            }
        }
        //удаление токена из БД
        public static void DeleteLastAddedToken()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    CheckTableExists(connection);

                    string deleteQuery = "DELETE FROM TokenTable WHERE Id_Token = (SELECT MAX(Id_Token) FROM TokenTable)";
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Последний добавленный токен успешно удалён.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении токена: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        //проверка наличия таблицы для токена
        private static void CheckTableExists(SqlConnection connection)
        {
            string checkTableQuery =
                "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TokenTable]') AND type in (N'U')) " +
                "BEGIN CREATE TABLE TokenTable(Id INT IDENTITY(1,1) PRIMARY KEY, Token VARCHAR(MAX)); END;";
            using (var command = new SqlCommand(checkTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static string GetLastAddedToken()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Проверка наличия таблицы TokeenTable
                    CheckTableExists(connection);

                    // Получение последнего добавленного токена
                    var selectQuery = "SELECT TOP 1 Token FROM TokenTable ORDER BY Id_Token DESC";
                    using (var command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Если токен найден
                            {
                                // Читаем значение токена из первого столбца
                                string tokenValue = reader.GetString(0); // или reader["Token"]

                                // Возвращаем строку с токеном
                                return tokenValue;
                            }
                        }
                    }

                    // Если токен не найден, возвращаем null
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при получении токена: {ex.Message}");
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}
