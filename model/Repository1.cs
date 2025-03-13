
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using taskLibrary;
using System.Net.Http.Json;

namespace lab.model
{
    public class Repository1 : TodoHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly string TodosUrl = "api/todos";
        public Repository1()
        {
            httpClient = GetHttpClient();
        }
        public async Task<List<Todo>?> GetTodosAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);
            var result = await httpClient.GetFromJsonAsync<Responce<List<Todo>>>(TodosUrl);
            //var todos = result.data;

            //var todo = new List<Todo>();
            //foreach(var tod in todos)
            //{
            //    todo.Add(TodoModel.Map(tod));
            //}
            return result?.data;
        }
    }
}
