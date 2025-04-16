using lab.DataBase;
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
        IFileRepository fileRepository;
        public MainEmpty()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Image_User1.Visibility = Visibility.Hidden;
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            fileRepository = new FileRepository();
            var tokenOfUser = BaseConnect.GetLastAddedToken();
            var name = await fileRepository.GetUser(tokenOfUser);
            Current_user.Content = name;

            var tok1 = BaseConnect.GetLastAddedToken();
            var infoUser = await fileRepository.GetImageUser(Image_User1,Image_User);
            Image_User.Source = infoUser;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Mainxaml mainxaml = new Mainxaml();
            var reptodosTask2 = mainxaml.ReturnFalseTodos();
            bool reptodos1 = await reptodosTask2;

            if (reptodos1 == false)
            {
                var add = new TaskNew();
                bool? result = add.ShowDialog();
            }
            if (reptodos1 == true)
            {
                Manager.MainFrame.Navigate(new Mainxaml());
            }
        }
        public async void ImageReload()
        {
        }
        private async void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
        }
        private async void Image_User1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        //изменение фото профиля
        private async void Image_User_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
