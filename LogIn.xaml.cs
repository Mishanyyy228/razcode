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

namespace lab
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window w2 = new Registration();
            Hide();
            w2.Show();
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Pochta_user11.Text != "student12@gmail.com")
            {
                if (Password_user11.Text != "Введите пароль")
                {
                    string email = Pochta_user11.Text;
                    string password = Password_user11.Text;

                    bool isEmailValid = email.ValidateEmail();
                    bool isPasswordValid = password.ValidatePassword();

                    if (!isEmailValid)
                    {
                        MessageBox.Show("Некорректный формат почты.");
                    }
                    if (!isPasswordValid)
                    {
                        MessageBox.Show("Пароль должен быть не менее 6 символов.");
                    }
                    if (isEmailValid | isPasswordValid)
                    {
                        Window w2 = new MainEmpty();
                        Hide();
                        w2.Show();
                    }
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
