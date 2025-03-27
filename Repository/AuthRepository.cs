using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoEntities;

namespace lab.Repository
{
    public class AuthRepository : TodoHttpClient, IAuthRepository
    {
        private readonly HttpClient httpClient;
        public AuthRepository()
        {
            httpClient = GetHttpClient();
        }
        public async Task<string> LoginAndGetUser(Usermodel user)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/auth/login", user);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка авторизации: {response.StatusCode}");
                }

                TokenStorage.Value = response.Content.ReadAsAsync<Responce<Token>>().Result.data.access_token;
                var token = new Token(TokenStorage.Value);
                BaseConnect.SaveToken(token);
                if (TokenStorage.Value == null)
                {
                    throw new InvalidOperationException("Не удалось получить токен.");
                }
                var tok1 = BaseConnect.GetLastAddedToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tok1);

                var result = await httpClient.GetAsync("api/user");
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка получения данных пользователя: {result.StatusCode}");
                }
                var userInfo = await result.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(userInfo);
                string name = (string)jObject["data"]["name"];
                if (name == null)
                {
                    throw new Exception($"Ошибка получения данных пользователя");
                }
                return name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> RegistrUser(Usermodel user)
        {
            try
            {
                var responce = await httpClient.PostAsJsonAsync("api/auth/registration", user);

                if (!responce.IsSuccessStatusCode)
                {
                    MessageBox.Show("Email уже занят. Пожалуйста, выберите другой.");
                }
                return "Успешно!Добро пожаловать!";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
