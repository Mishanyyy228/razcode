using lab.model;
using lab.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lab
{
    /// <summary>
    /// Логика взаимодействия для MainEmpty.xaml
    /// </summary>
    public partial class MainEmpty : Page
    {
        public MainEmpty(string nameUser,string Token1)
        {
            var currentUser = nameUser;
            InitializeComponent();
            if (nameUser != null)
            {
                Current_user.Content = currentUser;
            }
            Loaded += OnLoaded;
            Image_User1.Visibility = Visibility.Hidden;
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var repository = new FileRepository();
            var tok1 = BaseConnect.GetLastAddedToken();
            var rep1 = await repository.GetUser(tok1);
            var infoUser = await repository.GetImageUser(Image_User1,Image_User);
            Image_User.Source = infoUser;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nameUser = Current_user.Content;
            var currentUser = nameUser.ToString();
            Manager.MainFrame.Navigate(new Mainxaml(currentUser));
        }
        public async void ImageReload()
        {
        }
        private async void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { }
        private async void Image_User1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        //изменение фото профиля
        private async void Image_User_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
