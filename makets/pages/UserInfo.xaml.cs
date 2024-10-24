using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        private int userId;
        private readonly HttpClient _httpClient;
        public UserInfo(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(userId == 0) {return;}

            if(TextOnImage == null) {return;}

            String description = TextOnImage.Text.Trim();

            var request = new
            {
                UserId = userId,
                Description = description
            };
            // Отправляем POST запрос на сервер для сохранения цели
            var response = await _httpClient.PostAsJsonAsync("Autho/saveUserDescription", request);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(resultContent);

                // Проверяем, что сообщение от сервера содержит "Описание пользователя сохранено."
                if (result?.message == "Описание пользователя сохранено.")
                {
                    UserProfile userProfile = new UserProfile(userId);
                    userProfile.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось сохранить теги пользователя. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка при сохранении цели: {errorMessage}");
            }
           
        }
    }
}
