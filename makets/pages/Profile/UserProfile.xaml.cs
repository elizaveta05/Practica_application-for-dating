using makets.Model.Model_users;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace makets.pages
{
    public partial class UserProfile : Window
    {
        private int userId;
        private readonly HttpClient _httpClient;

        public UserProfile(int userid)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userid;
            LoadData(userId);
        }

        private async void LoadData(int userId)
        {
            //Конвентируем данные перед отправкой
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //Отправляем на серверную часть в контроллер
            var response = await _httpClient.PostAsync("Profile/getProfileUser", content);

            if (response.IsSuccessStatusCode)
            {
                // Чтение и парсинг ответа
                var resultContent = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<UserProfileData>(resultContent);

                if (userData != null)
                {
                    NameLabel.Content = userData.FullName ?? "Не указано";
                    AgeLabel.Content = $"{userData.Age} лет" ?? "Возраст не указан";
                    CityLabel.Content = $"г. {userData.City ?? "Не указано"}";
                    GenderLabel.Content = userData.Gender ?? "Не указано";

                    // Настройка изображения на основе пола
                    GenderFoto.Source = userData.Gender == "Мужской" ?
                        new BitmapImage(new Uri("/assets/man.png", UriKind.Relative)) :
                        new BitmapImage(new Uri("/assets/woman.png", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Данные профиля пользователя не загружаются.");
                }

            }
            else
            {
                MessageBox.Show("Ошибка загрузки данных о пользователе.");
            }
        }

        private void ChatsTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Открытие следующего окна
            //var chatsWindow = new ChatsWindow(); 
            //chatsWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditDataUser editDataUser = new EditDataUser(userId);
            editDataUser.Show();
            this.Close();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
