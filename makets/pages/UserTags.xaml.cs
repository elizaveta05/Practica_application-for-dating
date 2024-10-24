using makets.helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для UserTags.xaml
    /// </summary>
    public partial class UserTags : Window
    {
        private int userId;
        private readonly HttpClient _httpClient;
        private List<int> selectedTagIds = new List<int>();

        public UserTags(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
        }

        private void TagButton_Click(object sender, RoutedEventArgs e)
        {
            Button tagButton = sender as Button;
            if (tagButton != null)
            {
                string tagContent = tagButton.Content.ToString();

                // Попробуем безопасно преобразовать свойство Tag в целое число
                if (int.TryParse(tagButton.Tag.ToString(), out int tagId))
                {
                    selectedTagIds.Add(tagId);

                    double width = tagButton.Width;
                    double height = tagButton.Height;
                    TagPanel.Children.Remove(tagButton);

                    StackPanel tagStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    TextBlock tagTextBlock = new TextBlock { Text = tagContent, Margin = new Thickness(0, 0, 5, 0), FontSize = 16, FontWeight = FontWeights.Bold };
                    Button removeButton = new Button { Content = "X", Margin = new Thickness(0, 0, 5, 0), Style = (Style)FindResource("RemoveButtonStyle") };
                    removeButton.Click += (s, args) => RemoveTag_Click(tagContent, tagButton, tagStackPanel, width, height);

                    tagStackPanel.Children.Add(tagTextBlock);
                    tagStackPanel.Children.Add(removeButton);
                    SelectedTagPanel.Children.Add(tagStackPanel);
                }
                else
                {
                    MessageBox.Show("Некорректный формат тега.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RemoveTag_Click(string tagContent, Button tagButton, StackPanel tagStackPanel, double width, double height)
        {
            SelectedTagPanel.Children.Remove(tagStackPanel);
            tagButton.Width = width;
            tagButton.Height = height;
            TagPanel.Children.Add(tagButton);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SaveUserTagsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении тегов: {ex.Message}");
            }

        }

        private async Task SaveUserTagsAsync()
        {
            if (selectedTagIds == null || !selectedTagIds.Any())
            {
                MessageBox.Show("Выберите хотя бы один тег.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подготовка данных для отправки
            var data = new
            {
                tagIds = selectedTagIds, // список тегов
                userId = userId          // ID пользователя
            };

            // Конвентируем данные перед отправкой
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Отправляем на серверную часть в контроллер
            var response = await _httpClient.PostAsync("Autho/saveUserTags", content);

            // Ответ от сервера
            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(resultContent);

                // Проверяем, что сообщение от сервера содержит "Пользовательские теги сохранены."
                if (result?.message == "Пользовательские теги сохранены.")
                {
                    UserPurposes userPurposes = new UserPurposes(userId);
                    userPurposes.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось сохранить теги пользователя. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка при отправке данных: {error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class Interest
        {
            public int InterestId { get; set; }

            public string InterestName { get; set; } = null!;

        }
    }
}

