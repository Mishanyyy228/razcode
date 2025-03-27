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

namespace lab
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        IAuthRepository authRepository;
        public Registration()
        {
            InitializeComponent();
            authRepository = new AuthRepository();
        }
        private void Name_user_GotFocus_1(object sender, RoutedEventArgs e)
        {
            if (Name_user.Text == "Введите имя пользователя")
            {
                Name_user.Text = string.Empty;
                Name_user.Foreground = Brushes.Black;
            }
        }

        private void Email_user_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Email_user.Text == "Введите почту")
            {
                Email_user.Text = string.Empty;
                Email_user.Foreground = Brushes.Black;
            }
        }

        private void Pass_user_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_user.Text == "Введите пароль")
            {
                Pass_user.Text = string.Empty;
                Pass_user.Foreground = Brushes.Black;
            }
        }

        private void Pass_user1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_user1.Text == "Повторите пароль")
            {
                Pass_user1.Text = string.Empty;
                Pass_user1.Foreground = Brushes.Black;
            }
        }

        private void Name_user_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Name_user.Text == "")
            {
                Name_user.Text = "Введите имя пользователя";
                Name_user.Foreground = Brushes.Gray;
            }
        }

        private void Email_user_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Email_user.Text == "")
            {
                Email_user.Text = "Введите почту";
                Email_user.Foreground = Brushes.Gray;
            }
        }

        private void Pass_user_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_user.Text == "")
            {
                Pass_user.Text = "Введите пароль";
                Pass_user.Foreground = Brushes.Gray;
            }
        }

        private void Pass_user1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_user1.Text == "")
            {
                Pass_user1.Text = "Повторите пароль";
                Pass_user1.Foreground = Brushes.Gray;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new LogIn());
        }

        private async void  Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Name_user.Text != "Введите имя пользователя")
            {
                if (Email_user.Text != "Введите почту")
                {
                    if (Pass_user.Text != "Введите пароль")
                    {
                        string email = Email_user.Text;
                        string password = Pass_user.Text;
                        string name = Name_user.Text;
                        string password1 = Pass_user1.Text;

                        bool isEmailValid = email.ValidateEmail();
                        bool isPasswordValid = password.ValidatePassword();
                        bool isNameValid = name.ValidateName();
                        bool isPasswordValid1 = password1.ValidatePassword();

                        if (!isEmailValid | !isPasswordValid | !isNameValid)
                        {

                            if (!isEmailValid)
                            {
                                MessageBox.Show("Некорректный формат почты.");
                            }
                            if (!isPasswordValid1)
                            {
                                MessageBox.Show("Пароль должен быть не менее 6 символов.");
                            }
                            if (!isNameValid)
                            {
                                MessageBox.Show("Имя должно содержать не менее 3 символов.");
                            }
                            if (Pass_user.Text != Pass_user1.Text)
                            {
                                MessageBox.Show("Пароли должны совпадать");
                            }
                        }

                        if (Pass_user.Text == Pass_user1.Text)
                        {
                            var user = new Usermodel
                            {
                              Email = Email_user.Text,
                              Password = Pass_user.Text,
                              Name = Name_user.Text
                            };
                            var infoUser = await authRepository.RegistrUser(user);
                            MessageBox.Show(infoUser);
                        }
                        else
                        {
                            MessageBox.Show("Пароли должны совпадать");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле пароль пустое");
                    }
                }
                else
                {
                    MessageBox.Show("Поле почта пустое");
                }
            }
            else
            {
                MessageBox.Show("Поле имя пустое");
            }
        }
    }
}
