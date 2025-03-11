
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using taskLibrary;
using System.Net.Http.Json;

namespace lab
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
            return result?.data;
        }
    }
}
