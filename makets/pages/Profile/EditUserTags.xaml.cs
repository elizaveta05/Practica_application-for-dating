using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace makets.pages.Profile
{
    /// <summary>
    /// Логика взаимодействия для EditUserTags.xaml
    /// </summary>
    public partial class EditUserTags : Window
    {
        private List<int> selectedTagIds = new List<int>();
        private readonly HttpClient _httpClient;
        private int userId;
        public EditUserTags(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
            LoadData(userId);
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
                    if (!selectedTagIds.Contains(tagId))
                    {
                        selectedTagIds.Add(tagId);

                        // Убираем тег из TagPanel и добавляем в SelectedTagPanel
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

        private async Task LoadData(int userId)
        {
            // Конвертируем данные перед отправкой
            var json = JsonConvert.SerializeObject(userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Отправляем на сервер
            var response = await _httpClient.PostAsync("Profile/getUserTags", content);

            if (response.IsSuccessStatusCode)
            {
                // Чтение и парсинг ответа
                var resultContent = await response.Content.ReadAsStringAsync();
                var userTags = JsonConvert.DeserializeObject<List<int>>(resultContent);

                // Проходимся по каждому тегу в разметке
                foreach (Button tagButton in TagPanel.Children.OfType<Button>().ToList())
                {
                    if (int.TryParse(tagButton.Tag.ToString(), out int tagId) && userTags.Contains(tagId))
                    {
                        // Устанавливаем тег в выбранные, если он есть у пользователя
                        TagButton_Click(tagButton, null);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка загрузки данных о пользователе.");
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
            var response = await _httpClient.PostAsync("Registration/saveUserTags", content);

            // Ответ от сервера
            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(resultContent);

                // Проверяем, что сообщение от сервера содержит "Пользовательские теги сохранены."
                if (result?.message == "Пользовательские теги сохранены.")
                {
                    return;
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

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            await SaveUserTagsAsync();
            UserProfile userProfile = new UserProfile(userId);
            userProfile.Show();
            this.Close();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SaveUserTagsAsync();
            EditUserPurposes editUserInfo = new EditUserPurposes(userId);
            editUserInfo.Show();
            this.Close();
        }
    }
}
