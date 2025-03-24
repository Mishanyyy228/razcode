using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace lab.Repository
{
    public class FileRepository : TodoHttpClient
    {
        private readonly HttpClient httpClient;
        public FileRepository()
        {
            httpClient = GetHttpClient();
        }

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

        public async Task<string> GetUser(string user)
        {
            if (user != null)
            {
                var token = user.ToString();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var result = await httpClient.GetAsync("api/user");
                if (result.IsSuccessStatusCode)
                {
                    var userInfo = await result.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(userInfo);
                    string name = (string)jObject["data"]["name"];
                    if (name == null)
                    {
                        throw new Exception($"Ошибка получения данных пользователя");
                    }
                    return name;
                }
            }
            if (user == null)
            {
                return null;
            }
            return null;
        }

        public async Task<BitmapImage> PostAndGetImageUser(Image Image_User1, Image Image_User)
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
                            MessageBox.Show("Фото успешно загружено!");
                            return await GetImageUser(Image_User1, Image_User);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при загрузке фото");
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
    }
}
