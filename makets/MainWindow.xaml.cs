using makets.pages;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace makets
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
        }
        /*
            В разметке нужно поставить на пароль PasswordBox 
         */

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем логин и пароль из текстовых полей
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            // Формируем запрос
            var request = new
            {
                Login = login,
                Password = password
            };

            // Отправляем POST запрос на сервер для авторизации
            var response = await _httpClient.PostAsJsonAsync("Authorization/authorization", request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<dynamic>();
                MessageBox.Show("Авторизация успешна!");
                int userId = responseData.userId;
                if (userId.Equals(null))
                {
                    UserProfile window = new UserProfile(userId);
                    window.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось авторизоваться. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка авторизации: {errorMessage}");
            }
        }

        private void ForgotPwdBtn_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
