
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using taskLibrary;
using System.Net.Http.Json;
using TodoEntities;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

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
            return result?.data;
        }
        //авторизация
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
                if (TokenStorage.Value == null)
                {
                    throw new InvalidOperationException("Не удалось получить токен.");
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

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
        //регистрация
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
        //получение картинки
        public async Task<BitmapImage> GetImageUser(Image Image_User1, Image Image_User)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

            try
            {
                var result = await httpClient.GetAsync("api/user");
                var userInfo = await result.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(userInfo);
                string name = (string)jObject["data"]["imageId"];

                if (name == "000000000000000000000000")
                {
                    Image_User1.Visibility = Visibility.Visible;
                    Image_User.Visibility = Visibility.Hidden;
                    return null; 
                }
                else
                {
                    var response = await httpClient.GetAsync($"api/user/photo/{name}");

                    if (response.IsSuccessStatusCode)
                    {
                        Image_User1.Visibility = Visibility.Hidden;
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                        using (var stream = new MemoryStream(imageBytes))
                        {
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = stream;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            return bitmapImage;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка загрузки изображения.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return null; 
            }
        }
        //mananev13@gmail.com
        //установление картинки
        public async Task<BitmapImage> PostAndGetImageUser(Image Image_User1,Image Image_User)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Images (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    byte[] imageData = File.ReadAllBytes(filePath);

                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(new ByteArrayContent(imageData), "uploadedFile", System.IO.Path.GetFileName(filePath));

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                        var response = await httpClient.PostAsync("api/user/photo", formData);

                        if (response.IsSuccessStatusCode)
                        {
                            //Image_User1.Visibility = Visibility.Hidden;
                            MessageBox.Show("Фото успешно загружено!");
                            return await GetImageUser(Image_User1, Image_User);
                        }
                        else
                        {
                            string errorMessage = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Ошибка при загрузке фото: {errorMessage}. Код ответа: {response.StatusCode}");
                            return null; 
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                    return null; 
                }
            }
            return null;
        }
        //добавление задачи
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
                    var errorText = $"Запрос информации о пользователе не удался. Код ошибки: {(int)responce.StatusCode}. Сообщение: {await responce.Content.ReadAsStringAsync()}";
                    return errorText;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        //удаление задачи
        public async Task<string> DeleteTodos(Todo todo, ListBox Task_List)
        {
            todo = (Todo)Task_List.SelectedItem;
            if (todo != null)
            {
                var taskId = todo.id;
                var content = new StringContent(taskId, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorage.Value);

                var result = await httpClient.DeleteAsync($"/api/todos/{taskId}");
                if (result.IsSuccessStatusCode)
                {
                    var deleteString = "Задача удалена";
                    return deleteString;
                }
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Задача не удалена. Возникла ошибка: {(int)result.StatusCode}. Сообщение: {await result.Content.ReadAsStringAsync()}");
                }
            }
            return null;
        }

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
                    MessageBox.Show($"Задача не выполнена. Возникла ошибка: {(int)result.StatusCode}. Сообщение: {await result.Content.ReadAsStringAsync()}");
                }
            }
            return null;
        }
    }
}
