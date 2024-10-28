using makets.Model.Model_users;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для UserPurposes.xaml
    /// </summary>
    public partial class UserPurposes : Window
    {
        private int userId;
        private readonly HttpClient _httpClient;

        public UserPurposes(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton selectedToggleButton = sender as ToggleButton;

            // Снимаем выбор с остальных кнопок
            var parent = VisualTreeHelper.GetParent(selectedToggleButton);
            while (parent != null && !(parent is StackPanel))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is StackPanel stackPanel)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is ToggleButton toggleButton && toggleButton != selectedToggleButton)
                    {
                        toggleButton.IsChecked = false;
                    }
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Находим выбранную цель
            int selectedGoalId = 0;

            // Ищем ToggleButton в StackPanel
            foreach (var child in ((StackPanel)FindName("ToggleButtonsPanel")).Children)
            {
                if (child is ToggleButton toggleButton && toggleButton.IsChecked == true)
                {
                    selectedGoalId = int.Parse(toggleButton.Tag.ToString());
                    break;
                }
            }

            if (selectedGoalId == 0)
            {
                MessageBox.Show("Пожалуйста, выберите цель.");
                return;
            }

            // Формируем данные для запроса
            var request = new 
            {
                UserId = userId,
                GoalId = selectedGoalId
            };

            // Отправляем POST запрос на сервер для сохранения цели
            var response = await _httpClient.PostAsJsonAsync("Registration/saveUserGoal", request);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(resultContent);

                // Проверяем, что сообщение от сервера содержит "Пользовательские теги сохранены."
                if (result?.message == "Пользовательские теги сохранены.")
                {
                    UserInfo userInfo = new UserInfo(userId);
                    userInfo.Show();
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
