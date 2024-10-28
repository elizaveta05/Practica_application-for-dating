using Newtonsoft.Json;
using makets.Model.Model_users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace makets.pages.Profile
{
    /// <summary>
    /// Логика взаимодействия для EditUserInfo.xaml
    /// </summary>
    public partial class EditUserInfo : Window
    {
        private readonly HttpClient _httpClient;
        private int userId;
        public EditUserInfo(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
            LoadData(userId);
        }

        private async Task LoadData(int userId)
        {
            // Конвертируем данные перед отправкой
            var json = JsonConvert.SerializeObject(userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Отправляем на сервер
            var response = await _httpClient.PostAsync("Profile/getUserDescription", content);

            if (response.IsSuccessStatusCode)
            {
                // Чтение и парсинг ответа
                var resultContent = await response.Content.ReadAsStringAsync();
                var userDescription = JsonConvert.DeserializeObject<UserDescription>(resultContent);

                // Заполнение TextBox данными
                TextOnImage.Text = userDescription?.Description ?? "Нет информации о пользователе.";
            }
            else
            {
                MessageBox.Show("Ошибка загрузки данных о пользователе.");
            }
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e) 
        {
            string newDescription = TextOnImage.Text; // Получаем новое описание из TextBox

            if (string.IsNullOrWhiteSpace(newDescription))
            {
                MessageBox.Show("Описание не может быть пустым.");
                return;
            }

            var request = new UserDescription
            {
                UserId = userId,
                Description = newDescription
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Profile/updateUserDescription", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Описание пользователя успешно обновлено.");
                UserProfile userProfile = new UserProfile(userId);
                userProfile.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении описания.");
            }
        }

    }
}
