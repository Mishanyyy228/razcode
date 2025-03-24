using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using taskLibrary;

namespace lab.Repository
{
    public class TodoRepository: TodoHttpClient, ITodoRepository
    {
        private readonly HttpClient httpClient;
        private readonly string TodosUrl = "api/todos";

        public TodoRepository()
        {
            httpClient = GetHttpClient();
        }

        public async Task<List<Todo>?> GetTodosAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);
            var result = await httpClient.GetFromJsonAsync<Responce<List<Todo>>>(TodosUrl);
            return result?.data;
        }
        //3
        

        public async Task<string> NewTodos(Todo todo)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);
                var responce = await httpClient.PostAsJsonAsync("api/todos", todo);
                if (responce.IsSuccessStatusCode)
                {
                    var text = "Задача добавлена";
                    return text;
                }
                if (!responce.IsSuccessStatusCode)
                {
                    var errorText = "Запрос информации о пользователе не удался.";
                    return errorText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        //2
        //удаление задачи
        public async Task<string> DeleteTodos(Todo todo, ListBox Task_List)
        {
            todo = (Todo)Task_List.SelectedItem;
            if (todo != null)
            {
                var taskId = todo.id;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                var result = await httpClient.DeleteAsync($"/api/todos/{taskId}");
                if (result.IsSuccessStatusCode)
                {
                    var deleteString = "Задача удалена";
                    return deleteString;
                }
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Задача не удалена.");
                }
            }
            return null;
        }
        //2
        //присвоение задаче статуса готовности
        public async Task<string> TodosIsReady(Todo todo, ListBox Task_List)
        {
            todo = (Todo)Task_List.SelectedItem;

            if (todo != null)
            {
                var taskId = todo.id;
                var content = new StringContent(taskId, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                var result = await httpClient.PutAsync($"api/todos/mark/{taskId}", content);
                if (result.IsSuccessStatusCode)
                {
                    var stringMessage = "Задача выполнена";
                    return stringMessage;
                }
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Задача не выполнена.");
                }
            }
            return null;
        }
    }
}
