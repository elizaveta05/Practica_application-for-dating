using makets.Model.Model_users;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using QRCoder;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;

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
            var json = JsonConvert.SerializeObject(userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Profile/getProfileUser", content);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<UserProfileData>(resultContent);

                if (userData != null)
                {
                    NameLabel.Content = userData.FullName ?? "Не указано";
                    AgeLabel.Content = $"{userData.Age} лет" ?? "Возраст не указан";
                    CityLabel.Content = $"г. {userData.City ?? "Не указано"}";
                    GenderLabel.Content = userData.Gender ?? "Не указано";

                    await LoadUserProfilePhoto(userId, userData.Gender);
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

        private byte[] LoadDefaultImage(string gender)
        {
            string imagePath = gender == "Мужской" ? "C:/Users/elozo/Source/Repos/Practica_application-for-dating/makets/assets/man.png" :
                                                       "C:/Users/elozo/Source/Repos/Practica_application-for-dating/makets/assets/woman.png";

            return File.ReadAllBytes(imagePath);
        }

        private async Task LoadUserProfilePhoto(int userId, string gender)
        {
            var response = await _httpClient.GetAsync($"Profile/getUserPhotoProfile/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();

                if (resultContent.Contains("Фотография профиля не найдена"))
                {
                    byte[] defaultPhotoBytes = LoadDefaultImage(gender);
                    Foto.Source = ByteArrayToImage(defaultPhotoBytes);
                }
                else
                {
                    var photoData = Convert.FromBase64String(resultContent);
                    Foto.Source = ByteArrayToImage(photoData);
                }
            }
            else
            {
                byte[] defaultPhotoBytes = LoadDefaultImage(gender);
                if (defaultPhotoBytes != null)
                {
                    Foto.Source = ByteArrayToImage(defaultPhotoBytes);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditDataUser editDataUser = new EditDataUser(userId);
            editDataUser.Show();
            this.Close();
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                byte[] photoBytes;
                using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    photoBytes = new byte[stream.Length];
                    await stream.ReadAsync(photoBytes, 0, photoBytes.Length);
                }

                await UploadProfilePhoto(photoBytes);
            }
        }

        private async Task UploadProfilePhoto(byte[] photoBytes)
        {
            var photoData = Convert.ToBase64String(photoBytes);
            var content = new StringContent(JsonConvert.SerializeObject(new { UserId = userId, Photo = photoData }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Profile/postUserPhotoProfile", content);

            if (response.IsSuccessStatusCode)
            {
                Foto.Source = ByteArrayToImage(photoBytes);
                MessageBox.Show("Фото профиля обновлено успешно.");
            }
            else
            {
                MessageBox.Show("Ошибка при загрузке фото профиля.");
            }
        }

        private BitmapImage ByteArrayToImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            using (var memory = new MemoryStream(imageData))
            {
                memory.Position = 0;
                image.BeginInit();
                image.StreamSource = memory;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }
            return image;
        }
        private void ChatsTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = sender as TextBlock;

            if (clickedTextBlock != null)
            {
                switch (clickedTextBlock.Text)
                {
                    case "Чаты":
                        Chat chat = new Chat(userId);
                        chat.Show();
                        this.Close();
                        break;

                    case "Найти собеседника":
                        Search search = new Search();
                        search.Show();
                        this.Close();
                        break;
                }
            }
        }

    }
}
