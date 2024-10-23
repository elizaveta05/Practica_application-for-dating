using Server.Model.Model_users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private readonly HttpClient _httpClient;

        public Registration()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_login.Text) || string.IsNullOrEmpty(PassB1.Password) || string.IsNullOrEmpty(PassB2.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PassB1.Password != PassB2.Password)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = new UserDataRegister
            {
                Login = tb_login.Text,
                Password = PassB1.Password
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Autho/register", content);
            
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                // Переход на страницу приветствия
                WelcomePagexaml welcomWindow = new WelcomePagexaml();
                welcomWindow.Show();
                this.Close();
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
