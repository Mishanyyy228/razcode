using lab.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
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
using lab.Repository;
using System.Net.Http;
using TodoEntities;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using lab.model;
using System.Xml.Linq;


namespace lab
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Registration());
        }

        private void Password_user11_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password_user11.Text == "Введите пароль")
            {
                Password_user11.Text = string.Empty;
                Password_user11.Foreground = Brushes.Black;
            }
        }

        private void Password_user11_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password_user11.Text == "")
            {
                Password_user11.Text = "Введите пароль";
                Password_user11.Foreground = Brushes.Gray;
            }
        }

        private void Pochta_user11_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Pochta_user11.Text == "student12@gmail.com")
            {
                Pochta_user11.Text = string.Empty;
                Pochta_user11.Foreground = Brushes.Black;
            }
        }

        private void Pochta_user11_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Pochta_user11.Text == "")
            {
                Pochta_user11.Text = "student12@gmail.com";
                Pochta_user11.Foreground = Brushes.Black;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = new Usermodel
            {
                Email = Pochta_user11.Text,
                Password = Password_user11.Text
            };
            if (Pochta_user11.Text != "student12@gmail.com")
            {
                if (Password_user11.Text != "Введите пароль")
                {
                    string email = Pochta_user11.Text;
                    string password = Password_user11.Text;

                    bool isEmailValid = email.ValidateEmail();
                    bool isPasswordValid = password.ValidatePassword();

                    if (!isEmailValid & !isPasswordValid)
                    {
                        if (!isEmailValid)
                        {
                            MessageBox.Show("Некорректный формат почты.");
                        }
                        if (!isPasswordValid)
                        {
                            MessageBox.Show("Пароль должен быть не менее 6 символов.");
                        }
                    }
                    if (isEmailValid && isPasswordValid)
                    {
                        var repository = new AuthRepository();
                        var repository1 = new TodoRepository();
                        var infoUser = await repository.LoginAndGetUser(user);
                        var todosUser = await repository1.GetTodosAsync();
                        var Todos = todosUser.Count();
                        var token = new Token(TokenStorage.Value);
                        if(Todos==0)
                        {
                            //BaseConnect.SaveToken(token); // Вызываем метод статически
                            MessageBox.Show($"Вход выполнен успешно! Приветствуем вас, {infoUser}!", "Успех", MessageBoxButton.OK);
                            Manager.MainFrame.Navigate(new MainEmpty(infoUser));
                        }
                        else
                        {
                            MessageBox.Show($"Вход выполнен успешно! С возвращением, {infoUser}!", "Успех", MessageBoxButton.OK);
                            //BaseConnect.SaveToken(token); // Вызываем метод статически
                            Manager.MainFrame.Navigate(new Mainxaml(infoUser));
                        }
                    }
                    //kmlkmmlklm@hgv.seg
                }
                else
                {
                    MessageBox.Show("Поле пароль пустое!");
                }
            }
            else
            {
                MessageBox.Show("Поле почта пустое!");
            }
        }
    }
}

